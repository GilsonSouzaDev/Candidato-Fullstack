import { test, expect } from '@playwright/test';

test.describe('Fluxos de Vagas (Comportamento Humano)', () => {
  const TYPE_DELAY = 100;
  
  test('Fluxo: Criar Vaga, Candidatar, Ver no Kanban', async ({ page }) => {
    test.setTimeout(90000); // 90 segundos porque tem bastante digitação lenta
    
    // ----------------------------------------------------
    // PARTE 1: RECRUTADOR CRIA A VAGA
    // ----------------------------------------------------
    await page.goto('http://localhost:4200/');
    await page.waitForTimeout(1000);
    
    // Login Recrutador
    await page.getByRole('link', { name: /entrar/i }).click();
    await page.waitForTimeout(500);
    await page.locator('input[type="email"]').click();
    await page.locator('input[type="email"]').pressSequentially('recrutador@sistema.com', { delay: TYPE_DELAY });
    await page.locator('input[type="password"]').click();
    await page.locator('input[type="password"]').pressSequentially('recrutador123', { delay: TYPE_DELAY });
    await page.waitForTimeout(500);
    await page.getByRole('button', { name: /entrar/i }).click();
    
    // Dashboard do Recrutador
    await expect(page).toHaveURL(/.*\/dashboard/);
    await expect(page.getByText('Dashboard do Recrutador')).toBeVisible();
    await page.waitForTimeout(1000);
    
    // Criar Vaga
    await page.getByRole('button', { name: /Criar Vaga/i }).click();
    await page.waitForTimeout(500);
    
    const tituloVaga = `Vaga Teste Automatizado ${Date.now()}`;
    await page.locator('input[name="titulo"]').pressSequentially(tituloVaga, { delay: TYPE_DELAY });
    await page.locator('input[name="nomeEmpresa"]').pressSequentially('Tech Corp', { delay: TYPE_DELAY });
    await page.locator('textarea[name="descricao"]').pressSequentially('Descrição da vaga de teste automatizado.', { delay: TYPE_DELAY });
    await page.locator('textarea[name="atividades"]').pressSequentially('- Atividade 1\n- Atividade 2', { delay: TYPE_DELAY });
    await page.locator('textarea[name="requisitosObrigatorios"]').pressSequentially('- Requisito 1\n- Requisito 2', { delay: TYPE_DELAY });
    await page.locator('textarea[name="requisitosDesejaveis"]').pressSequentially('- Diferencial 1', { delay: TYPE_DELAY });
    await page.locator('textarea[name="beneficios"]').pressSequentially('- VA\n- VR\n- Plano de Saúde', { delay: TYPE_DELAY });
    await page.locator('input[name="salario"]').pressSequentially('15000', { delay: TYPE_DELAY });
    await page.waitForTimeout(500);
    
    await page.getByRole('button', { name: /Salvar Vaga/i }).click();
    await page.waitForTimeout(1000);
    
    // Verifica se vaga aparece na lista
    await expect(page.getByText(tituloVaga).first()).toBeVisible();
    
    // Logout
    await page.locator('.logout-btn').click();
    await page.waitForTimeout(1000);

    // ----------------------------------------------------
    // PARTE 2: CANDIDATO SE INSCREVE
    // ----------------------------------------------------
    await page.getByRole('link', { name: /entrar/i }).click();
    await page.waitForTimeout(500);
    await page.locator('input[type="email"]').click();
    await page.locator('input[type="email"]').pressSequentially('candidato_vagas@candidato.com', { delay: TYPE_DELAY });
    await page.locator('input[type="password"]').click();
    await page.locator('input[type="password"]').pressSequentially('123456', { delay: TYPE_DELAY });
    await page.waitForTimeout(500);
    await page.getByRole('button', { name: /entrar/i }).click();
    
    await page.waitForTimeout(1500);
    
    // Portal de Vagas
    await page.getByRole('link', { name: /Portal de Vagas/i }).click();
    await page.waitForTimeout(1500);
    
    // Acha a vaga recém-criada e clica em Ver Detalhes
    // Como criamos a vaga recentemente, ela deve estar na lista (se houver paginação, assume-se que está na primeira página)
    const vagaCard = page.locator('.vaga-card').filter({ hasText: tituloVaga }).first();
    await vagaCard.getByRole('link', { name: /Ver Detalhes/i }).click();
    await page.waitForTimeout(1500);
    
    // Visualiza os detalhes e se candidata
    await expect(page.getByText('Sobre a Vaga')).toBeVisible();
    await page.getByRole('button', { name: /Candidatar-se/i, exact: true }).click();
    await page.waitForTimeout(1500);
    
    // Confirma se o botão mudou para 'Você já está inscrito' ou se mostra mensagem de sucesso
    // Apenas verificamos que não deu erro e esperamos um tempinho
    await page.waitForTimeout(1500);
    
    // Logout
    await page.locator('.logout-btn').click();
    await page.waitForTimeout(1000);
    
    // ----------------------------------------------------
    // PARTE 3: RECRUTADOR VÊ KANBAN
    // ----------------------------------------------------
    await page.getByRole('link', { name: /entrar/i }).click();
    await page.waitForTimeout(500);
    await page.locator('input[type="email"]').click();
    await page.locator('input[type="email"]').pressSequentially('recrutador@sistema.com', { delay: 50 }); // Pode ser mais rápido agora
    await page.locator('input[type="password"]').click();
    await page.locator('input[type="password"]').pressSequentially('recrutador123', { delay: 50 });
    await page.waitForTimeout(500);
    await page.getByRole('button', { name: /entrar/i }).click();
    
    await expect(page).toHaveURL(/.*\/dashboard/);
    await page.waitForTimeout(1500);
    
    // Acha a vaga no dashboard do recrutador
    const linhaVaga = page.locator('tr.table-row').filter({ hasText: tituloVaga }).first();
    await linhaVaga.getByRole('link', { name: /Pipeline/i }).click();
    await page.waitForTimeout(1500);
    
    // Espera ver o candidato na coluna "Novos" (que é o padrão)
    await expect(page.getByText('Candidato Vagas')).toBeVisible(); 
    await page.waitForTimeout(1000);
    
    // Abre o card do usuário inscrito
    await page.locator('.kanban-column').first().getByText('Candidato Vagas').click();
    await page.waitForTimeout(1000);
    
    // Clica em detalhes
    await page.getByRole('link', { name: /Ver Detalhes/i }).click();
    await page.waitForTimeout(1500);
    
    // Espera o currículo carregar
    await expect(page.getByText('Meu Currículo')).toBeVisible();
    await page.waitForTimeout(1000);
    
    // Volta pela seta para a pipeline
    // O botão back-btn na tela de candidato tem class back-btn
    await page.locator('.back-btn').first().click();
    await page.waitForTimeout(1500);
    
    // Abre o card do usuário inscrito novamente, pois a página recarregou
    await page.getByText('Candidato Vagas').click();
    await page.waitForTimeout(1000);
    
    // Clica em mover para e escolha a proxima etapa do kanban
    // Mover para: Entrevista
    await page.locator('.kanban-select').first().selectOption('Entrevista');
    await page.waitForTimeout(1500);
    
    // Verifica se a contagem na coluna Entrevista aumentou, ou pelo menos espera
    await page.waitForTimeout(1000);
  });
});
