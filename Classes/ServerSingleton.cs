using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public class ServerSingleton
{
    private static ServerSingleton _instance;
    private const string RESERVATIONS_PATH = "./reservations.txt";
    private const string CLASSROOMS_PATH = "./classrooms.txt";
    private const string USERS_PATH = "./users.txt";
    private const string NOTIFICATIONS_PATH = "./Notifications.txt";
    private  List<IClassroom> _cachedRooms;
    private ServerSingleton()
    {
        _cachedRooms = new();

    }
    public static ServerSingleton GetInstance()
    {
        if(_instance == null)return new ServerSingleton();
        return _instance;
    }

    public User CreateUser(string Name, string Password, bool IsProfessor)
    {
        try
        {
            List<string> users = File.ReadAllLines(USERS_PATH).ToList();
            var usernames = users;
            usernames.ForEach(x => x.Split("|"));
            
            if (usernames.Contains(Name))
            {
                throw new Exception("Um usuário com esse nome já existe!");
            }
            File.AppendAllText(USERS_PATH, $"\n{Name}|{IsProfessor}|{Password}");
            return new User(Name, IsProfessor);
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
            var user = File.ReadAllLines(USERS_PATH).ToList().FirstOrDefault( l => l.StartsWith(Name));
            
            if (user == null)
            {
                throw new Exception("Usuário não encontrado!");
            }
            var password = user.Split("|")[2];
            
            if(Password != password)
            {
                throw new Exception("Senha incorreta!");
            }
            
            if(user.Split("|")[1] == "true")return new User(Name, true);
            else return new User(Name, false);        
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
                {                case "InfoLab":
                        _cachedRooms.Add(ClassroomsFactory
                            .CreateRoom(ClassroomType.InfoLab,int.Parse(separated[0])));
                        break;
                    case "SeminaryRoom":
                        _cachedRooms.Add(ClassroomsFactory
                            .CreateRoom(ClassroomType.SeminaryRoom,int.Parse(separated[0])));
                        break;
                    case "ChemistryLab":
                        _cachedRooms.Add(ClassroomsFactory
                            .CreateRoom(ClassroomType.ChemistryLab,int.Parse(separated[0])));
                        break;
                }

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


    

}