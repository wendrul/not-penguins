﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayManagement : MonoBehaviour
{
    [SerializeField] private float employeePrice;
    [SerializeField] private float customerPatience;
    [SerializeField] private float customerTraffic;
    [SerializeField] private float foodPrices;
    [SerializeField] private float foodQuantity;
    [SerializeField] private float dayStart;
    [SerializeField] private float dayEnd;
    [SerializeField] private float timeMultiplier = 3;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject MobileUi;
    [SerializeField] private EmployeeCard[] employeeCards;
    [SerializeField] private TextMeshProUGUI dayCountText;
    [SerializeField] private Boss currentBoss;
    [HideInInspector] public bool dayFinished { get; private set; }
    [HideInInspector] public bool bossActive { get; private set; }



    private float dayClock;
    private bool timeStop;
    private float minutes;
    public int dayCounter { get; set; }
    private TableManager tableManager;

    [SerializeField] private GameObject endOfDayMenu;
    private CentralTransactionLogic CTL;


    void Start()
    {
        dayCounter = 1;
        tableManager = GameObject.Find("TableManager").GetComponent<TableManager>();
        CTL = GameObject.Find("SpaceCantina").GetComponent<CentralTransactionLogic>();
        dayClock = dayStart;
        endOfDayMenu.SetActive(false);
    }

    void Update()
    {
        if (!dayFinished)
        {
            dayClock += timeMultiplier * Time.deltaTime;
            if (!bossActive && dayClock >= dayEnd)
            {
                EndDay();
            }
            UpdateClock();
        }
    }

    private void EndDay()
    {
        dayCounter++;
        for (int i = 0; i < employeeCards.Length; i++)
        {
            employeeCards[i].ResetCard();
        }
        PauseGame();
        PlayTransition();
        ShowEndOfDayMenu();
    }

    private void PlayTransition()
    {
    }

    private void StartBossFight()
    {
        currentBoss.BossCinematic();
    }

  

    private void PauseGame()
    {
        tableManager.FreeTables();
       // MobileUi.SetActive(false);
        dayFinished = true;
        CTL.EmployeeClockOut();
    }

    private void ShowEndOfDayMenu()
    {
        endOfDayMenu.SetActive(true);
        endOfDayMenu.GetComponent<EndOfDay>().SetGameStatus(CTL.GetBalance());
    }

    public void NextDay()
    {
        if (dayCounter == 5)
        {
            bossActive = true;
            StartBossFight();
        }
        dayClock = dayStart;
        CTL.EmployeeClockIn();
        dayFinished = false;
        endOfDayMenu.SetActive(false);
    }

    private void UpdateClock()
    {
        string hour;
        string minute;
        hour = ZeroPadding(Mathf.FloorToInt(dayClock / 60));
        minute = ZeroPadding(Mathf.FloorToInt(dayClock) % 60);
        text.text = hour + ':' + minute;
    }

    public void UpdateDayCounter()
    {
        dayCountText.text = "DAY:" + dayCounter.ToString(); 
    }

    private string ZeroPadding(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
