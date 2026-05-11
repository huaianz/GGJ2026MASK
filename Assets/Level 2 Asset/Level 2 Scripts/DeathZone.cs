using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Level2ResetController reset = FindObjectOfType<Level2ResetController>();
        if (reset != null) reset.RestartLevel2WhenDeadOrInit();
    }
}
