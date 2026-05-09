

public class reserve
{
    private int Id { get; set; }
    private int ClassroomId { get; set; }
    private int UserId { get; set; }
    public DateTime Date { get; set; }
    private TimeSpan StartTime { get; set; }
    private TimeSpan EndTime { get; set; }

        public reserve(int id, int classroomId, int userId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            Id = id;
            ClassroomId = classroomId;
            UserId = userId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
        }


}