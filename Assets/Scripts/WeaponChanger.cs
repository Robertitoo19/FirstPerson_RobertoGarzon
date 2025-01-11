using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField] private GameObject[] guns;
    //recoge indice arma actual.
    private int actualGun;
    //Saber si el arma esta o no desbloqueada
    private bool[] unlockedGuns;
    void Start()
    {
        //el mismo tamaño que el array de cuantas armas hay
        unlockedGuns = new bool[guns.Length];
        //el primer arma esta desbloqueada ya 
        unlockedGuns[0] = true;
    }
    void Update()
    {
        ChangeGunKeyBoard();
        ChangeGunMouse();

    }

    private void ChangeGunMouse()
    {
        float scrollWhell = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;

        if (scrollWhell > 0)
        {
            ChangeWeapon(actualGun + 1);
        }
        else if (scrollWhell < 0)
        {
            ChangeWeapon(actualGun - 1);
        }
    }

    private void ChangeGunKeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            ChangeWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            ChangeWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            ChangeWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            ChangeWeapon(3);
        }
    }

    private void ChangeWeapon(int newGun)
    {
        if (newGun < 0)
        {
            newGun = guns.Length - 1;
        }
        else if (newGun >= guns.Length)
        {
            newGun = 0;
        }
        //si esta desbloqueada, se activa
        if (unlockedGuns[newGun])
        {
            guns[actualGun].SetActive(false);
            guns[newGun].SetActive(true);
            actualGun = newGun;
        }
        else
        {
            Debug.Log("Esta arma no está desbloqueada.");
        }
    }

    public void UnlockWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < guns.Length)
        {
            //desbloquear arma
            unlockedGuns[weaponIndex] = true;
            Debug.Log("Arma desbloqueada" + guns[weaponIndex].name);
        }
    }
}
