using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Melee : MonoBehaviour
{
    [SerializeField] private GunSO myData;
    private Camera cam;

    private Animator anim;

    [Header("-----Audio-----")]
    [SerializeField] AudioManager audioManager;
    public AudioClip[] sonidos;
    void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Shoot();
    }
    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioManager.ReproducirSFX(sonidos[0]);
            anim.SetTrigger("Attack");
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, myData.attackDistance))
            {
                if (hitInfo.transform.TryGetComponent(out EnemyPart enemyPartScript))
                {
                    //quien ha impactado, entrar a su script, hacerle el daño del scriptable.
                    audioManager.ReproducirSFX(sonidos[1]);
                    enemyPartScript.ReceiveDamageEnemy(myData.attackDamage);
                }
            }
        }
    }
}
