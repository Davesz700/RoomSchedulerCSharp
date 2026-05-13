
public static class ClassroomsFactory
{
    public static IClassroom CreateRoom(ClassroomType Type, int number)
    {
        switch (Type)
        {
            case ClassroomType.IndividualStudyRoom:
                return new IndividualStudyRoomClassroom(number);            
            case ClassroomType.GroupWorkRoom:
                return new GroupWorkRoomClassroom(number);
            case ClassroomType.LabRoom:
                return new LabRoomClassroom(number);
            default:
                return null;
            
        }        
    }
}