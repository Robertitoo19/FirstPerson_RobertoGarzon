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
    private int currentChamber;

    private float reloadTime = 1.5f;
    private bool isReloading = false;
    private Animator anim;

    [SerializeField] private TMP_Text txtCurrentAmmo;
    [SerializeField] private TMP_Text txtCurrentChamber;

    [Header("-----Audio-----")]
    [SerializeField] AudioManager audioManager;
    public AudioClip[] sonidos;
    void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();

        currentAmmo = myData.MaxAmmo;
        currentChamber = myData.chamberBullets;

        txtCurrentAmmo.text = ("" + currentAmmo);
        txtCurrentChamber.text = ("" + currentChamber);
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
            audioManager.ReproducirSFX(sonidos[1]);
            currentAmmo--;
            txtCurrentAmmo.text = ("" + currentAmmo);

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, myData.attackDistance))
            {
                if (hitInfo.transform.TryGetComponent(out EnemyPart enemyPartScript))
                {
                    //quien ha impactado, entrar a su script, hacerle el daño del scriptable.
                    audioManager.ReproducirSFX(sonidos[2]);
                    enemyPartScript.ReceiveDamageEnemy(myData.attackDamage);
                }
            }
        }
    }
    IEnumerator Reload()
    {
        int emptys = myData.MaxAmmo - currentAmmo;
        currentChamber -= emptys;

        if (currentChamber > 0) //Tengo muchas balas en chamber como para cubrir los huecos
        {
            isReloading = true;

            audioManager.ReproducirSFX(sonidos[0]);

            anim.SetTrigger("Reload");

            yield return new WaitForSeconds(reloadTime);

            currentAmmo = myData.MaxAmmo;

            txtCurrentAmmo.text = ("" + currentAmmo);
            txtCurrentChamber.text = ("" + currentChamber);

            isReloading = false;
        }
        else //No tengo suficientes balas en chamber para cubrir todos los huecos
        {
            Debug.Log("no tienes");
        }
    }
}
