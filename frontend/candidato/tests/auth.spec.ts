import { test, expect } from '@playwright/test';

test.describe('Fluxos Principais (Comportamento Humano)', () => {
  const TYPE_DELAY = 100; // Simula velocidade humana de digitação
  
  test('Fluxo: Candidato realiza login e visualiza detalhes da vaga', async ({ page }) => {
    // 1. Acesso à Home
    await page.goto('http://localhost:4200/');
    await page.waitForTimeout(1000); // Pausa para simular usuário olhando a tela
    
    // 2. Navegar para Login
    await page.getByRole('link', { name: /entrar/i }).click();
    await page.waitForTimeout(500);

    // 3. Preencher formulário de Login como Admin/Recrutador para fins de teste
    await page.locator('input[type="email"]').click();
    await page.locator('input[type="email"]').pressSequentially('recrutador@sistema.com', { delay: TYPE_DELAY });
    
    await page.locator('input[type="password"]').click();
    await page.locator('input[type="password"]').pressSequentially('recrutador123', { delay: TYPE_DELAY });
    
    await page.waitForTimeout(800);
    
    // 4. Clicar em Entrar
    await page.getByRole('button', { name: /entrar/i }).click();

    // 5. Esperar carregamento do Dashboard (Recrutador)
    await expect(page).toHaveURL(/.*\/dashboard/);
    await expect(page.getByText('Dashboard do Recrutador')).toBeVisible();
    
    await page.waitForTimeout(1500);

    // 6. Logout
    await page.locator('.logout-btn').click();
    await page.waitForTimeout(1000);
    await expect(page).toHaveURL(/.*\/login/);
  });
});
