# 🛠️🚀 Motivação da criação   

A API foi criada para facilitar o controle de tarefas, permitindo aos usuários gerenciar suas listas de afazeres de maneira eficiente. Com funcionalidades de autenticação, operações CRUD, e integração com o SQL Server, esta API serve como uma ferramenta prática para organizar e acompanhar o progresso das tarefas. Além disso, ela foi desenvolvida com o objetivo de aprimorar habilidades técnicas em diversas tecnologias, como JWT, Bearer Tokens, xUnit, e Entity Framework.

---

# 💻 Tecnologias e Implementações

- **🔒 JWT**
- **🔑 Bearer Tokens**
- **🧪 xUnit**
- **🔗 Entity Framework**
- **💾 SQL Server**

---

# 📋 Funcionalidades 


## 🔒 Autenticação

- Necessario criar um usuario na mão na tabela de Usuarios com A ROle de Admin
- Logo apos enviar um post para o EndPoint "/v1/auth"
- Parametros que vao ser considerados na hora da autenticação :
    - Email 

## 🔍 Rocolher Todos as Tarefas

- Não precisa de autenticação 
- Retorna todos as Tarefas que foram inclusas no banco 

## 🔎 Recolher Tarefa pelo ID 

- Necessario autenticação 
- Recolhe somente uma Trefa de acordo com o Id 
- Id unico para cada tarefa

## 📥 Incluir tarefa

- Necessario autenticação
- Não pode encaminhar os seguintes campos vazio: 
    - Cabeçalho 
    - Conteudo
- O status da Tarefa é um enumerate :
    - 0: Pendente
    - 1: EmAndamento
    - 2: Concluido
    - 3: Cancelado
- Data e Hora é Automatico.

## 📤 Atualizar Tarefa

- Necessario autenticação
- Necessario passar o Id da Tarefa
- Id unico
- Tera que encaminhar : 
    - Cabeçalho 
    - Conteudo 
    - status  -> validar como obrigatorio <- 
- Data e Hora é Automatico.

## 🗑️ Exclução de Tarefa

- Necessario autenticação
- Necessario ID
- Id Unico 


