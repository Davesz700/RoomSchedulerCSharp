public interface IReservationStrategy
{
    void PlaceReservation(IClassroom classroom, User Author, DateTime From, DateTime To);
    
}