<<<<<<< HEAD
# Good Hamburger - Teste Técnico

Olá! Este projeto é um sistema de pedidos para uma lanchonete, desenvolvido como parte de um teste técnico. O objetivo principal foi entregar uma aplicação funcional, segura e que seja bem fácil de rodar para quem for avaliar.

## Escolhas Técnicas

*   **Stack**: Usei .NET 8 e Blazor WebAssembly. Escolhi o .NET 8 por ser a versão que tenho mais familiaridade e o Blazor para manter o projeto inteiro em C#.
*   **Banco de Dados**: Optei pelo PostgreSQL.
*   **Docker**: Containerizei a aplicação inteira. Fiz isso para facilitar o teste: você não precisa instalar banco de dados nem configurar nada na sua máquina, é só subir os containers e o sistema já sai funcionando com os dados iniciais.
*   **Testes**: Usei xUnit com Moq e FluentAssertions para os testes de unidade.

## Organização do Código

Para a parte de descontos, em vez de encher o código de condicionais (`if/else`), implementei um **Motor de Regras**. Usei o padrão Strategy para que cada promoção seja uma classe independente. Isso facilita muito se o cardápio crescer e precisarmos de novos combos.

Também adicionei algumas camadas de segurança:
*   **Rate Limiting**: Bloqueio de excesso de requisições para evitar abusos na API.
*   **Isolamento do Banco**: O Postgres só aceita conexões de dentro do Docker. Ele não expõe porta para o Windows, mantendo os dados protegidos.
*   **Aba de Diagnóstico**: Criei uma aba no menu lateral chamada "Diagnóstico". Ela serve para você testar as regras de negócio e ver os logs das travas de segurança funcionando na prática.

## Como rodar

Basta ter o Docker instalado, abrir o terminal na raiz do projeto e rodar:

```bash
docker-compose up -d --build
```

Depois de subir, você pode acessar:
*   **App (Blazor)**: http://localhost:8080
*   **Swagger (API)**: http://localhost:5000/swagger

## Testes Unitários

Se quiser rodar os testes via terminal, use:
```bash
dotnet test
```

---
**Daniel Igor**
=======
# Good-Hamburger
Sistema para lanchonete, desenvolvido como parte de um teste técnico, com foco em organização de código, escalabilidade, segurança e facilidade de execução
>>>>>>> 82eaa3bd164f1bf4bdc63eb81df8bc92aa4848a9
