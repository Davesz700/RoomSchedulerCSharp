public class User
{
    private string _name;
    private bool _isProfessor;

    public string Name {
        get
        {
            return this._name;
        }
    }//a
    public bool IsProfessor
    {
        get
        {
            return this._isProfessor;
        }
    }

    public User(string Name, bool IsProfessor)
    {
        this._name = Name;
        this._isProfessor = IsProfessor;
    }
}