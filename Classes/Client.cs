using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Client
{
    private readonly ServerSingleton _server = ServerSingleton.GetInstance();
    private readonly ReservationReportService _reportService = new();
    private User _currentUser;
    private const string CLASSROOMS_PATH = "./classrooms.txt";

    public Client()
    {
        _server.Subscribe(_reportService);
    }

    public void Run()
    {
        while (true)
        {
            ShowMenu();
            var option = Console.ReadLine()?.Trim();
            Console.WriteLine();

            switch (option)
            {
                case "1": CreateUser(); break;
                case "2": Login(); break;
                case "3": ListRooms(); break;
                case "4": CreateRoom(); break;
                case "5": MakeReservation(); break;
                case "6": ListReservations(); break;
                case "7": ShowReports(); break;
                case "8": ShowNotifications(); break;
                case "0": return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
            Console.Clear();
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("=== Room Scheduler CLI ===");
        Console.WriteLine(_currentUser == null ? "Usuário atual: nenhum" : $"Usuário atual: {_currentUser.Name} ({(_currentUser.IsProfessor ? "Professor" : "Aluno")})");
        Console.WriteLine("1 - Criar usuário");
        Console.WriteLine("2 - Login");
        Console.WriteLine("3 - Listar salas");
        Console.WriteLine("4 - Criar sala");
        Console.WriteLine("5 - Fazer reserva");
        Console.WriteLine("6 - Listar reservas");
        Console.WriteLine("7 - Exibir relatórios de reservas");
        Console.WriteLine("8 - Exibir notificações do usuário logado");
        Console.WriteLine("0 - Sair");
        Console.Write("Escolha uma opção: ");
    }

    private void CreateUser()
    {
        Console.Write("Nome: ");
        var name = Console.ReadLine()?.Trim();
        Console.Write("Senha: ");
        var password = Console.ReadLine()?.Trim();
        Console.Write("É professor? (s/n): ");
        var isProfessor = Console.ReadLine()?.Trim().ToLower() == "s";

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Nome e senha são obrigatórios.");
            return;
        }

        var user = _server.CreateUser(name, password, isProfessor);
        if (user != null)
        {
            Console.WriteLine($"Usuário '{name}' criado com sucesso.");
        }
    }

    private void Login()
    {
        Console.Write("Nome: ");
        var name = Console.ReadLine()?.Trim();
        Console.Write("Senha: ");
        var password = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Nome e senha são obrigatórios.");
            return;
        }

        var user = _server.Login(name, password);
        if (user != null)
        {
            _currentUser = user;
            _server.Subscribe(_currentUser);
            Console.WriteLine($"Login realizado. Bem-vindo, {_currentUser.Name}.");
        }
    }

    private void ListRooms()
    {
        var rooms = LoadRooms();
        if (!rooms.Any())
        {
            Console.WriteLine("Nenhuma sala cadastrada.");
            return;
        }

        Console.WriteLine("Salas cadastradas:");
        foreach (var room in rooms)
        {
            Console.WriteLine($"- Sala {room.Number}: {room.GetType().Name}");
        }
    }

    private void CreateRoom()
    {
        Console.Write("Número da sala: ");
        if (!int.TryParse(Console.ReadLine(), out var number))
        {
            Console.WriteLine("Número inválido.");
            return;
        }

        Console.WriteLine("Tipo de sala:");
        Console.WriteLine("1 - IndividualStudyRoom");
        Console.WriteLine("2 - GroupWorkRoom");
        Console.WriteLine("3 - LabRoom");
        Console.Write("Escolha: ");
        var option = Console.ReadLine()?.Trim();

        var type = option switch
        {
            "1" => ClassroomType.IndividualStudyRoom,
            "2" => ClassroomType.GroupWorkRoom,
            "3" => ClassroomType.LabRoom,
            _ => (ClassroomType?)null
        };

        if (type == null)
        {
            Console.WriteLine("Tipo de sala inválido.");
            return;
        }

        _server.CreateRoom(number, type.Value);
        Console.WriteLine($"Sala {number} do tipo {type} criada.");
    }

    private void MakeReservation()
    {
        if (_currentUser == null)
        {
            Console.WriteLine("É necessário fazer login antes de reservar.");
            return;
        }

        var rooms = LoadRooms();
        if (!rooms.Any())
        {
            Console.WriteLine("Não há salas cadastradas para reservar.");
            return;
        }

        Console.Write("Número da sala: ");
        if (!int.TryParse(Console.ReadLine(), out var roomNumber))
        {
            Console.WriteLine("Número inválido.");
            return;
        }

        var classroom = rooms.FirstOrDefault(r => r.Number == roomNumber);
        if (classroom == null)
        {
            Console.WriteLine("Sala não encontrada.");
            return;
        }

        Console.Write("Início da reserva (ex: 2026-05-20 14:00): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var from))
        {
            Console.WriteLine("Data de início inválida.");
            return;
        }

        Console.Write("Fim da reserva (ex: 2026-05-20 16:00): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var to))
        {
            Console.WriteLine("Data de fim inválida.");
            return;
        }

        if (to <= from)
        {
            Console.WriteLine("A data de fim deve ser posterior à data de início.");
            return;
        }

        classroom.ReservationStrategy = _currentUser.IsProfessor
            ? new ProfessorPriority()
            : new FirstPriorityStrategie();

        classroom.PlaceReservation(classroom, _currentUser, from, to);
        Console.WriteLine("Tentativa de reserva realizada.");
    }

    private void ListReservations()
    {
        var reservations = _server.GetReservations();
        if (reservations == null || !reservations.Any())
        {
            Console.WriteLine("Nenhuma reserva encontrada.");
            return;
        }

        Console.WriteLine("Reservas atuais:");
        foreach (var reservation in reservations)
        {
            Console.WriteLine($"- Sala {reservation.RoomNumber}: {reservation.Author.Name} de {reservation.From:yyyy-MM-dd HH:mm} até {reservation.To:yyyy-MM-dd HH:mm}");
        }
    }

    private void ShowReports()
    {
        var lines = _reportService.ReportLines;
        if (!lines.Any())
        {
            Console.WriteLine("Nenhum relatório disponível ainda.");
            return;
        }

        Console.WriteLine("Relatórios de reservas:");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }

    private void ShowNotifications()
    {
        if (_currentUser == null)
        {
            Console.WriteLine("Faça login para ver suas notificações.");
            return;
        }

        var notifications = _currentUser.Notifications;
        if (!notifications.Any())
        {
            Console.WriteLine("Nenhuma notificação disponível.");
            return;
        }

        Console.WriteLine("Notificações:");
        foreach (var notification in notifications)
        {
            Console.WriteLine(notification);
        }
    }

    private static List<IClassroom> LoadRooms()
    {
        if (!File.Exists(CLASSROOMS_PATH))
        {
            return new List<IClassroom>();
        }

        var lines = File.ReadAllLines(CLASSROOMS_PATH)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        return lines.Select(line =>
        {
            var pieces = line.Split('|');
            if (pieces.Length != 2) return null;

            if (!int.TryParse(pieces[0], out var number)) return null;
            return pieces[1] switch
            {
                "IndividualStudyRoom" => ClassroomsFactory.CreateRoom(ClassroomType.IndividualStudyRoom, number),
                "GroupWorkRoom" => ClassroomsFactory.CreateRoom(ClassroomType.GroupWorkRoom, number),
                "LabRoom" => ClassroomsFactory.CreateRoom(ClassroomType.LabRoom, number),
                _ => null,
            };
        })
        .Where(room => room != null)
        .ToList()!;
    }
}
