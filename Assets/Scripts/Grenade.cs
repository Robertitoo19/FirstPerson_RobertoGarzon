using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float impulseForce;
    [SerializeField] private float explosionRatio;
    [SerializeField] private float explosionForce;
    [SerializeField] private GameObject prefabExplosion;
    [SerializeField] private LayerMask whatisExplotable;

    [Header("-----Audio-----")]
    public AudioClip[] sonidos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
                    //donde apunta z.
        rb.AddForce(transform.forward * impulseForce, ForceMode.Impulse);
        Destroy(gameObject, 1.5f);
    }
    void Update()
    {

    }
    //se ejecuta cuando choca.
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        AudioManager.audioManager.ReproducirSFX(sonidos[0]);
    }
    //se ejecuta cuando se muere.
    private void OnDestroy()
    {
        Instantiate(prefabExplosion, transform.position, Quaternion.identity);

        Collider[] collsDetected = Physics.OverlapSphere(transform.position, explosionRatio, whatisExplotable);

        if (collsDetected.Length > 0)
        {
            for (int i = 0; i < collsDetected.Length; i++)
            {
                if(collsDetected[i].TryGetComponent(out EnemyPart scriptJoints))
                {
                    scriptJoints.Explode(explosionForce, transform.position, explosionRatio, 3.5f);
                }
            }
        }
    }
}
