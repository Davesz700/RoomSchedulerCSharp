public class Reservation
{
    private User _author;
    private DateTime _from;
    private DateTime _to;
    private int _roomNumber;



    public Reservation(User Author, DateTime From, DateTime To, int RoomNumber)
    {
        this._author = Author;
        this._from = From;
        this._to = To;
        this._roomNumber = RoomNumber;
    }


    public int RoomNumber
    {
        get
        {
            return this._roomNumber;
        }   
        set
        {
            this._roomNumber = value;
        }
    }
    
    public DateTime From
    {
        get
        {
            return this._from;
        }
        set
        {
            this._from = value;
        }
    }

    public DateTime To
    {
        get
        {
            return this._to;
        }
        set
        {
            this._to = value;
        }
    }
    
    
    
    public User Author
    {
        get
        {
            return this._author;
        }
    }
    
}