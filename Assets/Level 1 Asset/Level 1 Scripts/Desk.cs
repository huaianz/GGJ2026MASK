using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{
    [Header("桌子设置")]
    [SerializeField] private AudioSource DeskSound;
    public GameObject Food;

    private bool isDesk = true;
    private bool canInteract = false;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && canInteract &&
            !GameManager.Instance.Task_SO.Task[0].IsAlarm &&
            GameManager.Instance.Task_SO.Task[0].Refrigerator == 2&&
            GameManager.Instance.Task_SO.Task[0].Stove==1)
        {

            AlarmEachother();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDesk)
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDesk)
        {
            canInteract = false;
        }
    }

    //点击J后执行的事
    public void AlarmEachother()
    {
        //alarmSound.Play();
        EventHandler.CallUpdateTipsUI(4);
        UIManager.Instance.TipPanel.SetActive(true);
        UIManager.Instance.NoteButton.SetActive(true);
        Food.SetActive(true);
        isDesk = false;
        canInteract = false;

        GameManager.Instance.Task_SO.Task[0].Desk = 1;
    }
}
