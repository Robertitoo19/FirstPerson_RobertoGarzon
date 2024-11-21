using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    [SerializeField] private Enemigo mainScript;
    [SerializeField] private float damageMultiply;
    public void ReceiveDamageEnemy(float attackReceive)
    {
        mainScript.LivesEnemy -= attackReceive * damageMultiply;
        if (mainScript.LivesEnemy <= 0)
        {
            mainScript.Dead();
        }
    }
    public void Explode()
    {

    }
}
