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

        timer = myData.cadence;
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
        if ((Input.GetKeyDown(KeyCode.R) && currentAmmo > 0) || currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        Shoot();
    }
    private void Shoot()
    {
        timer += Time.deltaTime;

        if (Input.GetMouseButton(0) && timer >= myData.cadence)
        {
            system.Play();
            currentAmmo--;
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
