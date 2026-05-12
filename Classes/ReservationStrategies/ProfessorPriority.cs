public class ProfessorPriority : IReservationStrategy
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
                    if (res.Author.IsProfessor)
                    {
                        throw new Exception("Data já preenchida por um professor para essa sala!");
                    }
                    server.DeleteReservation(res);
                    Reservation newReservation = new Reservation(Author, From, To, classroom.Number );
                    server.StoreReservation(newReservation);

                }
                
            }

        }   
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }
}