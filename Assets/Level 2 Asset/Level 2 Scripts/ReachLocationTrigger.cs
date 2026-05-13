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

        // 【移除】ShowPhoneDialogue内部已经处理了玩家暂停，这里不需要重复调用
        // Player player = other.GetComponent<Player>();
        // if (player != null)
        // {
        //     player.PauseMovement();
        // }
    }


    public void ResetSingleTrigger()
    {
        hasTriggered = false;
        gameObject.SetActive(true);
    }
}
