using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryChild : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventHandler.CallUpdateMemoryUI(1);
        }
    }
}
