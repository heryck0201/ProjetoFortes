# 📚 EscolaFortes – Sistema de Gerenciamento Escolar  
Aplicação completa com **Backend em .NET 8 (DDD)** e **Frontend em React**, incluindo testes unitários com **xUnit + Moq**.

---

## 🚀 Tecnologias Utilizadas

### **Backend (.NET 8)**
- ASP.NET Core Web API  
- Entity Framework Core 9  
- SQL Server  
- DDD (Domain-Driven Design)  
- Injeção de Dependência  
- Swagger  
- xUnit  
- Moq  

### **Frontend**
- React  
- Axios  
- React Hooks  

---

## 🏗 Arquitetura do Backend (DDD)

EscolaFortesBack/
│
├── Escola.API → Controllers, Swagger, inicialização
├── Escola.Application → Serviços, DTOs, rules
├── Escola.Domain → Entidades, interfaces e regras de domínio
├── Escola.Infraestrutura → Repositórios, EF Core, Mapeamentos
├── Escola.Infra.Ioc → Injeção de dependências
└── Escola.Teste → xUnit + Moq


---
➡️ Antes de rodar, atualize para o servidor onde deseja conectar.
## 🗄 Configuração do Banco de Dados (SQL Server)

A API usa a seguinte connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Sua ConectString"
}

🏃 Como Rodar o Backend (API)

A API é executada via IIS Express dentro da pasta Escola.API.

1. Abra o projeto no Visual Studio
EscolaFortesBack/Escola.API

2. Garanta que o projeto Escola.API esteja como projeto de inicialização

Clique com botão direito → Set as Startup Project.

3. Rodar pelo IIS Express

Pressione F5 ou clique no botão:

IIS Express ▶️


A API sobe em algo como:

https://localhost:44336
https://localhost:7071
http://localhost:5071

4. Acessar Swagger
/swagger

🧪 Como Rodar os Testes Unitários
cd EscolaFortesBack/Escola.Teste
dotnet test

🎨 Como Rodar o Frontend (React)
1. Entrar na pasta do front:
cd EscolaFortesFront

2. Instalar dependências:
npm install

3. Rodar o projeto:
npm start


Aplicação estará rodando em:

http://localhost:3000

📦 Estrutura do Repositório
ProjetoFortes
│
├── EscolaFortesBack/
│   ├── Escola.API/
│   ├── Escola.Domain/
│   ├── Escola.Application/
│   ├── Escola.Infraestrutura/
│   ├── Escola.Infra.Ioc/
│   └── Escola.Teste/
│
├── EscolaFortesFront/
│
├── .gitignore
└── README.md

🔧 Funcionalidades Principais
🧑‍🎓 Gerenciamento de Alunos

Criar aluno

Editar aluno

Excluir aluno

Listar alunos

Regra: não permite cadastrar menor de idade

📘 Gerenciamento de Cursos

Criar curso

Editar curso

Excluir curso

Listar cursos

📚 Gerenciamento de Matrículas

Matricular aluno em um ou mais cursos

Listar matrículas

Remover matrícula

✔️ Projeto pronto para executar

Basta rodar:

Backend via IIS Express

Frontend via npm start