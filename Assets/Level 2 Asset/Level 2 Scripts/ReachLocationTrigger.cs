using UnityEngine;

public enum ReachLocationType
{
    SubwayEntrance,
    SubwayPlatformLeft,
    TicketGate,
    StairTop,
    StairMiddle,
    StairBottom,
    SubwayWithDoors
}

public class ReachLocationTrigger : MonoBehaviour
{
    public ReachLocationType type;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (hasTriggered) return;
        // If the colliding object is not the player, exit the method
        if (!other.CompareTag("Player")) return;
        hasTriggered = true;

        TaskCheckerLevel2 checker = FindObjectOfType<TaskCheckerLevel2>();
        if (checker != null)
        {
            checker.OnReach(type);
        }
        if (UIManager.Instance != null)
        {
             UIManager.Instance.ShowPhoneDialogue(type);
        }
    }


    public void ResetSingleTrigger()
    {
        hasTriggered = false;
        gameObject.SetActive(true);
    }
}
