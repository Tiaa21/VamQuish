using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
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
    private Animator animator;

    //Reloading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

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

        if (AmmoManager.Instance.ammoDisplay != null)
        {
           AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft/bulletsPerBurst}/{magazineSize/bulletsPerBurst}";
        }
    }
    

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        animator.SetTrigger("Recoil");

        SoundManager.Instance.shootingSoundGlock19.Play();

        readyToShoot = false;

        // Spawn the bullet at the weapon's position
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 shootDirection = Camera.main.transform.forward;
            rb.AddForce(shootDirection * bulletVelocity, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Bullet prefab is missing a Rigidbody component!");
        }

        StartCoroutine(DestroyBullet(bullet, bulletLifetime));
        Invoke("ResetShot", shootingDelay);
    }

    private void Reload()
    {
        isReloading = true;
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
