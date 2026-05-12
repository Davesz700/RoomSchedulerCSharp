using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public class ServerSingleton : IReservationSubject
{
    private static ServerSingleton _instance;
    private const string RESERVATIONS_PATH = "./reservations.txt";
    private const string CLASSROOMS_PATH = "./classrooms.txt";
    private const string USERS_PATH = "./users.txt";
    private const string NOTIFICATIONS_PATH = "./Notifications.txt";
    private List<IClassroom> _cachedRooms;
    private List<User> _cachedUsers;
    private List<Reservation> _cachedReservations;
    private readonly List<IReservationObserver> _subscribers = new();

    private ServerSingleton()
    {
        _cachedRooms = new();
        _cachedUsers = new();
        _cachedReservations = new();
    }
    public static ServerSingleton GetInstance()
    {
        if(_instance == null)
        {
            _instance = new ServerSingleton();
        }
        return _instance;
    }

    public void Subscribe(IReservationObserver observer)
    {
        if (observer != null && !_subscribers.Contains(observer))
        {
            _subscribers.Add(observer);
        }
    }

    public void Unsubscribe(IReservationObserver observer)
    {
        if (observer != null)
        {
            _subscribers.Remove(observer);
        }
    }

    public List<Reservation> GetReservations()
    {
        if (_cachedReservations.Count == 0)
        {
            CacheReservations();
        }

        return _cachedReservations;
    }

    private void NotifyReservationChanged(Reservation reservation)
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.ReservationChanged(this, reservation);
        }
    }

    public User CreateUser(string Name, string Password, bool IsProfessor)
    {
        try
        {
            if(_cachedUsers.Count == 0)
            {
                CacheUsers();
            }

            if (_cachedUsers.Any(u => u.Name == Name))
            {
                throw new Exception("Um usuário com esse nome já existe!");
            }

            File.AppendAllText(USERS_PATH, $"\n{Name}|{IsProfessor}|{Password}");
            var createdUser = new User(Name, IsProfessor);
            _cachedUsers.Add(createdUser);
            return createdUser;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }
    public User Login(string Name, string Password)
    {
        try
        {
            if(_cachedUsers.Count == 0)
            {
                CacheUsers();
            }

            var cachedUser = _cachedUsers.FirstOrDefault(u => u.Name == Name);

            if (cachedUser == null)
            {
                throw new Exception("Usuário não encontrado!");
            }

            var userLine = File.ReadAllLines(USERS_PATH).ToList().FirstOrDefault(l => l.StartsWith(Name + "|"));
            if (userLine == null)
            {
                throw new Exception("Usuário não encontrado!");
            }

            var password = userLine.Split("|")[2];

            if(Password != password)
            {
                throw new Exception("Senha incorreta!");
            }

            return new User(Name, cachedUser.IsProfessor);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }


    void CacheRooms()
    {
        var classrooms = File.ReadAllLines(CLASSROOMS_PATH).ToList();
        classrooms.ForEach(x =>
        {
           var separated = x.Split("|");

           if(!_cachedRooms.Any(x => x.Number == int.Parse(separated[0])))
            {

                switch (separated[1])
                {
                    case "IndividualStudyRoom":
                        _cachedRooms.Add(ClassroomsFactory
                            .CreateRoom(ClassroomType.IndividualStudyRoom,int.Parse(separated[0])));
                        break;
                    case "GroupWorkRoom":
                        _cachedRooms.Add(ClassroomsFactory
                            .CreateRoom(ClassroomType.GroupWorkRoom,int.Parse(separated[0])));
                        break;
                    case "LabRoom":
                        _cachedRooms.Add(ClassroomsFactory
                            .CreateRoom(ClassroomType.LabRoom,int.Parse(separated[0])));
                        break;
                }

            }

        });
    }

    void CacheUsers()
    {
        var users = File.ReadAllLines(USERS_PATH).ToList();
        users.ForEach(x =>
        {
            var separated = x.Split("|");

            if(!_cachedUsers.Any(u => u.Name == separated[0]))
            {
                _cachedUsers.Add(new User(separated[0], separated[1] == "true"));
            }
        });
    }

    void CacheReservations()
    {
        if(_cachedUsers.Count == 0)
        {
            CacheUsers();
        }

        var reservations = File.ReadAllLines(RESERVATIONS_PATH).ToList();
        reservations.ForEach(x =>
        {
            var separated = x.Split("|");
            var authorName = separated[0];
            var roomNumber = int.Parse(separated[1]);
            var from = DateTime.Parse(separated[2]);
            var to = DateTime.Parse(separated[3]);
            

            if(!_cachedReservations.Any(r => r.Author.Name == authorName && r.From == from && r.To == to && r.RoomNumber == roomNumber))
            {
                var author = _cachedUsers.FirstOrDefault(u => u.Name == authorName) ?? new User(authorName, false);
                var reservation = new Reservation(author, from, to, roomNumber);
                reservation.RoomNumber = roomNumber;
                _cachedReservations.Add(reservation);
            }
        });
    }

    public void CreateRoom(int Number, ClassroomType type)
    {
        try
        {
            if(_cachedRooms.Count == 0)
            {
                CacheRooms();
            }           

            if(_cachedRooms.Any(x => x.Number == Number))
            {
                throw new Exception("Já existe uma sala com esse número!");
            }
            
            File.AppendAllText(CLASSROOMS_PATH, $"\n{Number}|{type}");
            CacheRooms();

        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
        return;
    }

    public void StoreReservation(Reservation reservation)
    {
        var storeData = $"\n{reservation.Author.Name}|{reservation.RoomNumber}|{reservation.From}|{reservation.To}";

        File.AppendAllText(RESERVATIONS_PATH, storeData);
        CacheReservations();
    }
    public void DeleteReservation(Reservation reservation)
    {
        var lineToDelete = $"\n{reservation.Author.Name}|{reservation.RoomNumber}|{reservation.From}|{reservation.To}";
        var newLines = File.ReadLines(RESERVATIONS_PATH).Where(l => l!= lineToDelete).ToList();
        File.WriteAllLines(RESERVATIONS_PATH, newLines);
        CacheReservations();
    }
}