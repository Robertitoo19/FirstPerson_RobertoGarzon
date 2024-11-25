using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    [SerializeField] private Enemigo mainScript;
    [SerializeField] private float damageMultiply;

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void ReceiveDamageEnemy(float attackReceive)
    {
        mainScript.LivesEnemy -= attackReceive * damageMultiply;

        if (mainScript.LivesEnemy <= 0)
        {
            mainScript.Dead();
        }
    }
    public void Explode(float explosionForce, Vector3 impactPoint, float explosionRatio, float upModifier)
    {
        mainScript.Dead();
        rb.AddExplosionForce(explosionForce, impactPoint, explosionRatio, upModifier, ForceMode.Impulse);
    }
}
