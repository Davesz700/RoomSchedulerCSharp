using System.Collections.Generic;

public interface IReservationSubject
{
    IReadOnlyList<Reservation> GetReservations();
}
