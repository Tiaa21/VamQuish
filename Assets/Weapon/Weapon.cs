using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

    //Weapon Damage
    public int weaponDamage;

    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletLifetime = 3f;

    // Shoot
    public bool isShooting, readyToShoot;
    public float shootingDelay = 0.5f;
    private float nextFireTime;
    public GameObject muzzleEffect;

    // Burst Type Weapon
    public int bulletsPerBurst = 3;
    private int burstBulletLeft;

    // Spread
    public float spreadIntensity = 0.5f;

    //Animation
    internal Animator animator;

    //Reloading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    //Weapon Position
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    //Weapon Types

    public enum WeaponModel
    {
        Glock19,
        M4,
        PumpJoti
    }

    //Weapon Model

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
    }

    void Update()
    {
        if (isActiveWeapon)
        {

            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            GetComponent<Outline>().enabled = false;
            
            //Empty Mag
            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.EmptySoundGlock19.Play();
            }
            switch (currentShootingMode)
            {
                case ShootingMode.Auto:
                    isShooting = Input.GetKey(KeyCode.Mouse0);
                    break;

                case ShootingMode.Single:
                case ShootingMode.Burst:
                    isShooting = Input.GetKeyDown(KeyCode.Mouse0);
                    break;
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
            {
                Reload();
            }

            if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                Reload();
            }

            if (isShooting && readyToShoot && bulletsLeft > 0)
            {
                if (currentShootingMode == ShootingMode.Burst)
                {
                    FireBurst();
                }
                else if (currentShootingMode == ShootingMode.Auto)
                {
                    if (Time.time >= nextFireTime)
                    {
                        FireWeapon();
                        nextFireTime = Time.time + shootingDelay;
                    }
                }
                else
                {
                    FireWeapon();
                }
            }

            
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }


    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        animator.SetTrigger("Recoil");

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 ShootingDirection = CalculateDirectionAndSpread().normalized;

        // Spawn the bullet at the weapon's position
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        bullet.transform.forward = ShootingDirection;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 shootDirection = Camera.main.transform.forward;
            rb.AddForce(shootDirection * bulletVelocity, ForceMode.Impulse);
        }

        StartCoroutine(DestroyBullet(bullet, bulletLifetime));
        Invoke("ResetShot", shootingDelay);
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        //Shoot the bullet frm the middle of the screen/the crosshair
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        isReloading = true;

        animator.SetTrigger("Reload");

        Invoke("ReloadDone", reloadTime);
    }

    private void ReloadDone()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void FireBurst()
    {
        readyToShoot = false;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            // Create a slight spread for each bullet in the burst
            float spreadOffsetX = Random.Range(-spreadIntensity, spreadIntensity);
            float spreadOffsetY = Random.Range(-spreadIntensity, spreadIntensity);

            Quaternion spreadRotation = bulletSpawn.rotation * Quaternion.Euler(spreadOffsetY, spreadOffsetX, 0);

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, spreadRotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 shootDirection = Camera.main.transform.forward;
                rb.AddForce(shootDirection * bulletVelocity, ForceMode.Impulse);
            }

            StartCoroutine(DestroyBullet(bullet, bulletLifetime));
        }

        Invoke("ResetShot", shootingDelay);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private IEnumerator DestroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
