using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GunSO myData;

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
        anim = GetComponent<Animator>();

        currentAmmo = myData.MaxAmmo;
        currentChamber = myData.chamberBullets;

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
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            audioManager.ReproducirSFX(sonidos[0]);
            currentAmmo--;
            txtCurrentAmmo.text = ("" + currentAmmo);
            //instanciar copia del prefab granada en la boca del bazoka. misma rotacion q el bazooka.
            Instantiate(grenadePrefab, spawnPoint.position, transform.rotation);
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
        else 
        {
            Debug.Log("no tienes");
        }
    }
}
