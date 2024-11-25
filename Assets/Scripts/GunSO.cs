using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Arma")]
public class GunSO : ScriptableObject
{
    //DATOS
    public int MaxAmmo;
    public int chamberBullets;
    public float cadence;
    public float attackDistance;
    public float attackDamage;
}
