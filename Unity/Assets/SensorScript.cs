﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorScript : MonoBehaviour
{

    protected int sensorID;
    protected Model model;
    
    public SensorScript(int sensorID, Model model){
        this.sensorID = sensorID;
        this.model = model;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}