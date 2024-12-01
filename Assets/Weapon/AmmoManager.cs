using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Weapon;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; set; }
    
    //UI
    public TextMeshProUGUI ammoDisplay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
