using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualGun : MonoBehaviour
{
    [SerializeField] private GunSO myData;
    private Camera cam;

    [SerializeField] ParticleSystem system;
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            system.Play();
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, myData.attackDistance))
            {
                if (hitInfo.transform.CompareTag("EnemyPart"))
                {
                    //quien ha impactado, entrar a su script, hacerle el daño del scriptable.
                    hitInfo.transform.GetComponent<Enemigo>().ReceiveDamageEnemy(myData.attackDamage);
                }
            }
            
        }
    }
}
