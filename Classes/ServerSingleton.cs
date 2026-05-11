using System.IO;
using System.Collections.Generic;
using System.Linq;

public class ServerSingleton
{
    private static ServerSingleton _instance;
    private const string RESERVATIONS_PATH = "./reservations.txt";
    private const string CLASSROOMS_PATH = "./classrooms.txt";
    private const string USERS_PATH = "./users.txt";

    private ServerSingleton()
    {
        
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
            File.AppendAllText(USERS_PATH, $"{Name}|{IsProfessor}|{Password}");
            return new User(Name, IsProfessor);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            
        }
        return null;
    }
    public void Login()
    {
        
    }


    

}