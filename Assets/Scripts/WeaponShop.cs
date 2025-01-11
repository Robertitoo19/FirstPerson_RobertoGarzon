using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    [SerializeField] private string WeaponName;
    [SerializeField] private int WeaponPrice;
    [SerializeField] private int WeaponIndex;
    [SerializeField] private WeaponChanger weaponScript;

    public int WeaponPrice1 { get => WeaponPrice; set => WeaponPrice = value; }

    public void UnlockWeapon()
    {
        //desbloquear arma
        weaponScript.UnlockWeapon(WeaponIndex);
        Debug.Log("Armas desbloqueada" + WeaponName);
    }
}
