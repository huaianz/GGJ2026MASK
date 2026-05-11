using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomWindow : MonoBehaviour
{
    [Header("卧室窗户设置")]
    [SerializeField] private AudioSource WindowSound;
    public GameObject OpenWindow;
    public GameObject CloseWindow;

    private bool isWindow= true;
    private bool canInteract = false;

    //public Animation ChildLeave;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (canInteract &&
            !GameManager.Instance.Task_SO.Task[0].IsAlarm &&
            GameManager.Instance.Task_SO.Task[0].Refrigerator == 2 &&
            GameManager.Instance.Task_SO.Task[0].Stove == 1 &&
            GameManager.Instance.Task_SO.Task[0].Desk == 1&&
            GameManager.Instance.Task_SO.Task[0].Child == 1&&
            GameManager.Instance.Task_SO.Task[0].Bag == 1)
            {
                AlarmEachother();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isWindow)
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isWindow)
        {
            canInteract = false;
        }
    }

    //点击J后执行的事
    public void AlarmEachother()
    {
        //alarmSound.Play();
        EventHandler.CallUpdateTipsUI(11);
        UIManager.Instance.TipPanel.SetActive(true);
        isWindow = false;
        canInteract = false;
        OpenWindow.SetActive(true);
        CloseWindow.SetActive(false);
        GameManager.Instance.Task_SO.Task[0].Window = 1;
    }
}
