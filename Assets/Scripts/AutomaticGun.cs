using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class AutomaticGun : MonoBehaviour
{
    [SerializeField] private GunSO myData;
    private Camera cam;

    [SerializeField] private ParticleSystem system;

    private float timer;
    void Start()
    {
        cam = Camera.main;

        timer = myData.cadence;
    }
    void Update()
    {
        Shoot();
    }
    private void Shoot()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && timer >= myData.cadence)
        {
            system.Play();
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, myData.attackDistance))
            {
                if (hitInfo.transform.TryGetComponent(out EnemyPart enemyPartScript))
                {
                    //quien ha impactado, entrar a su script, hacerle el daño del scriptable.
                    enemyPartScript.ReceiveDamageEnemy(myData.attackDamage);
                }
            }
            timer = 0;
        }
    }
}
