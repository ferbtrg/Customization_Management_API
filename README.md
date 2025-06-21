# API de Gestão de Customizações

API para gerenciar solicitações de customização de unidades imobiliárias.

## Arquitetura

Este projeto foi estruturado utilizando os princípios do Domain-Driven Design (DDD), organizando a lógica em camadas distintas:

-   **Domain**: Contém a lógica de negócio principal, entidades e objetos de valor. Não possui dependências de outras canadas.
-   **Application**: Implementa os casos de uso da aplicação, combinando com a camada de domínio para realizar tarefas.
-   **Infrastructure**: Fornece implementações para conceitos externos, como acesso ao banco de dados.
-   **API**: A camada de apresentação, que expõe as funcionalidades da aplicação através da REST API.

## Tecnologias Utilizadas

-   .NET 9
-   ASP.NET Core
-   Entity Framework Core
-   SQLite
-   Swagger (Swashbuckle)
-   xUnit
-   JWT (JSON Web Tokens)

## Como Começar

### Pré-requisitos

-   .NET 9 SDK ou superior.

### Instalação e Execução

1.  **Clone o repositório:**
    ```sh
    git clone <url-do-repositorio>
    cd <diretorio-do-projeto>
    ```

2.  **Restaure as dependências:**
    ```sh
    dotnet restore
    ```

3.  **Aplique as migrações do banco de dados:**
    Este comando irá criar o arquivo do banco de dados SQLite (`.db`) e seu esquema na raiz do projeto.
    ```sh
    dotnet ef database update
    ```

4.  **Execute a aplicação:**
    ```sh
    dotnet run
    ```
    A API estará rodando (yay). Verifique o console para ver as URLs exatas em que a aplicação está escutando (ex: `https://localhost:7172`).

### Nota Sobre o Banco de Dados

O projeto está configurado para gerar um banco de dados local (`.db`) automaticamente.
Este arquivo não foi incluído no Git. 
-> Gerar sua própria instância local do banco ao executar o comando `dotnet ef database update`. (Como informado acima)

## Uso e Testes da API

A documentação da API está disponível via Swagger.

1.  Acesse a URL do Swagger informada no console ao iniciar a aplicação (ex: `https://localhost:7172/swagger`).
2.  Para acessar os endpoints protegidos, obtenha um token de admin utilizando o endpoint `POST /api/Auth/login` com o username `"admin"`.
3.  Clique no botão **Authorize** na interface do Swagger e insira o token no formato: `Bearer {seu_token}`.
4.  Agora você pode testar todos os endpoints criados.

