
public static class ClassroomsFactory
{
    public static IClassroom CreateRoom(ClassroomType Type, int number)
    {
        switch (Type)
        {
            case ClassroomType.InfoLab:
                return new InfoLabClassroom(number);            
            case ClassroomType.SeminaryRoom:
                return new SeminaryClassroom(number);
            case ClassroomType.ChemistryLab:
                return new ChemistryLabClassroom(number);
            default:
                return null;
            
        }        
    }
}