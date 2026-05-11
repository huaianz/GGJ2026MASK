using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryStudent : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventHandler.CallUpdateMemoryUI(2);
        }
    }
}
