using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [Header("扫地机器人设置")]
    [SerializeField] private AudioSource RobotSound;
    //public GameObject OpenWindow;
    //public GameObject CloseWindow;

    private bool isRobot = true;
    private bool canInteract = false;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (canInteract &&
            !GameManager.Instance.Task_SO.Task[0].IsAlarm &&
            GameManager.Instance.Task_SO.Task[0].Refrigerator == 2 &&
            GameManager.Instance.Task_SO.Task[0].Stove == 1 &&
            GameManager.Instance.Task_SO.Task[0].Desk == 1 &&
            GameManager.Instance.Task_SO.Task[0].Child == 1 &&
            GameManager.Instance.Task_SO.Task[0].Bag == 1 &&
            GameManager.Instance.Task_SO.Task[0].Window == 3)
            {
                AlarmEachother();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isRobot)
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isRobot)
        {
            canInteract = false;
        }
    }

    //点击J后执行的事
    public void AlarmEachother()
    {
        //alarmSound.Play();
        EventHandler.CallUpdateTipsUI(14);
        UIManager.Instance.TipPanel.SetActive(true);
        UIManager.Instance.NoteButton.SetActive(true);
        isRobot = false;
        canInteract = false;

        GameManager.Instance.Task_SO.Task[0].Robot = 1;
    }
}
