using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon = null;
    public AmmoPack hoveredAmmoPack = null;

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

    private void Update()
    {
        //Crosshair
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRayCast = hit.transform.gameObject;

            //If Crosshair hovered over weapon, then the weapon will be highlighted
            if (objectHitByRayCast.GetComponent<Weapon>() && objectHitByRayCast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                hoveredWeapon = objectHitByRayCast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickUpWeapon(objectHitByRayCast.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }

        }

    }
}
