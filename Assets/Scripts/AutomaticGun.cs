using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class AutomaticGun : MonoBehaviour
{
    [SerializeField] private GunSO myData;
    private Camera cam;

    [SerializeField] private ParticleSystem system;

    private float timer;

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

    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }

    void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();

        currentAmmo = myData.MaxAmmo;
        currentChamber = myData.chamberBullets;

        timer = myData.cadence;

        txtCurrentAmmo.text = ("" + currentAmmo);
        txtCurrentChamber.text = ("" + currentChamber);
    }
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
        timer += Time.deltaTime;

        if (Input.GetMouseButton(0) && timer >= myData.cadence && currentAmmo > 0)
        {
            system.Play();
            audioManager.ReproducirSFX(sonidos[0]);
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
            timer = 0;
        }
    }
    IEnumerator Reload()
    {
        int emptys = myData.MaxAmmo - currentAmmo;
        currentChamber -= emptys;

        if (currentChamber > 0)
        {
            isReloading = true;

            audioManager.ReproducirSFX(sonidos[1]);

            anim.SetTrigger("Reload");

            yield return new WaitForSeconds(reloadTime);

            currentAmmo = myData.MaxAmmo;

            txtCurrentAmmo.text = ("" + currentAmmo);
            txtCurrentChamber.text = ("" + currentChamber);

            isReloading = false;
        }
        else if (currentChamber == 0) 
        {
            currentAmmo = currentAmmo + currentChamber;
            currentChamber = 0;
            txtCurrentAmmo.text = ("" + currentAmmo);
            txtCurrentChamber.text = ("" + currentChamber);
        }
    }
}
