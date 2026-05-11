using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    [Header("衣服设置")]
    [SerializeField] private AudioSource ClothesSound;
    public GameObject HaveClo;
    public GameObject NoClo;

    private bool isClothes = true;
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
            GameManager.Instance.Task_SO.Task[0].Window == 3&&
            GameManager.Instance.Task_SO.Task[0].Robot == 1)
            {
                AlarmEachother();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isClothes)
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isClothes)
        {
            canInteract = false;
        }
    }

    //点击J后执行的事
    public void AlarmEachother()
    {
        //alarmSound.Play();
        EventHandler.CallUpdateTipsUI(15);
        UIManager.Instance.TipPanel.SetActive(true);
        UIManager.Instance.NoteButton.SetActive(true);
        HaveClo.SetActive(false);
        NoClo.SetActive(true);
        isClothes = false;
        canInteract = false;

        GameManager.Instance.Task_SO.Task[0].Clothes = 1;
    }
}
