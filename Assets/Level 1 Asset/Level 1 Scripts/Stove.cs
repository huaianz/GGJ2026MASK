using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [Header("灶台设置")]
    [SerializeField] private AudioSource StoveSound;

    public GameObject HaveStove;
    private bool isStove = true;
    private bool canInteract = false;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && canInteract&&
            !GameManager.Instance.Task_SO.Task[0].IsAlarm &&
            GameManager.Instance.Task_SO.Task[0].Refrigerator==1)
        {

            AlarmEachother();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isStove)
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isStove)
        {
            canInteract = false;
        }
    }

    //点击J后执行的事
    public void AlarmEachother()
    {
        //alarmSound.Play();
        EventHandler.CallUpdateTipsUI(2);
        UIManager.Instance.TipPanel.SetActive(true);
        UIManager.Instance.NoteButton.SetActive(true);
        isStove = false;
        canInteract = false;
        HaveStove.SetActive(true);
        GameManager.Instance.Task_SO.Task[0].Stove = 1;
    }
}
