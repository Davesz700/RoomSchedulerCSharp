public class GroupWorkRoomClassroom : IClassroom
{
    private int _number;
    private IReservationStrategy _reservationStrategy;
    public int Number
    {
        get
        {
            return _number;
        }
    }

    public GroupWorkRoomClassroom(int number)
    {
        this._number = number;
        this.ReservationStrategy = new FirstPriorityStrategie();
    }
    
    
    public IReservationStrategy ReservationStrategy
    {
        get
        {
            return _reservationStrategy;
        }
        set
        {
            if(value != null) _reservationStrategy = value;
        }
    }

    public void PlaceReservation(IClassroom classroom, User Author, DateTime From, DateTime To)
    {
        this._reservationStrategy.PlaceReservation(classroom, Author, From, To);
    }

}