$ErrorActionPreference = "Continue"
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = { $true }
$apiUrl = "http://localhost:5260/api"

function Test-Api {
    param($Name, $Uri, $Method, $Body, $Headers)
    Write-Host "Executando: $Name..."
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
        Write-Host "ERRO EM $Name`: $_" -ForegroundColor Red
        return $null
    }
}

# 1. Registrar Recrutador
$email = "rh@empresa.com"
$regBody = @{ Nome="Recrutador Senior"; Email=$email; Senha="123" } | ConvertTo-Json
$regRes = Test-Api "Registrar RH" "$apiUrl/auth/register" "Post" $regBody $null

# 2. Login
$loginBody = @{ Email=$email; Senha="123" } | ConvertTo-Json
$loginRes = Test-Api "Login RH" "$apiUrl/auth/login" "Post" $loginBody $null
$token = $loginRes.token
if (-not $token) { $token = $loginRes.Token }
$token = $token.Trim()
$headers = @{ Authorization = "Bearer $token" }

# 3. Criar Vagas
$vagas = @(
    @{ Titulo="Desenvolvedor(a) Full Stack Pleno"; Descricao="Vaga focada em Angular e .NET"; Requisitos="Angular,.NET,SQL"; Salario=8000 },
    @{ Titulo="Analista de Dados Senior"; Descricao="Trabalhar com grandes volumes de dados"; Requisitos="Python,SQL,AWS"; Salario=12000 },
    @{ Titulo="Designer UX/UI"; Descricao="Foco em interfaces e experiência do usuário"; Requisitos="Figma,UI,UX"; Salario=7000 }
)

$vagaIds = @()
foreach ($v in $vagas) {
    $vBody = $v | ConvertTo-Json
    $vRes = Test-Api "Criar Vaga $($v.Titulo)" "$apiUrl/vagas" "Post" $vBody $headers
    $vagaIds += $vRes.id
}

# 4. Criar Candidatos e Aplicar
$candidatos = @(
    @{ Nome="Ana Silva"; Pai="Joao"; Mae="Maria"; Curso="Engenharia de Software" },
    @{ Nome="Carlos Souza"; Pai="Jose"; Mae="Fernanda"; Curso="Ciencia da Computacao" },
    @{ Nome="Beatriz Costa"; Pai="Fernando"; Mae="Juliana"; Curso="Sistemas de Informacao" },
    @{ Nome="Daniel Oliveira"; Pai="Marcos"; Mae="Leticia"; Curso="Design Digital" },
    @{ Nome="Eduardo Lima"; Pai="Roberto"; Mae="Camila"; Curso="Analise de Dados" }
)

$statuses = @("Triagem", "Entrevista", "Desafio técnico", "Oferta")
$candIds = @()

foreach ($c in $candidatos) {
    $candBody = @{
        Nome=$c.Nome
        Filiacao=@{ NomeMae=$c.Mae; NomePai=$c.Pai }
        Endereco=@{ Cep="00000000"; Logradouro="Rua"; Numero="1"; Bairro="Centro"; Cidade=@{Nome="SP"; Estado=@{Nome="SP"; Sigla="SP"}} }
        Telefones=@( @{ Numero="11999999999"; Tipo=1 } )
        Cursos=@( @{ Nome=$c.Curso; Instituicao="USP" } )
    } | ConvertTo-Json -Depth 5
    
    $candRes = Test-Api "Criar Candidato $($c.Nome)" "$apiUrl/candidato" "Post" $candBody $null
    $cId = $candRes.id
    $candIds += $cId
    
    # Aplicar à primeira vaga (Desenvolvedor Full Stack)
    $v1 = $vagaIds[0]
    $apBody = @{ VagaId=$v1; CandidatoId=$cId } | ConvertTo-Json
    Test-Api "Candidatar $($c.Nome) na Vaga 1" "$apiUrl/candidaturas/aplicar" "Post" $apBody $headers
}

# 5. Listar e alterar os status
$candList = Test-Api "Listar Candidaturas Vaga 1" "$apiUrl/candidaturas/vaga/$($vagaIds[0])" "Get" $null $headers

if ($candList) {
    for ($i = 0; $i -lt $candList.Count; $i++) {
        $cand = $candList[$i]
        $status = $statuses[$i % $statuses.Length]
        $stBody = @{ Status=$status } | ConvertTo-Json
        Test-Api "Mover $($cand.candidatoNome) para $status" "$apiUrl/candidaturas/$($cand.id)/status" "Put" $stBody $headers
    }
}

Write-Host "Banco populado com sucesso!" -ForegroundColor Green
