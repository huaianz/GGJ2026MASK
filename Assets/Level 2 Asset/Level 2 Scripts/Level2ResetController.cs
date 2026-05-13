using UnityEngine;

public class Level2ResetController : MonoBehaviour
{
    [Header("Spawn Point")]
    public Transform spawnPoint;

    [Header("Auto-find triggers in Level2")]
    public ReachLocationTrigger[] triggers;



    public void RestartLevel2WhenDeadOrInit()
    {
        TaskCheckerLevel2 taskChecker = FindObjectOfType<TaskCheckerLevel2>();
        if (taskChecker != null)
        {
            taskChecker.ResetTaskFlags();
        }

        ResetAllTriggers();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        resetPlayerPositionAndVelocity(player);
        ForceFacingRight(player);
    }

    private void resetPlayerPositionAndVelocity(GameObject player)
    {
        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;
        }
    }

    public void ResetAllTriggers()
    {
        // Reset all location triggers
        if (triggers == null || triggers.Length == 0)
            triggers = FindObjectsOfType<ReachLocationTrigger>(true);

        foreach (var t in triggers)
            t.ResetSingleTrigger();
    }

    private void resetPlayerPositionAndVelocity()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;
        }
    }

    private void ForceFacingRight(GameObject player)
    {
        Player playerController = player.GetComponent<Player>();
        if (playerController != null)
        {
            playerController.IsFacingRight = true;
        }

        Vector3 scale = player.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        player.transform.localScale = scale;
    }

}
