﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asshole : Scholar
{

    void Awake()
    {
        this.tag = "Asshole";
        key += this.tag + "_";
    }


    void Update()
    {
        
    }

    public void Bulling(string bullType, bool strong)
    {
        if (strong)
        {
            Stress(10);
            Stop();
            textBox.Say(key + bullType);
            Continue();
        }
        else
        {
            Stop();
            textBox.Say(key + bullType);
            Continue();
        }
    }
}
