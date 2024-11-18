using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField] private GameObject[] guns;
    //recoge indice arma actual.
    private int actualGun;
    void Start()
    {
        
    }
    void Update()
    {
        ChangeGunKeyBoard();
        ChangeGunMouse();

    }

    private void ChangeGunMouse()
    {
        float scrollWhell = Input.GetAxis("Mouse ScrollWheel");

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
        //desactivar arma actual.
        guns[actualGun].SetActive(false);
        
        //si el indice es negativo.
        if (newGun < 0)
        {
            //indice es el ultimo de la lista
            newGun = guns.Length - 1;
        }
        else if(newGun > guns.Length)
        {
            newGun = 0;
        }

        //activar nueva arma.
        guns[newGun].SetActive(true);

        actualGun = newGun;
    }
}
