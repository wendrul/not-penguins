﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManagment : MonoBehaviour
{
    [SerializeField] private RessourceZone input;
    [SerializeField] private RessourceZone output;
    [SerializeField] private string zoneName;
    [SerializeField] private Transform stationsTrasform;
    [SerializeField] private Transform waitingZone;
    [SerializeField] private Transform spawnZone;
    [SerializeField] private float employeeSalary;
    [SerializeField] private bool test;
    [SerializeField] GameObject workerPrefab;
    private List<GameObject> employees;
    private List<Workstation> stations;
    private List<EmployeeBehaviour> employeesScripts;
    private List<Order> orders;
    private CentralTransactionLogic zoneManager;
    void Start()
    {
        zoneManager = GetComponentInParent<CentralTransactionLogic>();
        orders = new List<Order>();
        employees = new List<GameObject>();
        employeesScripts = new List<EmployeeBehaviour>();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out EmployeeBehaviour employee))
            {
                employees.Add(child.gameObject);
                employeesScripts.Add(employee);
            }
        }
        stations = new List<Workstation>();
        foreach (Transform s in stationsTrasform)
        {
            if (s.TryGetComponent(out Workstation station))
            {
                stations.Add(station);
            }
        }
    }
    public void CashIn(float amount)
    {
        zoneManager.CashIn(amount);
    }
    public float GiveSalary()
    {
        float total = 0f;
        foreach (EmployeeBehaviour emp in employeesScripts)
        {
            total += employeeSalary * Time.deltaTime / 60;
        }
        return total;
    }

    void Update()
    {
        if (ShouldBeginTask(out Workstation workstation, out EmployeeBehaviour employee, out Order order))
        {
            order.IsBeingPrepared = true;
            BeginTask(workstation, employee, order);
        }
        if (ShouldBringDirtyPlates(out Order o, out Waiter waiter))
        {
            o.IsBeingTakenToClean = true;
            waiter.BringDirtyPlates(o);
        }
        //print($"{zoneName}: {orders.Count} orders here");
    }
    public bool ShouldBringDirtyPlates(out Order order, out Waiter waiter)
    {
        order = null;
        waiter = null;
        EmployeeBehaviour employee = FindFreeEmployee();


        foreach (Order o in orders)
        {
            if (o.Customer.HasFinishedEating)
            {
                order = o;
            }
        }
        if (employee == null || order == null || order.IsBeingTakenToClean)
        {
            return false;
        }
        if (employee.TryGetComponent(out Waiter w))
        {
            waiter = w;
            return true;
        }
        return false;
    }
    public bool ShouldBeginTask(out Workstation workstation, out EmployeeBehaviour employee, out Order order)
    {
        workstation = FindUnoccupiedStation();
        employee = FindFreeEmployee();
        order = CheckForOrders();

        if (workstation == null || employee == null || order == null || order.IsBeingPrepared)
        {
            return false;
        }
        return true;
    }

    private Order CheckForOrders()
    {
        foreach (Order order in orders)
        {
            if (!order.IsBeingPrepared)
            {
                return order;
            }
        }
        return null;
    }

    public void TaskAccomplished(Order order)
    {
        orders.Remove(order);
        output.AddRessources(1);
        zoneManager.SendToNextZone(order, zoneName);
    }
    internal void DrawResource()
    {
        input.RemoveRessources(1);
    }

    public void BeginTask(Workstation station, EmployeeBehaviour employee, Order order)
    {
        employee.BeginTask(station, order);
        test = false;
    }

    private EmployeeBehaviour FindFreeEmployee()
    {
        foreach (EmployeeBehaviour employee in employeesScripts)    
        {
            if (!employee.IsBusy)
            {
                return employee;
            }
        }
        return null;
    }

    private Workstation FindUnoccupiedStation()
    {
        foreach (Workstation station in stations)
        {
            if (!station.InUse)
            {
                return station;
            }
        }
        return null;
    }
    public void HireEmployee()
    {
        GameObject newEmployee = Instantiate(workerPrefab, spawnZone.position, Quaternion.identity, transform);
        employees.Add(newEmployee);
        employeesScripts.Add(newEmployee.GetComponent<EmployeeBehaviour>());
    }

    public void UpgradeEmployee()
    {
        if (employees.Count == 0)
        {
            HireEmployee();
        }
        else
        {
            employeesScripts[0].SalaryRaise();
        }
    }

    public Transform GetInputPos()
    {
        return input.Output;
    }

    public void AssignEmployee(GameObject employee)
    {

    }

    internal Transform GetOutputPos()
    {
        return output.Input;
    }

    internal Transform GetWaitingZone()
    {
        return waitingZone;
    }
    internal string GetName()
    {
        return zoneName;
    }
    public void AddOrder(Order order)
    {
        orders.Add(order);
    }
    public void TurnInOrder(Order order)
    {

    }

    public void Yell()
    {
        foreach (EmployeeBehaviour employee in employeesScripts)
        {
            employee.GotYelledAt = true;
        }
    }
}
