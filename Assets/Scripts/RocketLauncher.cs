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
    private float reloadTime = 1.5f;
    private bool isReloading = false;
    private Animator anim;

    [SerializeField] private TMP_Text txtCurrentAmmo;
    void Start()
    {
        anim = GetComponent<Animator>();

        currentAmmo = myData.MaxAmmo;

        txtCurrentAmmo.text = ("" + currentAmmo);
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
            currentAmmo--;
            txtCurrentAmmo.text = ("" + currentAmmo);
            //instanciar copia del prefab granada en la boca del bazoka. misma rotacion q el bazooka.
            Instantiate(grenadePrefab, spawnPoint.position, transform.rotation);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;

        anim.SetTrigger("Reload");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = myData.MaxAmmo;

        txtCurrentAmmo.text = ("" + currentAmmo);

        isReloading = false;
    }
}
