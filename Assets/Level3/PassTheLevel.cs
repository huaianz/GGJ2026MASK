using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTheLevel : MonoBehaviour
{
    public bool isPass=false;
    public bool canPass;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && isPass)
        {
            UIManager.Instance.Passthelevel();
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPass = true;
            UIManager.Instance.playerDontMovent();
            EventHandler.CallUpdateTipsUI(18);
        }
    }
}
