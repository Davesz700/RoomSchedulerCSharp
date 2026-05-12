using System.Collections.Generic;

public class ReservationReportService : IReservationObserver
{// implementação do observer para gerar relatórios de reservas
    private readonly List<string> _reportLines = new();

    public IReadOnlyList<string> ReportLines => _reportLines.AsReadOnly();

    public void ReservationChanged(IReservationSubject subject, Reservation reservation)
    {
        var totalReservations = subject.GetReservations().Count;
        _reportLines.Add($"Relatório: nova reserva na sala {reservation.RoomNumber} por {reservation.Author.Name}. Total de reservas: {totalReservations}.");
    }// método que é chamado quando uma reserva é criada ou alterada.
}
