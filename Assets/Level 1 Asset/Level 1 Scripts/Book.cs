using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [Header("书本设置")]
    [SerializeField] private AudioSource BookSound;
    public GameObject haveBook;
    public GameObject nobook;

    private bool isBook = true;
    private bool canInteract = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && canInteract &&
            !GameManager.Instance.Task_SO.Task[0].IsAlarm &&
            GameManager.Instance.Task_SO.Task[0].Refrigerator == 2 &&
            GameManager.Instance.Task_SO.Task[0].Stove == 1 &&
            GameManager.Instance.Task_SO.Task[0].Desk == 1&&
            GameManager.Instance.Task_SO.Task[0].Child==1)
        {

            AlarmEachother();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isBook)
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isBook)
        {
            canInteract = false;
        }
    }

    //点击J后执行的事
    public void AlarmEachother()
    {
        //alarmSound.Play();
        EventHandler.CallUpdateTipsUI(6);
        UIManager.Instance.TipPanel.SetActive(true);
        UIManager.Instance.NoteButton.SetActive(true);
        isBook = false;
        canInteract = false;
        nobook.SetActive(true);
        haveBook.SetActive(false);
        GameManager.Instance.Task_SO.Task[0].Book = 1;
    }
}
