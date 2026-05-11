using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour
{
    [Header("小孩设置")]
    [SerializeField] private AudioSource ChildSound;
    public GameObject Havechild;
    public GameObject Nochild;

    private bool isChild = true;
    private bool canInteract = false;
    private bool isDialogue = false;
    private bool canDialogue = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (canInteract &&
            !GameManager.Instance.Task_SO.Task[0].IsAlarm &&
            GameManager.Instance.Task_SO.Task[0].Refrigerator == 2 &&
            GameManager.Instance.Task_SO.Task[0].Stove == 1 &&
            GameManager.Instance.Task_SO.Task[0].Desk == 1)
            {
                AlarmEachother();
            }
            else if(canDialogue &&
            !GameManager.Instance.Task_SO.Task[0].IsAlarm &&
            GameManager.Instance.Task_SO.Task[0].Refrigerator == 2 &&
            GameManager.Instance.Task_SO.Task[0].Stove == 1 &&
            GameManager.Instance.Task_SO.Task[0].Desk == 1 &&
            GameManager.Instance.Task_SO.Task[0].Child == 1 &&
            GameManager.Instance.Task_SO.Task[0].Bag == 1)
            {
                Dialogue();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isChild)
        {
            canInteract = true;
        }
        if (other.CompareTag("Player") && isDialogue)
        {
            canDialogue = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isChild)
        {
            canInteract = false;
        }
        if (other.CompareTag("Player") && isDialogue)
        {
            canDialogue = false;
        }
    }

    //点击J后执行的事
    public void AlarmEachother()
    {
        //alarmSound.Play();
        EventHandler.CallUpdateTipsUI(5);
        UIManager.Instance.TipPanel.SetActive(true);
        UIManager.Instance.NoteButton.SetActive(true);
        isChild = false;
        canInteract = false;

        GameManager.Instance.Task_SO.Task[0].Child = 1;
        isDialogue = true;
    }

    //对话文本的播放
    public void Dialogue()
    {
        isDialogue = false;
        canDialogue=false;
        StartCoroutine(Wait());
        
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        EventHandler.CallUpdateTipsUI(8);
        yield return new WaitForSeconds(2f);
        EventHandler.CallUpdateTipsUI(9);
        yield return new WaitForSeconds(2f);
        EventHandler.CallUpdateTipsUI(10);
        Nochild.SetActive(true);
        Havechild.SetActive(false);
    }
}
