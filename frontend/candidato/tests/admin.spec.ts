import { test, expect } from '@playwright/test';

// Constante para atraso de digitação (simulando humano)
const TYPE_DELAY = 100;

test.describe('Fluxos de Administração (Comportamento Humano)', () => {
  // Configura um tempo maior para o teste caso as digitações demorem
  test.setTimeout(60000);

  test('Fluxo: Admin realiza login e cadastra recrutador', async ({ page }) => {
    // 1. Acessa a tela de Login
    await page.goto('http://localhost:4200/login');
    
    // Entra com as credenciais de admin
    await page.locator('input[name="email"]').pressSequentially('admin@candidato.com', { delay: TYPE_DELAY });
    await page.locator('input[name="senha"]').pressSequentially('123456', { delay: TYPE_DELAY });
    
    // Clica no botão de login
    await page.getByRole('button', { name: /Entrar no Sistema/i, exact: true }).click();
    
    // Aguarda o login e redirecionamento
    await page.waitForTimeout(1500);
    
    // 2. Acessa o painel de administração (admin é redirecionado para /admin)
    await expect(page).toHaveURL(/.*\/admin/);
    await expect(page.getByRole('heading', { name: 'Painel Admin' })).toBeVisible();
    await page.waitForTimeout(1000);
    
    // 3. Cadastra um recrutador
    const uniqueId = Date.now().toString().slice(-6);
    await page.locator('input[name="nome"]').pressSequentially(`Recrutador Silva ${uniqueId}`, { delay: TYPE_DELAY });
    await page.locator('input[name="email"]').pressSequentially(`recrutador_${uniqueId}@empresa.com`, { delay: TYPE_DELAY });
    await page.locator('input[name="senha"]').pressSequentially('senha123', { delay: TYPE_DELAY });
    
    await page.getByRole('button', { name: /Cadastrar Recrutador/i, exact: true }).click();
    await page.waitForTimeout(1000);
    
    // Verifica mensagem de sucesso
    await expect(page.getByText(/Recrutador criado com sucesso|sucesso/i)).toBeVisible();
    await page.waitForTimeout(1500);
    
    // 4. Realiza logout
    await page.locator('.logout-btn').click();
    await page.waitForTimeout(1000);
    
    // Verifica se voltou para a página inicial ou login
    await expect(page).toHaveURL(/.*\/login|.*\//);
  });
});
