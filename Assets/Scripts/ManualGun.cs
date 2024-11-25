using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualGun : MonoBehaviour
{
    [SerializeField] private GunSO myData;
    private Camera cam;

    [SerializeField] ParticleSystem system;

    [Header("-----Reload-----")]

    private int currentAmmo;
    private float reloadTime = 1.5f;
    private bool isReloading = false;

    void Start()
    {
        cam = Camera.main;

        currentAmmo = myData.MaxAmmo;
    }
    void Update()
    {
        if (isReloading)
        {
            return; 
        }
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        system.Play();
        currentAmmo--;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, myData.attackDistance))
        {
            if (hitInfo.transform.CompareTag("EnemyPart"))
            {
                //quien ha impactado, entrar a su script, hacerle el daño del scriptable.
                hitInfo.transform.GetComponent<EnemyPart>().ReceiveDamageEnemy(myData.attackDamage);
            }
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = myData.MaxAmmo;

        isReloading = false;
    }
}
