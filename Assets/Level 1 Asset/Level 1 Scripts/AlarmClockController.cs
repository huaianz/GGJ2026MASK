using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using static IInteractable;

public class AlarmClockController : MonoBehaviour
{
    [Header("闹钟设置")]
    [SerializeField] private AudioSource alarmSound; // 闹钟声音
    public GameObject Openalarm;
    public GameObject Closealarm;


    private bool isAlarmOn=true;
    private bool canInteract = false;

    private void Start()
    {
        //if (isAlarmOn && canInteract)
        //{
        //    alarmSound.Play();
        //    alarmAnimation.Play();
        //}
    }

    public void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.J))
        {

            AlarmEachother();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&&isAlarmOn)
        {
            EventHandler.CallUpdateTipsUI(0);

        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player")&&isAlarmOn)
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")&&isAlarmOn)
        {
            UIManager.Instance.TipPanel.SetActive(false);
            canInteract=false;
        }
    }
    
    //点击J后执行的事
    public void AlarmEachother()
    {
        //alarmSound.Stop();
        //alarmAnimation.Stop();
        UIManager.Instance.TipPanel.SetActive(false);
        UIManager.Instance.NoteButton.SetActive(true);
        isAlarmOn = false;
        canInteract = false;

        GameManager.Instance.Task_SO.Task[0].IsAlarm=false;
        Closealarm.SetActive(true);
        Openalarm.SetActive(false);
    }
}
