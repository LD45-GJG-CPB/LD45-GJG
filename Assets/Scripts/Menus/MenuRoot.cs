﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuRoot : MonoBehaviour
{
    private MenuPage[] _menuPages;
    private int _activeIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        _menuPages = gameObject.GetComponentsInChildren<MenuPage>();
        
        foreach (var menuPage in _menuPages)
        {
            menuPage.setRoot(this);
            menuPage.Disable();
        }
        Show(0);
    }

    public void Show(int index = 0)
    {
        Debug.Log($"Index: {index}");
        var i = 0;
        foreach (var menuPage in _menuPages)
        {
            Debug.Log(menuPage);
            Debug.Log(i);
            if (i == index)
            {
                menuPage.Enable();
            }
            else
            {
                menuPage.Disable();
            }
            i++;
        }
    }

    public void Show(MenuPage menuPage)
    {
        Show(_menuPages.ToList().IndexOf(menuPage));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}