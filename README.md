# VirtualCard
Generate Virtual Card Number API


## Introdução
Esta API foi feita para cadastrar cliente e gerar um número de cartao virtual. Usaremos duas operações do REST API: GET; POST.

A versão desta API será v1.

Abaixo estão as rotas da API

VERBO       URL                 PARAMETROS ou OBJETO      DESCRIÇÃO
HttpGet   - v1/clients       -                          - Retorna todos os clientes 
HttpGet   - v1/cards         -                          - Retorna todos os cartões
HttpGet   - v1/cards/clients -  email                   - Retorna os cartões de um determinado cliente 
HttpPost  - v1/clients       -  Client                  - Cadastra cliente
HttpPost   - v1/cards        -  email                   - Cadastra cartão de um cliente


## Pré requisitos
  - Visual Studio 2019
  - WEB API ASPNET Core 3.1
  - EntityFramework Core In Memory 5.0
  - CreditCardValidator 2.0. 

Skills
  - C#
  - ORM(Object Relational Mapping)
  - Restful services


## Passo a passo

# Passo 01 - Criar projeto

Abra o Visual Studio e siga os passos.
Selecionar CREATE A NEW PROJECT.
Na caixa de pesquisa digite ASPNET CORE WEB APPLICATION
Selecionar na lista abaixo o template ASPNET CORE WEB APPLICATION
Clicar em NEXT
Digite o nome do seu projeto e escolha o local.
Clicar em CREATE.


Passo a passo
criacao dos models;
criacao do dbcontext;
instalacao do entityframworkcore.InMemory pelo Nuget Package Manager
instalaccao do CreditCardValidator pelo Nuget Package Manager
configuracao do dbcontext;
criacao dos controllers;

## Checklist
  
- Setup inicial do projeto
  - 
  - Dependências
  - Arquivos .properties
  - Configuração de segurança
- Modelo de domínio
  - Entidades e relacionamentos
  - Mapeamento objeto-relacional
  - Seed
- Criar endpoints
  - [GET] /products
  - [GET] /orders
  - [POST] /orders
  - [PUT] /orders/{id}/delivered
- Validar perfil dev
  - Base de dados Postgres local
  - Testar todos endpoints
- Preparar projeto para implantação
  - Arquivo system.properties
  - Profile prod -> commit
- Implantar projeto no Heroku
  - Criar app e provisionar Postgres
  - Criar base de dados remota
  - Executar comandos no Heroku CLI

**ATENÇÃO: O PROJETO NÃO RODA LOCALMENTE NO PROFILE PROD! Se você quiser rodar o projeto localmente depois, mude para o profile test.**
