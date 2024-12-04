using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            print("Dead");
        }
        else
        {
            print("hit");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VampireHand"))
        {
            TakeDamage(other.gameObject.GetComponent<VampireHand>().damage);
        }
    }
}
