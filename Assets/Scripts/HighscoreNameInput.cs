﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreNameInput : MonoBehaviour
{

    public TextMeshProUGUI nameInput;
    public Score score;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveName()
    {
        var name = nameInput.text;
        Debug.Log($"[HighscoreNameInput] received name: {name.ToString()}");

        StartCoroutine(HighScoreAPI.Save(name, score.GetScore(), (s =>
        {
            
        })));
        }
}