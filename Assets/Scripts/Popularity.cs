﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popularity : MonoBehaviour
{
    [SerializeField] private GameOver gameOverScreen;

    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdatePopularity(int popularity)
    {
        slider.value += popularity;
        if (slider.value == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
    }
}
