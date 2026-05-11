using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SchoolBag : MonoBehaviour
{
    [Header("书本设置")]
    [SerializeField] private AudioSource BagSound;
    public GameObject Bag;

    private bool isBag = true;
    private bool canInteract = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && canInteract &&
            !GameManager.Instance.Task_SO.Task[0].IsAlarm &&
            GameManager.Instance.Task_SO.Task[0].Refrigerator == 2 &&
            GameManager.Instance.Task_SO.Task[0].Stove == 1 &&
            GameManager.Instance.Task_SO.Task[0].Desk == 1 &&
            GameManager.Instance.Task_SO.Task[0].Child == 1&&
            GameManager.Instance.Task_SO.Task[0].Book==1)
        {

            AlarmEachother();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isBag)
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isBag)
        {
            canInteract = false;
        }
    }

    //点击J后执行的事
    public void AlarmEachother()
    {
        //alarmSound.Play();
        EventHandler.CallUpdateTipsUI(7);
        UIManager.Instance.TipPanel.SetActive(true);
        UIManager.Instance.NoteButton.SetActive(true);
        isBag = false;
        canInteract = false;

        GameManager.Instance.Task_SO.Task[0].Bag = 1;
        Bag.SetActive(false);

    }
}