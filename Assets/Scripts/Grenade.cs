using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float impulseForce;
    [SerializeField] private GameObject prefabExplosion;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
                    //donde apunta z.
        rb.AddForce(transform.forward * impulseForce, ForceMode.Impulse);
        Destroy(gameObject, 2.5f);
    }
    void Update()
    {

    }
    //se ejecuta cuando se muere.
    private void OnDestroy()
    {
        Instantiate(prefabExplosion, transform.position, Quaternion.identity);
    }
}
