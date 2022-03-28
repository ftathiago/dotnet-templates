# Authentication and authorization

## Status

Accepted

## Context

### O que nós precisamos?

- Que usuários possam ser cadastrados e autenticados dentro do domínio;
- Que as aplicações do domínio possam ter um fluxo de login unificado;
- Que o Login e o primeiro acesso sigam um fluxo seguro;
- Que usuários autenticados possam acessar os recursos do domínio;
- Que o token possa ser utilizado e validado pelos micro serviços que necessitem de autenticação/autenticação;

### O que vem por aí?

- Está prevista a criação de um sistema centralizado de controle de permissões;
- O APIM é quem deverá autenticar e validar tokens

A solução para autenticação de usuários no domínio não pode ser algo que custe muito, dado que, poderá substituída logo.

## Decision

- Um serviço dedicado de autenticação/autorização gera o token
- A chave privada deverá ser compartilhada entre os serviços, afim de que todos possam validar o token recebido;
  - Obviamente, estará armazenada em alguma secret;

## Consequences

- A aplicação WebApi não terá endpoints de login/logout/cadastro de usuários;
- Uma possível fragilidade, dado que a chave será compartilhada entre sistemas;
- Transferir a autenticação do token para a APIM quando for compilada;
- Descartar o sistema de cadastro quando chegar o momento;
  