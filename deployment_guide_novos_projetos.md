# Guia Definitivo: Deploy de Múltiplos Projetos na Infraestrutura Atual

Este documento foi criado para guiar você no processo de adicionar **dois novos projetos** (sendo um com upload de imagens) na sua infraestrutura atual (Oracle Cloud + Docker + GitHub Actions).

> [!WARNING]
> **Segurança:** Este documento contém referências à sua infraestrutura real. Mantenha-o seguro. Algumas senhas (como o Token do Docker Hub) não podem ser recuperadas, devendo ser recriadas caso tenham sido perdidas.

---

## 1. Credenciais e Acessos Necessários

### Acesso à Instância (Oracle Cloud VM)
A sua máquina virtual ARM (Ampere A1) possui 2 OCPUs e 12GB RAM (mais do que suficiente para suportar múltiplos projetos).
- **IP Público:** `141.148.17.246`
- **Usuário:** `ubuntu` (ou `opc` dependendo do SO da Oracle, mas o atalho já está configurado)
- **Acesso SSH:** Basta abrir o terminal e digitar `ssh portfolio`
- **Diretório atual de deploy:** `/opt/portfolio/`

### Acesso ao Docker Hub
O GitHub Actions usa o Docker Hub para hospedar a imagem do seu backend antes de baixar na Oracle Cloud.
- **Usuário Docker Hub:** `gilsonsouzadev`
- **Access Token:** O token original não fica salvo em texto plano (por segurança, ele só existe dentro dos *Secrets* do GitHub antigo). 
  - **Como obter um novo (se necessário):** Acesse [hub.docker.com](https://hub.docker.com/), vá em *Account Settings* > *Security* > *New Access Token*. Crie um token com permissão de "Read & Write" e guarde-o para o próximo passo.

### Acesso ao GitHub Actions (Secrets)
Para que os novos projetos tenham CI/CD automatizado, o repositório GitHub de cada um deles precisará das seguintes **Action Secrets** configuradas (`Settings` > `Secrets and variables` > `Actions` > `New repository secret`):
- `DOCKERHUB_TOKEN`: O token gerado no passo acima.
- `VM_HOST`: `141.148.17.246`
- `VM_USER`: `ubuntu` (ou o usuário configurado na sua chave SSH)
- `VM_SSH_KEY`: O conteúdo da sua chave privada SSH (encontrada no seu PC local em `~/.ssh/id_rsa` ou similar).

---

## 2. Elementos Faltantes (O Que Precisamos Ajustar)

Como o servidor atualmente roda apenas o Portfólio, ele provavelmente assume que "qualquer pessoa acessando o IP vai para o Portfólio". Para colocar mais dois projetos, precisamos resolver o "trânsito" das requisições:

> [!IMPORTANT]
> **1. Proxy Reverso (Nginx / Traefik):** Precisamos configurar um Proxy Reverso. Ele será a "porta de entrada" (porta 80/443). Quando uma requisição chegar para `api-loja.seu-dominio.com`, ele manda para o contêiner 2. Quando chegar para `portfolio...`, manda para o contêiner 1.
> 
> **2. Subdomínios:** Você precisará criar novos subdomínios (ex: no DuckDNS) para apontar para o mesmo IP (`141.148.17.246`), permitindo que o Proxy Reverso diferencie qual projeto o usuário quer acessar.
>
> **3. Mapeamento de Volumes (Uploads):** Para o novo projeto que terá upload de imagens, precisaremos garantir no `docker-compose.yml` dele um novo volume mapeado (ex: `- /opt/projeto2/uploads:/app/uploads`) para que as imagens não se misturem com as do portfólio.
>
> **4. Rede Docker (Docker Network):** Criaremos uma rede Docker externa (ex: `proxy-network`) para que o Nginx consiga conversar com os contêineres de todos os 3 projetos isoladamente.

---

## 3. Lista de Tarefas para o Deploy (Passo a Passo)

### Fase 1: Preparação da Infraestrutura (Oracle VM)
- [ ] Conectar na VM via `ssh portfolio`.
- [ ] Criar a rede compartilhada do Docker: `docker network create web-network`.
- [ ] Modificar o `docker-compose.yml` atual do portfólio para conectar nesta nova rede.
- [ ] Criar pastas para os novos projetos: `mkdir -p /opt/projeto2/uploads` e `mkdir -p /opt/projeto3`.
- [ ] Instalar e configurar um Proxy Reverso Global (Nginx) rodando em um Docker isolado, mapeado para a porta 80/443.

### Fase 2: Configuração dos Novos Projetos (Repositórios Locais)
Para cada um dos 2 novos projetos:
- [ ] Criar o arquivo `Dockerfile` na raiz do Backend.
- [ ] Criar o arquivo `docker-compose.yml` apontando a imagem para o Docker Hub, mapeando os volumes de imagem (ex: `/opt/projeto2/uploads:/app/images`), e conectando à rede `web-network`.
- [ ] Criar a pasta `.github/workflows/deploy.yml` baseada no script do Portfólio, ajustando os nomes das imagens no Docker Hub.
- [ ] No repositório do GitHub de cada projeto, adicionar as 4 Secrets (`DOCKERHUB_TOKEN`, `VM_HOST`, `VM_USER`, `VM_SSH_KEY`).

### Fase 3: Ajustes de Banco de Dados e DNS
- [ ] Se os novos projetos usarem banco de dados, configurar os schemas/usuários no Oracle Database (ou em um Postgres rodando em Docker na própria VM).
- [ ] Criar as URLs no DuckDNS para os novos projetos (ex: `projeto2-gilson.duckdns.org`), apontando para o IP `141.148.17.246`.
- [ ] Configurar o arquivo `nginx.conf` do proxy reverso para interceptar o domínio novo e redirecionar para a porta do contêiner do Projeto 2.

### Fase 4: Execução
- [ ] Fazer um `git push origin main` no Projeto 2.
- [ ] Acompanhar o GitHub Actions fazer o Build, enviar para o Docker Hub, acessar a VM por SSH e rodar o `docker compose up -d`.
- [ ] Testar o upload de imagens na URL de produção do Projeto 2 para garantir que as permissões da pasta `/opt/projeto2/uploads` estão corretas.

---

> [!TIP]
> **Próximo passo sugerido:** Recomendo começarmos criando o **Proxy Reverso Global** na sua máquina Oracle. Se quiser, me avise e eu preparo a configuração do Nginx e a Rede Docker unificada para prepararmos o terreno!
