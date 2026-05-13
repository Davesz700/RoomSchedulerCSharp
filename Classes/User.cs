using System.Collections.Generic;

public class User : IReservationObserver
{
    private string _name;
    private bool _isProfessor;
    private readonly List<string> _notifications = new();

    public string Name {
        get
        {
            return this._name;
        }
    }

    public bool IsProfessor
    {
        get
        {
            return this._isProfessor;
        }
    }

    public IReadOnlyList<string> Notifications => _notifications.AsReadOnly();

    public User(string Name, bool IsProfessor)
    {
        this._name = Name;
        this._isProfessor = IsProfessor;
    }

    public void ReservationChanged(IReservationSubject subject, Reservation reservation)
    {
        _notifications.Add($"Notificação: reserva em sala {reservation.RoomNumber} criada por {reservation.Author.Name} de {reservation.From:o} até {reservation.To:o}.");
    }// método que é chamado quando uma reserva é criada ou alterada.
}