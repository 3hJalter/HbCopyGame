using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    // public static UIManager Instance
    // {
    //     get
    //     {
    //         if (_instance == null)
    //         {
    //             _instance = FindObjectOfType<UIManager>();
    //         }
    //
    //         return _instance; 
    //     }
    // }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private TextMeshProUGUI coinText;

    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
}
