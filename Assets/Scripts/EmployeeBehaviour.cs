﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeBehaviour : MonoBehaviour
{
    public bool IsBusy { get; private set; }
    private bool shouldWait;
    private bool platePickup;
    private float timeWaited;
    private Transform currentWorkstation;
    private int currentDestIndex;
    private int currentWaitTimeIndex;
    private List<Transform> destinations;
    private List<float> waitTimes;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float speedBoostMultiplier;
    private ZoneManagment parentZone;
    private bool hasPlate;
    private float timer;
    private float platePickupTimer;

    void Start()
    {
        parentZone = transform.GetComponentInParent<ZoneManagment>();
    }

    void Update()
    {
        if (IsBusy)
        {
            if (Vector3.Distance(destinations[currentDestIndex].position, transform.position) < 0.1f)
            {
                shouldWait = true;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, destinations[currentDestIndex].position, movementSpeed * Time.deltaTime);
            }
            if (shouldWait)
            {
                timer += Time.deltaTime;
                if (timer >= waitTimes[currentDestIndex])
                {
                    shouldWait = false;
                    timer = 0;
                    currentDestIndex++;
                }
            }
            if (currentDestIndex >= destinations.Count)
            {
                IsBusy = false;
                destinations = null;
                currentDestIndex = 0;
            }
        }
        else
        {
            if (!(Vector3.Distance(parentZone.GetWaitingZone().position, transform.position) < 0.1f))
            {
                transform.position = Vector3.MoveTowards(transform.position, parentZone.GetWaitingZone().position, movementSpeed * Time.deltaTime);
            }
        }
    }

    public void BeginTask(Transform station, List<float> waitTimesList)
    {
        IsBusy = true;
        destinations = new List<Transform>();
        destinations.Add(parentZone.GetInputPos());
        destinations.Add(station);
        destinations.Add(parentZone.GetOutputPos());
        waitTimes = waitTimesList;
    }
}
