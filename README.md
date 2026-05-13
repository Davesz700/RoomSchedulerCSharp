# RoomSchedulerCS

RoomSchedulerCS é um agendador de salas simples em C# que roda como um aplicativo de console.

## O que ele faz

- Gerencia usuários com login e cadastro.
- Permite criar salas de diferentes tipos.
- Suporta reservas de salas com verificação de conflito de horários.
- Usa estratégias de reserva para definir regras de prioridade.
- Gera relatórios simples de reservas e envia notificações a usuários.

## Estrutura principal

- `Program.cs`: ponto de entrada do aplicativo que inicia o cliente CLI.
- `Classes/Client.cs`: menu interativo em linha de comando para usar as funcionalidades.
- `Classes/ServerSingleton.cs`: servidor único responsável por armazenar e recuperar dados de usuários, salas e reservas.
- `Classes/ReservationReportService.cs`: serviço que observa mudanças em reservas e registra relatórios.
- `Classes/ClassroomsFactory.cs`: fábrica para criar instâncias de salas.
- `Classes/ReservationStrategies/`: estratégias de reserva aplicadas a diferentes regras de prioridade.

## Arquivos de dados

O projeto guarda dados simples em arquivos de texto:

- `classrooms.txt` — salas cadastradas
- `users.txt` — usuários cadastrados
- `reservations.txt` — reservas de salas

> Esses arquivos são usados diretamente pelo `ServerSingleton`.

## Como executar

1. Abra o terminal na pasta do projeto.
2. Execute `dotnet build` para compilar.
3. Execute `dotnet run` para iniciar o aplicativo.
4. Use o menu exibido para criar usuário, fazer login, criar salas, reservar e visualizar relatórios.

## Funcionamento do CLI

O menu permite:

1. Criar usuário
2. Fazer login
3. Listar salas existentes
4. Criar nova sala
5. Fazer reserva de uma sala
6. Listar reservas cadastradas
7. Exibir relatórios de reservas
8. Exibir notificações do usuário logado
0. Sair

## Regras de reserva

- `FirstPriorityStrategie`: bloqueia reservas em conflito de horário.
- `ProfessorPriority`: permite que professor substitua reservas de alunos, mas não permite sobrescrever reservas de outros professores.

## Observadores

- Usuários e o serviço de relatório implementam `IReservationObserver`.
- Quando uma reserva é armazenada, o servidor notifica os observadores.

## Nota

O projeto é ideal para testes locais e demonstração de um sistema de agendamento com arquitetura simples e persistência em arquivos.
