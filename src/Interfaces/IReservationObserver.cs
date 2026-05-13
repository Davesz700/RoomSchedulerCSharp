public interface IReservationObserver
{
    void ReservationChanged(IReservationSubject subject, Reservation reservation);
}//criação da interface do observer
