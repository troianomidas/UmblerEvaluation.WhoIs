# Desafio Umbler

Esta é uma aplicação web que recebe um domínio e mostra suas informações de DNS.

Este é um exemplo real de sistema que utilizamos na Umbler.

Ex: Consultar os dados de registro do dominio `umbler.com`

**Retorno:**
- Name servers (ns254.umbler.com)
- IP do registro A (177.55.66.99)
- Empresa que está hospedado (Umbler)

Essas informações são descobertas através de consultas nos servidores DNS e de WHOIS.

*Obs: WHOIS (pronuncia-se "ruís") é um protocolo específico para consultar informações de contato e DNS de domínios na internet.*

Nesta aplicação, os dados obtidos são salvos em um banco de dados, evitando uma segunda consulta desnecessaria, caso seu TTL ainda não tenha expirado.

*Obs: O TTL é um valor em um registro DNS que determina o número de segundos antes que alterações subsequentes no registro sejam efetuadas. Ou seja, usamos este valor para determinar quando uma informação está velha e deve ser renovada.*

Tecnologias Backend utilizadas:

- C#
- Asp.Net Core
- MySQL
- Entity Framework

Tecnologias Frontend utilizadas:

- Webpack
- Babel
- ES7

Para rodar o projeto você vai precisar instalar:

- dotnet Core SDK (https://www.microsoft.com/net/download/windows dotnet Core 6.0.201 SDK)
- Um editor de código, acoselhamos o Visual Studio ou VisualStudio Code. (https://code.visualstudio.com/)
- NodeJs v17.6.0 para "buildar" o FrontEnd (https://nodejs.org/en/)
- Um banco de dados MySQL (vc pode rodar localmente ou criar um site PHP gratuitamente no app da Umbler https://app.umbler.com/ que lhe oferece o banco Mysql adicionamente)

Com as ferramentas devidamente instaladas, basta executar os seguintes comandos:

Para "buildar" o javascript basta executar:

`npm install`
`npm run build`

Para Rodar o projeto:

Execute a migration no banco mysql:

`dotnet tool update --global dotnet-ef`
`dotnet tool ef database update`

E após: 

`dotnet run` (ou clique em "play" no editor do vscode)

# Objetivos:

Se você rodar o projeto e testar um domínio, verá que ele já está funcionando. Porém, queremos melhorar varios pontos deste projeto:

# FrontEnd

 - Os dados retornados não estão formatados, e devem ser apresentados de uma forma legível.
 - Não há validação no frontend permitindo que seja submetido uma requsição inválida para o servidor (por exemplo, um domínio sem extensão).
 - Está sendo utilizado "vanilla-js" para fazer a requisição para o backend, apesar de já estar configurado o webpack. O ideal seria utilizar algum framework mais moderno como ReactJs ou Blazor.  

# BackEnd

 - Não há validação no backend permitindo que uma requisição inválida prossiga, o que ocasiona exceptions (erro 500).
 - A complexidade ciclomática do controller está muito alta, o ideal seria utilizar uma arquitetura em camadas.
 - O DomainController está retornando a própria entidade de domínio por JSON, o que faz com que propriedades como Id, Ttl e UpdatedAt sejam mandadas para o cliente web desnecessariamente. Retornar uma ViewModel (DTO) neste caso seria mais aconselhado.

# Testes

 - A cobertura de testes unitários está muito baixa, e o DomainController está impossível de ser testado pois não há como "mockar" a infraestrutura.
 - O Banco de dados já está sendo "mockado" graças ao InMemoryDataBase do EntityFramework, mas as consultas ao Whois e Dns não. 

# Dica

- Este teste não tem "pegadinha", é algo pensado para ser simples. Aconselhamos a ler o código, e inclusive algumas dicas textuais deixadas nos testes unitários. 
- Há um teste unitário que está comentado, que obrigatoriamente tem que passar.
- Diferencial: criar mais testes.

# Entrega

- Enviei o link do seu repositório com o código atualizado.
- O repositório deve estar público para que possamos acessar..
- Modifique Este readme adicionando informações sobre os motivos das mudanças realizadas.

# Modificações:

# Arquitetura e organização (Hexagonal / Ports & Adapters)
- Antes: MVC “tudo no controller”, WHOIS/DNS, regra de TTL e persistência misturados.
- Agora: Arquitetura Hexagonal com camadas explícitas:
	- Domain: entidades, invariantes e validações de domínio.
	- Application: CQRS com MediatR (casos de uso como CreateDomainCommand), FluentValidation e AutoMapper.
    - Infrastructure: EF Core + mappings, repositórios e migrações (adapter de persistência).
	- WebApp: Blazor Server como driver (UI), chama use cases via IMediator (sem violar domínio).

Motivo: Por que: separação clara de responsabilidades, baixo acoplamento e alta testabilidade; fica mais fácil evoluir (ex.: trocar banco, expor API, usar fila/worker) sem tocar na regra de negócio.

# De Controller anêmico → CQRS com pipeline de validação
- Introduzimos MediatR e um ValidationBehavior (pipeline) com FluentValidation:
	- O handler faz só o que precisa (orquestra repositório e mapping).
	- As regras de validação ficam nos validators (ex.: domínio deve conter “.” e TLD ≥ 2, IP válido, TTL ≥ 0 etc.).
- DTO/Response específicos (ex.: CreateDomainCommandResponse) evitam over-posting/leak de propriedades internas.

Motivo: controllers/Componentes leves; regras e validações centrais e reutilizáveis; fica muito mais fácil de testar.

# Persistência e migrações

- Antes: MySQL acoplado no controller.
- Agora: PostgreSQL com EF Core 9, DefaultContext e mapping fluente (DomainConfig).
- Timestamps corrigidos: created_at/updated_at como timestamptz (UTC de verdade).
- Índice único em domain_name para evitar duplicatas.
- Makefile + docker-compose para subir Postgres e aplicar migrações (inclui senha URL-encoded).
- Connection string com timeouts configuráveis.

Motivo: infra reprodutível, previsível e segura contra duplicidades/race; sem erros de Kind (UTC vs timestamp).

# Lógica de reuso por TTL (cache sem duplicar)

- Repositório com GetByDomainNameAsync.
- Extensão IsFresh(now) para checar TTL (com base em UpdatedAt/CreatedAt).
- Handler:
	- Valida o comando.
	- Busca por domínio existente.
	- Se fresh → retorna sem consultar de novo.
	- Se stale/inexistente → cria um novo registro.

Motivo: evita consultas externas desnecessárias; economiza latência e chamadas WHOIS/DNS.

# UI com Blazor Server (responsivo e simples)

- Componente Counter.razor passa a:
	- Validar entrada básica.
	- Resolver DNS (A record) com DnsClient (timeout curto).
	- Chamar WHOIS com CancellationTokenSource específico (5s).
	- Enviar Command via IMediator.
	- Não reaproveitar o token do WHOIS pro DB (evita TaskCanceledException).
- Tratamento de exceções e logs (ILogger).

Motivo: UX melhor, sem deadlocks (nada de .Result/.Wait()), front desacoplado do domínio.

# Validações robustas (cliente e servidor)

- Servidor (FluentValidation):
	- Domínio com . e TLD ≥ 2; IP v4 válido; TTL ≥ 0; campos obrigatórios.
- Middleware de validação converte ValidationException em resposta consistente;

Motivo: segurança + DX. Evita 500 por entrada inválida, padroniza mensagens.

# Testes: mais cobertura, mais rápidos e previsíveis

- Unitários de domínio: validações de entidade e invariantes.
- Unitários de application:
	- Handler com AutoMapper real (profile) e/ou mock.
	- Builders + Bogus para fixtures estáveis (CreateDomainCommandBuilder, etc.).
	- Repository com EF InMemory/SQLite (sem bater no Postgres).
- Crosscutting/pipeline: testes do fluxo MediatR + ValidationBehavior (sem web/infra real).
- Sem dependência de WHOIS/DNS reais nos testes (mocks/stubs).

Motivo: testes rápidos e confiáveis; regressões pegam cedo; o design fica orientado à testabilidade.

PS.: Utilizei o Bogus somente para demonstrar que com entidades mais complexas é possível automatizar mocks e stubs. 
O mesmo se aplica para a controller. Não há necessidade dela existir, para um aplicação específica dessa o BlazorServer e o MediatR cumprem bem o papel
de controlador (sem violar o domínio).

A aplicação inteira em si é uma bazuca para matar uma mosca. Mas como é um desafio técnico e fui orientado a fazer o mais completo possível,
optei por fazer dessa forma.

# Segurança e robustez

- Sem over-posting: apenas DTOs saem do caso de uso (sem expor entidade).
- Timeouts em WHOIS/DNS; tokens separados para evitar cancelar operações do EF; logs úteis para troubleshooting.

# Escalabilidade e extensibilidade

- Hexagonal: você pode trocar o adapter (ex.: MySQL, Dynamo, Redis cache) sem mexer no domínio.
- Mais portas fáceis: REST minimal APIs, gRPC, Worker de refresh, fila de reprocessamento, etc.
- Observabilidade: logs estruturados, health checks já registrados.

Como rodar
Pré-requisitos

.NET 8 SDK

Docker e Docker Compose

# 1 - Subir Postgres e aplicar migrações

cd Database
make postgres      # sobe o postgres e roda as migrações
make up            # só sobe o postgres
make migrate       # roda/força as migrações
make status        # vê status
make down          # derruba containers
make clean         # derruba + remove volume

# 2 - Configurar variáveis de ambiente do banco

Na pasta Database/, crie .env (ou use o sample.env já pronto):

DB_USER=developer
\n
DB_PASS=umbler@!Ev4l
DB_PASS_URLENC=umbler%40%21Ev4l   # URL-encoded
DB_NAME=whoisDb

# 2 - Configurar variáveis de ambiente do banco

Na pasta Database/, crie .env (ou use o sample.env já pronto):

DB_USER=developer
DB_PASS=umbler@!Ev4l
DB_PASS_URLENC=umbler%40%21Ev4l   # URL-encoded
DB_NAME=whoisDb


# 3 - Subir Postgres + rodar migrações

- Na pasta Database/:
	- make postgres        # sobe o Postgres e executa as migrações
	# ou, se quiser separado:
	- make up              # sobe só o Postgres
	- make migrate         # roda/força as migrações

- Comandos úteis:
	- make status          # mostra status dos containers
	- make down            # derruba os containers
	- make clean           # derruba e remove volumes (reset do banco)

# 4 - Conferir a conexão da aplicação

No projeto Web (Blazor), verifique appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=whoisDb;Username=developer;Password=umbler@!Ev4l"
  }
}

# 5 - Rodar a aplicação

- Na raiz da solução (onde está o .sln ou o projeto web):
	- dotnet restore
	- dotnet build
	- dotnet run

- A aplicação abrirá em algo como https://localhost:5xxx (ou http://localhost:5xxx).

# 6 - Usar a tela

- Acesse /counter (ex.: https://localhost:5xxx/counter).
- Informe um domínio (ex.: ns254.umbler.com) e clique em Create.
- Se já existir e o TTL ainda estiver válido, a aplicação retorna do cache; senão, resolve DNS/WHOIS e salva.
- Validações e erros aparecem via UI (e também ficam nos logs).

# 7 - Rodar testes

- Na solução de testes:
	- dotnet test

# Fluxo de desenvolvimento

1. Altere código e rode os testes: 
	dotnet test
2. Suba Postgres com make postgres (só na 1ª vez ou quando mudar migrações).
3. Rode o WebApp com dotnet run e teste a UI.
4. Use make logs e os logs do .NET para debugar timeouts/DNS/WHOIS.

# Como contribuir

1) Convenções de código

- C# 12 / .NET 8
- CQRS via MediatR
- FluentValidation com ValidationBehavior no pipeline
- AutoMapper para conversão de Command ⇄ Entity ⇄ Response
- Arquitetura hexagonal (ports & adapters):
- Domain: entidades e invariantes
- Application: casos de uso, handlers, validators, profiles
- Infrastructure: EF Core, mappings, repositórios
- WebApp: UI (Blazor Server) como driver

2) Pull Requests

- Crie uma branch a partir de main:
	- git checkout -b feat/nome-da-feature
- Garanta que os testes passam (dotnet test) e que não quebrou o build.
- Descreva claramente o problema e a solução.
- Se mexer em banco, inclua a migration na pasta de migrações.
- Se expuser novos endpoints/rotas ou parâmetros, atualize o README.

# Dicas

- WHOIS/DNS lentos?
	- Ajuste timeouts no componente (WHOIS: CancellationTokenSource(5s), DNS: LookupClientOptions.Timeout=3s).

- Insert falha com DateTime/UTC?
	- Garanta timestamptz no Postgres ou não envie UtcNow para colunas timestamp (sem time zone) — o projeto já está preparado para timestamptz.

- Timeout no DB?
	- Confirme Host/Port e credenciais; teste com make psql.
	- Ajuste Timeout e CommandTimeout na connection string.
