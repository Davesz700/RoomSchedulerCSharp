public class IndividualStudyRoomClassroom : IClassroom
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

    public IndividualStudyRoomClassroom(int number){
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

    public void PlaceReservation()
    {
        
    }

}