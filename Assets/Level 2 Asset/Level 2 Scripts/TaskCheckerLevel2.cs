using UnityEngine;

public class TaskCheckerLevel2 : MonoBehaviour
{
    public PlayerTasksLevel2 level2Tasks = new PlayerTasksLevel2();

    public void ResetTaskFlags()
    {
        level2Tasks.hasReachedSubwayEntrance = false;
        level2Tasks.hasReachedSubwayPlatformLeft = false;

        level2Tasks.hasReachedTicketGate = false;
        level2Tasks.hasReachedStairTop = false;
        level2Tasks.hasReachedStairMiddle = false;
        level2Tasks.hasReachedStairBottom = false;
        level2Tasks.hasReachedSubwayWithDoors = false;
    }

    public bool IsLevel2Complete()
    {
        return level2Tasks.hasReachedSubwayEntrance
            && level2Tasks.hasReachedSubwayPlatformLeft
            && level2Tasks.hasReachedTicketGate
            && level2Tasks.hasReachedStairTop
            && level2Tasks.hasReachedStairMiddle
            && level2Tasks.hasReachedStairBottom
            && level2Tasks.hasReachedSubwayWithDoors;
    }

    public void OnReach(ReachLocationType type)
    {
        switch (type)
        {
            case ReachLocationType.SubwayEntrance:       level2Tasks.hasReachedSubwayEntrance = true; break;
            case ReachLocationType.SubwayPlatformLeft:   level2Tasks.hasReachedSubwayPlatformLeft = true; break;
            case ReachLocationType.TicketGate:           level2Tasks.hasReachedTicketGate = true; break;
            case ReachLocationType.StairTop:             level2Tasks.hasReachedStairTop = true; break;
            case ReachLocationType.StairMiddle:          level2Tasks.hasReachedStairMiddle = true; break;
            case ReachLocationType.StairBottom:          level2Tasks.hasReachedStairBottom = true; break;
            case ReachLocationType.SubwayWithDoors:      level2Tasks.hasReachedSubwayWithDoors = true; break;
        }
    }
}
