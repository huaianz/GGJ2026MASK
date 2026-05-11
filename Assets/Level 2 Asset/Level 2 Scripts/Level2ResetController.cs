using UnityEngine;

public class Level2ResetController : MonoBehaviour
{
    [Header("Spawn Point")]
    public Transform spawnPoint;

    [Header("Auto-find triggers in Level2")]
    public ReachLocationTrigger[] triggers;

    

    public void RestartLevel2WhenDeadOrInit()
    {
        // Reset Task Flags
        TaskCheckerLevel2 taskChecker = FindObjectOfType<TaskCheckerLevel2>();
        if (taskChecker != null)
        {
            taskChecker.ResetTaskFlags();
        }

        // Reset All Location Triggers
        ResetAllTriggers();

        // Reset Player Position and velocity
        resetPlayerPositionAndVelocity();

        // Force Player to face right
        ForceFacingRight(GameObject.FindGameObjectWithTag("Player"));
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
       var sprite = player.GetComponent<SpriteRenderer>();
       if (sprite != null)
        sprite.flipX = false;
    }
    
}
