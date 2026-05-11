using System.IO;
using System.Collections.Generic;
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
            string[] users = File.ReadAllLines(USERS_PATH);
            
        }
        catch
        {
            
        }
        return null;
    }


    

}