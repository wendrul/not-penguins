﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
    private string ZeroPadding(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
