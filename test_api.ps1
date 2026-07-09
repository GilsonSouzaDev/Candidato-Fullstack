$ErrorActionPreference = "Continue"
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = { $true }
$apiUrl = "https://localhost:7246/api"

Write-Host "1. Fechando processos antigos nas portas 5260 e 7246..."
try {
    $proc = Get-Process -Id (Get-NetTCPConnection -LocalPort 5260 -ErrorAction SilentlyContinue).OwningProcess -ErrorAction SilentlyContinue
    if ($proc) { Stop-Process -Id $proc.Id -Force }
} catch {}
try {
    $proc2 = Get-Process -Id (Get-NetTCPConnection -LocalPort 7246 -ErrorAction SilentlyContinue).OwningProcess -ErrorAction SilentlyContinue
    if ($proc2) { Stop-Process -Id $proc2.Id -Force }
} catch {}

Write-Host "2. Iniciando o Backend..."
$apiProcess = Start-Process -FilePath "dotnet" -ArgumentList "run" -WorkingDirectory "c:\workspace\candidato-fullstack\backend\candidato" -PassThru -NoNewWindow
Start-Sleep -Seconds 7

Write-Host "--- TESTES DA API ---"
$results = @()
$random = Get-Random
$email = "teste$random@recrutador.com"

function Test-Api {
    param($Name, $Uri, $Method, $Body, $Headers)
    Write-Host "Testando: $Name..."
    try {
        if ($Body -and $Headers) {
            $res = Invoke-RestMethod -Uri $Uri -Method $Method -Body $Body -Headers $Headers -ContentType "application/json"
        } elseif ($Body) {
            $res = Invoke-RestMethod -Uri $Uri -Method $Method -Body $Body -ContentType "application/json"
        } elseif ($Headers) {
            $res = Invoke-RestMethod -Uri $Uri -Method $Method -Headers $Headers -ContentType "application/json"
        } else {
            $res = Invoke-RestMethod -Uri $Uri -Method $Method -ContentType "application/json"
        }
        return $res
    } catch {
        if ($_.Exception.Response) {
            $stream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($stream)
            $err = $reader.ReadToEnd()
            Write-Host "ERRO EM $Name`: $err" -ForegroundColor Red
            Write-Host "HEADERS: $($_.Exception.Response.Headers.ToString())" -ForegroundColor Yellow
        } else {
            Write-Host "ERRO EM $Name`: $_" -ForegroundColor Red
        }
        return $null
    }
}

# 1. Registrar
$regBody = @{ Nome="Recrutador Teste"; Email=$email; Senha="123" } | ConvertTo-Json
$regRes = Test-Api "Registrar" "$apiUrl/auth/register" "Post" $regBody $null
if ($regRes) { $results += "Registro: $($regRes.nome) / $($regRes.email)" }

# 2. Login
$loginBody = @{ Email=$email; Senha="123" } | ConvertTo-Json
$loginRes = Test-Api "Login" "$apiUrl/auth/login" "Post" $loginBody $null
$token = $loginRes.token
if (-not $token) {
    $token = $loginRes.Token
}
$token = $token.Trim()
$headers = @{ Authorization = "Bearer $token" }
if ($token) { $results += "Login: Sucesso (Token JWT Recebido)" }

# 3. Criar Vaga
$vagaBody = @{ Titulo="Desenvolvedor C#"; Descricao="Vaga Fullstack"; Requisitos="C# e Angular"; Salario=5000 } | ConvertTo-Json
$vagaRes = Test-Api "Criar Vaga" "$apiUrl/vagas" "Post" $vagaBody $headers
$vagaId = $vagaRes.id
if ($vagaId) { $results += "Vaga Criada: ID $vagaId - $($vagaRes.titulo)" }

# 4. Criar Candidato
$candBody = @{
    Nome="Joao Testador $random"
    Filiacao=@{ NomeMae="Maria Testador"; NomePai="Jose" }
    Endereco=@{ Cep="12345678"; Logradouro="Rua ABC"; Numero="1"; Bairro="Centro"; Cidade=@{Nome="Sao Paulo"; Estado=@{Nome="Sao Paulo"; Sigla="SP"}} }
    Telefones=@( @{ Numero="11999999999" } )
    Cursos=@( @{ Nome="Ciencia da Computacao"; Instituicao="USP" } )
} | ConvertTo-Json -Depth 5
$candRes = Test-Api "Criar Candidato" "$apiUrl/candidato" "Post" $candBody $null
$candId = $candRes.id
if ($candId) { $results += "Candidato Criado: ID $candId - $($candRes.nome)" }

# 5. Aplicar
if ($vagaId -and $candId) {
    $aplicarBody = @{ VagaId=$vagaId; CandidatoId=$candId } | ConvertTo-Json
    $aplicarRes = Test-Api "Aplicar a Vaga" "$apiUrl/candidaturas/aplicar" "Post" $aplicarBody $headers
    if ($aplicarRes) { $results += "Candidatura: $($aplicarRes.message)" }

    # 6. Listar Candidaturas
    $candDaVagaList = Test-Api "Listar Candidaturas" "$apiUrl/candidaturas/vaga/$vagaId" "Get" $null $headers
    if ($candDaVagaList -and $candDaVagaList.Count -gt 0) {
        $candDaVaga = $candDaVagaList[0]
        $candidaturaId = $candDaVaga.id
        $results += "Listar Candidatos: $($candDaVaga.candidatoNome) - $($candDaVaga.statusCandidatura)"
        
        # 7. Atualizar Status
        $statusBody = @{ Status="Aprovado!" } | ConvertTo-Json
        $statusRes = Test-Api "Atualizar Status" "$apiUrl/candidaturas/$candidaturaId/status" "Put" $statusBody $headers
        if ($statusRes) { $results += "Atualizar Status: $($statusRes.message)" }
    }
}

Write-Host "--- RESULTADOS FINAIS ---"
$results | ForEach-Object { Write-Host $_ }

Write-Host "Fechando Backend..."
Stop-Process -Id $apiProcess.Id -Force
