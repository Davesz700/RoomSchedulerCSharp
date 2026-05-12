
public interface IClassroom
{
    int Number {get ;}    
    IReservationStrategy ReservationStrategy {get; set;}

    void PlaceReservation(IClassroom classroom, User Author, DateTime From, DateTime To);
}