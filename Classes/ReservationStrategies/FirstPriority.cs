using System.Linq;

public class FirstPriorityStrategie : IReservationStrategy
{
    public void PlaceReservation(IClassroom classroom, User Author, DateTime From, DateTime To)
    {
        try{
            ServerSingleton server = ServerSingleton.GetInstance();
            List<Reservation> reservations = server.GetReservations().FindAll(x => x.RoomNumber == classroom.Number);
            foreach (Reservation res in reservations)
            {
                if((From >= res.From && From <= res.To)||(To >= res.From && To<= res.To))
                {
                    throw new Exception("Data já preenchida para essa sala!");
                }
                
            }
            Reservation newReservation = new Reservation(Author, From, To, classroom.Number );
            server.StoreReservation(newReservation);
        }   
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }
}