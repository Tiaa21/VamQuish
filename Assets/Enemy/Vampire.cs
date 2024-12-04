using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : MonoBehaviour
{
    public VampireHand vampireHand;
    public int vampireDamage;

    private void Start()
    {
        vampireHand.damage = vampireDamage;
    }
}
