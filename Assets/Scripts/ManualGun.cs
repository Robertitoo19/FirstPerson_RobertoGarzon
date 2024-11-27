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
    private Animator anim;

    void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();

        currentAmmo = myData.MaxAmmo;
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
        if ((Input.GetKeyDown(KeyCode.R) && currentAmmo > 0) || currentAmmo <= 0 )
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

        anim.SetTrigger("Reload");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = myData.MaxAmmo;

        isReloading = false;
    }
}
