# Desafio backend
 
## Requisitos não funcionais 
- [x] A aplicação deverá ser construida com .Net utilizando C#.
- [x] Utilizar apenas os seguintes bancos de dados (Postgress, MongoDB)
    - [x] Não utilizar PL/pgSQL
- [x] Escolha o sistema de mensageria de sua preferencia( RabbitMq, Sqs/Sns , Kafka, Gooogle Pub/Sub ou qualquer outro)

## Aplicação a ser desenvolvida
Seu objetivo é criar uma aplicação para gerenciar aluguel de motos e entregadores. Quando um entregador estiver registrado e com uma locação ativa poderá também efetuar entregas de pedidos disponíveis na plataforma.

### Casos de uso
- [x] Eu como usuário admin quero cadastrar uma nova moto.
  - [x] Os dados obrigatórios da moto são Identificador, Ano, Modelo e Placa
  - [x] A placa é um dado único e não pode se repetir.
  - [x] Quando a moto for cadastrada a aplicação deverá gerar um evento de moto cadastrada
    - [x] A notificação deverá ser publicada por mensageria.
    - [x] Criar um consumidor para notificar quando o ano da moto for "2024"
    - [x] Assim que a mensagem for recebida, deverá ser armazenada no banco de dados para consulta futura.
- [x] Eu como usuário admin quero consultar as motos existentes na plataforma e conseguir filtrar pela placa.
- [x] Eu como usuário admin quero modificar uma moto alterando apenas sua placa que foi cadastrado indevidamente
- [x] Eu como usuário admin quero remover uma moto que foi cadastrado incorretamente, desde que não tenha registro de locações.
- [x] Eu como usuário entregador quero me cadastrar na plataforma para alugar motos.
    - [x] Os dados do entregador são (identificador, nome, cnpj, data de nascimento, número da CNHh, tipo da CNH, imagemCNH)
    - [x] Os tipos de cnh válidos são A, B ou ambas A+B.
    - [x] O cnpj é único e não pode se repetir.
    - [x] O número da CNH é único e não pode se repetir.
- [x] Eu como entregador quero enviar a foto de minha cnh para atualizar meu cadastro.
    - [x] O formato do arquivo deve ser png ou bmp.
    - [x] A foto não poderá ser armazenada no banco de dados, você pode utilizar um serviço de storage( disco local, amazon s3, minIO ou outros).
- [x] Eu como entregador quero alugar uma moto por um período.
    - [x] Os planos disponíveis para locação são:
        - [x] 7 dias com um custo de R$30,00 por dia
        - [x] 15 dias com um custo de R$28,00 por dia
        - [x] 30 dias com um custo de R$22,00 por dia
        - [x] 45 dias com um custo de R$20,00 por dia
        - [x] 50 dias com um custo de R$18,00 por dia
    - [x] A locação obrigatóriamente tem que ter uma data de inicio e uma data de término e outra data de previsão de término.
    - [x] O inicio da locação obrigatóriamente é o primeiro dia após a data de criação.
    - [x] Somente entregadores habilitados na categoria A podem efetuar uma locação
- [x] Eu como entregador quero informar a data que irei devolver a moto e consultar o valor total da locação.
    - [x] Quando a data informada for inferior a data prevista do término, será cobrado o valor das diárias e uma multa adicional
        - [x] Para plano de 7 dias o valor da multa é de 20% sobre o valor das diárias não efetivadas.
        - [x]Para plano de 15 dias o valor da multa é de 40% sobre o valor das diárias não efetivadas.
    - [x] Quando a data informada for superior a data prevista do término, será cobrado um valor adicional de R$50,00 por diária adicional.
    

## Diferenciais 🚀
- [x] Testes unitários
- [ ] Testes de integração
- [x] EntityFramework e/ou Dapper
- [ ] Docker e Docker Compose
- [x] Design Patterns
- [ ] Documentação
- [x] Tratamento de erros
- [x] Arquitetura e modelagem de dados
- [x] Código escrito em língua inglesa
- [ ] Código limpo e organizado
- [ ] Logs bem estruturados
- [x] Seguir convenções utilizadas pela comunidade
  

