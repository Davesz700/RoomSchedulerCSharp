public enum ClassroomType
{
    Laboratory,
    StudyRoom,
    WorkgroupRoom,
}//Tipagem de sala de aula, para diferenciar os tipos de salas disponíveis



 public interface IClassroom
{
    int number_classroom { get; }
     int Capacity { get; }
     bool IsAvailable { get; }
     int id { get; }
    DateTime? SchedulingDate { get; set; }//a data de agendamento pode ser nula ou não, dependendo se a sala já tiver sido agendada ou não   

    //List<Classroom> GetAvailableClassrooms(DateTime date);
}//interface para definir as propriedades e métodos que as salas de aula devem implementar

public class Classroom : IClassroom
{
    public ClassroomType Type { get; private set; }
    public int number_classroom { get; private set; }
    public int Capacity { get; private set; }
    public bool IsAvailable { get; private set; }
    public int id { get; private set; }
    public DateTime? SchedulingDate { get; set; }

    public Classroom(ClassroomType type, int number_classroom, int capacity, bool isAvailable, int id)
    {
        Type = type;
        this.number_classroom = number_classroom;
        Capacity = capacity;
        IsAvailable = isAvailable;
        this.id = id;
        SchedulingDate = null;
    }
}//Utilizamos uma classe base para representar salas de aula

public class Laboratory : Classroom
{
    public Laboratory(int number_classroom, int capacity, bool isAvailable, int id)
        : base(ClassroomType.Laboratory, number_classroom, capacity, isAvailable, id)
    {
    }
}

public class StudyRoom : Classroom
{
    public StudyRoom(int number_classroom, int capacity, bool isAvailable, int id)
        : base(ClassroomType.StudyRoom, number_classroom, capacity, isAvailable, id)
    {
    }
}

public class WorkgroupRoom : Classroom
{
    public WorkgroupRoom(int number_classroom, int capacity, bool isAvailable, int id)
        : base(ClassroomType.WorkgroupRoom, number_classroom, capacity, isAvailable, id)
    {
    }
}



public class ClassroomFactory//Fábrica para criar instâncias de salas de aula com base no tipo especificado
{
    public static IClassroom CreateClassroom(ClassroomType type, int number_classroom, int capacity, bool isAvailable, int id)
    {
        switch (type)
        {
            case ClassroomType.Laboratory:
                return new Laboratory(number_classroom, capacity, isAvailable, id);
            case ClassroomType.StudyRoom:
                return new StudyRoom(number_classroom, capacity, isAvailable, id);
            case ClassroomType.WorkgroupRoom:
                return new WorkgroupRoom(number_classroom, capacity, isAvailable, id);
            default:
                throw new ArgumentException("Invalid classroom type");
        }
    }
}
