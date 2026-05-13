
public interface IClassroom
{
    int Number {get ;}    
    IReservationStrategy ReservationStrategy {get; set;}

    void PlaceReservation();
}