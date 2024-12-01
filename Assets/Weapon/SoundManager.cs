using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;
    public AudioSource ReloadingSoundGlock19;

    public AudioSource ReloadingSoundM4;

    public AudioClip Glock19Shot;
    public AudioClip M4Shot;
    public AudioSource EmptySoundGlock19;

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

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Glock19:
                ShootingChannel.PlayOneShot(Glock19Shot);
                break;
            case WeaponModel.M4:
                ShootingChannel.PlayOneShot(M4Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Glock19:
                ReloadingSoundGlock19.Play();
                break;
            case WeaponModel.M4:
                ReloadingSoundM4.Play();
                break;
        }
    }
}
