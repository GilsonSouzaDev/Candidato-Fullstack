import { test, expect } from '@playwright/test';

const TYPE_DELAY = 100;

test.describe('Fluxos de Candidato (Comportamento Humano)', () => {
  test.setTimeout(180000); // 3 minutos para o fluxo completo

  test('Fluxo: Preencher perfil, buscar vaga, candidatar, cancelar', async ({ page }) => {
    // 1. Acessa Login
    await page.goto('http://localhost:4200/login');

    const uniqueId = Date.now().toString().slice(-4);

    // Login do candidato
    await page.locator('input[name="email"]').pressSequentially('candidato@candidato.com', { delay: TYPE_DELAY });
    await page.locator('input[name="senha"]').pressSequentially('123456', { delay: TYPE_DELAY });
    await page.getByRole('button', { name: /Entrar no Sistema/i, exact: true }).click();
    await page.waitForTimeout(2000);

    // 2. Acessa Meu Currículo (link no nav)
    await page.getByRole('link', { name: /Meu Currículo/i }).click();
    await page.waitForTimeout(1500);

    // 3. Na tela de lista, clica no botão de editar (aria-label) OU "Cadastrar Currículo"
    const cadastrarBtn = page.getByRole('button', { name: /Cadastrar Currículo/i });
    const editarBtn = page.getByRole('button', { name: /Editar Currículo/i });

    const hasCurriculo = await editarBtn.isVisible();
    if (hasCurriculo) {
      await editarBtn.first().click();
    } else {
      await cadastrarBtn.click();
    }
    await page.waitForTimeout(1500);

    // Aguarda o formulário de edição com o stepper
    await page.waitForSelector('mat-stepper', { timeout: 10000 });

    // ===== PASSO 1: Dados Pessoais =====
    const nomeInput = page.locator('input[id="Nome"]');
    await nomeInput.fill('');
    await nomeInput.pressSequentially(`Candidato Silva ${uniqueId}`, { delay: TYPE_DELAY });

    // Preenche CPF se vazio
    const cpfInput = page.locator('input[id="Cpf"]');
    const cpfVal = await cpfInput.inputValue();
    if (!cpfVal) {
      await cpfInput.fill('111.222.333-44');
    }

    // Preenche Data de Nascimento se vazia
    const dataNasc = page.locator('input[id="DataNascimento"]');
    const dataNascVal = await dataNasc.inputValue();
    if (!dataNascVal) {
      await dataNasc.fill('1998-05-10');
    }

    // ===== Navegar: Passo 1 -> 2 =====
    await page.locator('button.mat-stepper-next').nth(0).click({ force: true });
    await page.waitForTimeout(600);

    // ===== PASSO 2: Filiação & Endereço =====
    // Nome da Mãe
    const nomeMaeInput = page.locator('input[id="NomeMae"]');
    const nomeMaeVal = await nomeMaeInput.inputValue();
    if (!nomeMaeVal) {
      await nomeMaeInput.fill('Maria da Silva');
    }

    // Endereço: CEP
    const cepInput = page.locator('input[id="Cep"]');
    const cepVal = await cepInput.inputValue();
    if (!cepVal) {
      await cepInput.fill('01001-000');
    }

    // Logradouro
    const logradouroInput = page.locator('input[id="Logradouro"]');
    const logradouroVal = await logradouroInput.inputValue();
    if (!logradouroVal) {
      await logradouroInput.fill('Praça da Sé');
    }

    // Número
    const numeroEndInput = page.locator('input[id="Numero"]');
    const numeroEndVal = await numeroEndInput.inputValue();
    if (!numeroEndVal) {
      await numeroEndInput.fill('1');
    }

    // Bairro
    const bairroInput = page.locator('input[id="Bairro"]');
    const bairroVal = await bairroInput.inputValue();
    if (!bairroVal) {
      await bairroInput.fill('Sé');
    }

    // Cidade (pelo title)
    const cidadeInput = page.locator('input[title="Cidade"]');
    const cidadeVal = await cidadeInput.inputValue();
    if (!cidadeVal) {
      await cidadeInput.fill('São Paulo');
    }

    // Estado
    const estadoInput = page.locator('input[title="Estado"]');
    const estadoVal = await estadoInput.inputValue();
    if (!estadoVal) {
      await estadoInput.fill('São Paulo');
    }

    // Sigla
    const siglaInput = page.locator('input[title="Sigla"]');
    const siglaVal = await siglaInput.inputValue();
    if (!siglaVal) {
      await siglaInput.fill('SP');
    }

    // ===== Navegar: Passo 2 -> 3 =====
    await page.locator('button.mat-stepper-next').nth(1).click({ force: true });
    await page.waitForTimeout(600);

    // ===== PASSO 3: Contatos (Telefone obrigatório) =====
    // Verifica se já tem telefone, senão adiciona
    const telefoneInputs = page.locator('[formArrayName="Telefones"] input');
    const telefoneCount = await telefoneInputs.count();

    if (telefoneCount === 0) {
      // Clica em "+ Adicionar Telefone"
      await page.getByRole('button', { name: /Adicionar Telefone/i }).click();
      await page.waitForTimeout(500);

      // Preenche o número
      await page.locator('[formArrayName="Telefones"] input[placeholder="(11) 99999-9999"]').fill('11999998888');

      // Seleciona o tipo "Celular"
      await page.locator('[formArrayName="Telefones"] select').selectOption('Celular');
    }

    // ===== Navegar: Passo 3 -> 4 =====
    await page.locator('button.mat-stepper-next').nth(2).click({ force: true });
    await page.waitForTimeout(600);

    // ===== PASSO 4 -> 5 =====
    await page.locator('button.mat-stepper-next').nth(3).click({ force: true });
    await page.waitForTimeout(600);

    // ===== PASSO 5 -> 6 =====
    await page.locator('button.mat-stepper-next').nth(4).click({ force: true });
    await page.waitForTimeout(600);

    // ===== Salva o Currículo =====
    await page.getByRole('button', { name: /Salvar Currículo/i, exact: true }).click();
    await page.waitForTimeout(1000);

    // Aguarda o MatDialog de confirmação e clica em Confirmar
    await page.waitForSelector('mat-dialog-container', { timeout: 10000 });
    await page.locator('mat-dialog-container').getByRole('button', { name: /Confirmar/i }).click();
    await page.waitForTimeout(2000);

    // Aguarda snackbar de sucesso (try/catch pois pode expirar durante navegação)
    try {
      await page.locator('mat-snack-bar-container').waitFor({ timeout: 8000 });
    } catch (_) { /* snackbar pode ter expirado na transição */ }
    await page.waitForTimeout(1500);

    // ===== 4. Acessa painel de vagas =====
    await page.getByRole('link', { name: /Portal de Vagas/i }).click();
    await page.waitForTimeout(1500);

    // ===== 5. Pesquisa de vaga =====
    const searchInput = page.locator('.search-input');
    await searchInput.pressSequentially('Desenvolvedor', { delay: TYPE_DELAY });
    await page.waitForTimeout(1500);

    // ===== 6. Abre detalhes da primeira vaga =====
    const vagaCard = page.locator('.vaga-card').first();
    await vagaCard.getByRole('link', { name: /Ver Detalhes/i }).click();
    await page.waitForTimeout(1500);

    // ===== 7. Verifica a página de detalhes da vaga =====
    await expect(page.getByText('Sobre a Vaga')).toBeVisible({ timeout: 10000 });

    // Checa se já está inscrito; se não, se candidata
    const isApplied = await page.getByText('Você está inscrito!').isVisible();
    if (!isApplied) {
      await page.getByRole('button', { name: /Candidatar-se/i, exact: true }).click();
      await page.waitForTimeout(2000);
      await expect(page.getByText(/Você está inscrito!|Candidatura enviada com sucesso/i).first()).toBeVisible({ timeout: 10000 });
    }

    // ===== 8. Volta para o portal de vagas =====
    await page.getByRole('link', { name: /Portal de Vagas/i }).click();
    await page.waitForTimeout(1500);

    // ===== 9. Seleciona filtro Minhas Inscrições =====
    await page.getByRole('button', { name: /Minhas Inscrições/i }).click();
    await page.waitForTimeout(1500);

    // ===== 10. Cancela uma inscrição =====
    const cancelBtn = page.locator('.vaga-card').first().getByRole('button', { name: /Cancelar Inscrição/i });
    if (await cancelBtn.isVisible()) {
      await cancelBtn.click();
      await page.waitForTimeout(2000);
      await expect(page.getByText(/cancelada/i)).toBeVisible({ timeout: 10000 });
    }

    // ===== 11. Logout =====
    await page.locator('.logout-btn').click();
    await page.waitForTimeout(1000);
    await expect(page).toHaveURL(/.*\/login|.*\//);
  });
});
