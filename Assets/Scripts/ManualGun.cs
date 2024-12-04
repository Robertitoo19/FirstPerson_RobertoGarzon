using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManualGun : MonoBehaviour
{
    [SerializeField] private GunSO myData;
    private Camera cam;

    [SerializeField] ParticleSystem system;

    [Header("-----Reload-----")]

    private int currentAmmo;
    //private int currentChamber;

    private float reloadTime = 1.5f;
    private bool isReloading = false;
    private Animator anim;

    [SerializeField] private TMP_Text txtCurrentAmmo;

    void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();

        currentAmmo = myData.MaxAmmo;
        //currentChamber = myData.chamberBullets;

        txtCurrentAmmo.text = ("" + currentAmmo);
    }
    //Como un update
    //Cuando no haya terminado de cargar y se cambie de arma, al volver siga recargando.
    private void OnEnable()
    {
        isReloading = false;
    }
    void Update()
    {
        if (isReloading)
        {
            return; 
        }
        if ((Input.GetKeyDown(KeyCode.R) && currentAmmo < myData.MaxAmmo))
        {
            StartCoroutine(Reload());
            return;
        }

        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            system.Play();
            currentAmmo--;
            txtCurrentAmmo.text = ("" + currentAmmo);

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, myData.attackDistance))
            {
                if (hitInfo.transform.TryGetComponent(out EnemyPart enemyPartScript))
                {
                    //quien ha impactado, entrar a su script, hacerle el daño del scriptable.
                    enemyPartScript.ReceiveDamageEnemy(myData.attackDamage);
                }
            }
        }
    }
    IEnumerator Reload()
    {
        //int huecos = myData.MaxAmmo - currentAmmo;
        //currentChamber -= huecos;
        //if() //Tengo muchas balas en chamber como para cubrir los huecos
        //{

        //}
        //else //No tengo suficientes balas en chamber para cubrir todos los huecs....
        //{

        //}

        isReloading = true;

        anim.SetTrigger("Reload");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = myData.MaxAmmo;

        txtCurrentAmmo.text = ("" + currentAmmo);

        isReloading = false;
    }
}
