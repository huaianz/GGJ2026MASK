using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    [Header("冰箱设置")]
    [SerializeField] private AudioSource RefrigeratorSound;

    public GameObject openRefrigerator;
    public GameObject closeRefrigerator;
    public GameObject Juice;

    private bool isRefrigerator = false;
    private bool canInteract = false;

    private bool isopen=true;
    private bool canopen;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!GameManager.Instance.Task_SO.Task[0].IsAlarm&&canInteract&& GameManager.Instance.Task_SO.Task[0].Refrigerator==1&& GameManager.Instance.Task_SO.Task[0].Stove==1)
            {
                AlarmEachother();
            }
            if(canopen&& !GameManager.Instance.Task_SO.Task[0].IsAlarm)
            {
                OpenRefrigerator();
            }
            
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isRefrigerator)
            {
                canInteract = true;
            }
            if (isopen)
            {
                canopen = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isRefrigerator)
        {
            canInteract = false;
        }
        else if (other.CompareTag("Player")&&isopen)//打开冰箱的步骤
        {
            canopen = false;
        }
    }

    //点击J后执行的事
    public void AlarmEachother()
    {
        UIManager.Instance.TipPanel.SetActive(true);
        GameManager.Instance.Task_SO.Task[0].Refrigerator = 2;
        isRefrigerator = false;
        canInteract = false;
        openRefrigerator.SetActive(false);
        closeRefrigerator.SetActive(true);
        Juice.SetActive(true);
        EventHandler.CallUpdateTipsUI(3);
    }

    //打开冰箱
    public void OpenRefrigerator()
    {
        openRefrigerator.SetActive(true);
        closeRefrigerator.SetActive(false);
        //获得鸡蛋
        UIManager.Instance.TipPanel.SetActive(true);
        EventHandler.CallUpdateTipsUI(1);
        isopen = false;
        GameManager.Instance.Task_SO.Task[0].Refrigerator = 1;
        canopen = false;
        isRefrigerator = true;
    }
}
