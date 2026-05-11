using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    [Header("衣柜设置")]
    [SerializeField] private AudioSource WardrobeSound;
    public GameObject openWardrobe;
    public GameObject closeWardrobe;
    public GameObject HaveClothes;

    private bool isWardrobe = true;
    private bool canInteract = false;

    private bool Open=true;

    private bool isOpen = false;
    private bool canOpen = false;

    private bool isReclose = false;
    private bool canReclose = false;


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
            GameManager.Instance.Task_SO.Task[0].Window == 3 &&
            GameManager.Instance.Task_SO.Task[0].Robot == 1&&
            GameManager.Instance.Task_SO.Task[0].Clothes==1)
            {
                AlarmEachother();
            }
            if (canOpen)
            {
                OpenWardrobe();
            }
            if (isReclose)
            {
                HaveClothes.SetActive(false);
                closeWardrobe.SetActive(true);
                openWardrobe.SetActive(false);

                isReclose = false;
                canReclose = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isWardrobe&&isOpen)
        {
            canInteract = true;
        }
        if (other.CompareTag("Player")&& Open)
        {
            canOpen=true;
        }
        if (other.CompareTag("Player") && canReclose)
        {
            isReclose = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isWardrobe&&isOpen)
        {
            canInteract = false;
        }
        if (other.CompareTag("Player") && Open)
        {
            canOpen = false;
        }
        if (other.CompareTag("Player") && canReclose)
        {
            isReclose=false;
        }
    }

    //点击J后执行的事
    public void AlarmEachother()
    {
        //alarmSound.Play();
        EventHandler.CallUpdateTipsUI(16);
        UIManager.Instance.TipPanel.SetActive(true);
        UIManager.Instance.NoteButton.SetActive(true);
        isWardrobe = false;
        canInteract = false;
        openWardrobe.SetActive(false);
        closeWardrobe.SetActive(false);
        HaveClothes.SetActive(true);
        GameManager.Instance.Task_SO.Task[0].Wardrobe = 1;
        canReclose = true;
        StartCoroutine(Conclusion());
    }
    private IEnumerator Conclusion()
    {
        yield return new WaitForSeconds(2f);
        EventHandler.CallUpdateTipsUI(17);

    }

    //打开衣柜
    public void OpenWardrobe()
    {
        closeWardrobe.SetActive(false);
        openWardrobe.SetActive(true);
        HaveClothes.SetActive(false);
        isOpen = true;
        Open=false;
        canOpen = false;
    }
}
