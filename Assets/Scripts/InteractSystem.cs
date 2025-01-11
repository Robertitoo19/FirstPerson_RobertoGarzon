using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class InteractSystem : MonoBehaviour
{
    private Camera cam;
    [Header("-----Externo-----")]
    [SerializeField] private AutomaticGun automaticGun;
    [SerializeField] private ManualGun manualGun;
    [SerializeField] private RocketLauncher rocket;
    [SerializeField] private FP player;
    [SerializeField] private GunSO myDataAR;
    [SerializeField] private GunSO myDataRL;
    [SerializeField] private GunSO myDataPistol;
    [SerializeField] private TMP_Text txtCurrentChamberAR;
    [SerializeField] private TMP_Text txtCurrentChamberRL;
    [SerializeField] private TMP_Text txtCurrentChamberPistol;
    [SerializeField] private TMP_Text txtPoints;
    [SerializeField] private TMP_Text txtLives;
    [SerializeField] private TMP_Text txtWeaponPrice;


    [SerializeField] private float interactDistance;
    private Transform actualInteract;


    void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        InteractDetection();
    }

    private void InteractDetection()
    {
        //conseguimos ver algo
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactDistance))
        {
            //ver si lleva el script de caja municion
            if (hit.transform.TryGetComponent(out CajaMunicion scritpAmmoBox))
            {
                //si lo lleva es un interactuable  
                actualInteract = scritpAmmoBox.transform;
                //activar outline
                actualInteract.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    scritpAmmoBox.OpenBox();

                    if (rocket.gameObject.activeSelf && player.Points >= 5000)
                    {
                        player.Points -= 5000;
                        rocket.CurrentChamber += myDataRL.chamberBullets;
                        txtPoints.text = ("" + player.Points);
                        txtCurrentChamberRL.text = ("" + rocket.CurrentChamber);
                    }
                    else if (automaticGun.gameObject.activeSelf && player.Points >= 2500)
                    {
                        player.Points -= 2500;
                        automaticGun.CurrentChamber += myDataAR.chamberBullets;
                        txtPoints.text = ("" + player.Points);
                        txtCurrentChamberAR.text = ("" + automaticGun.CurrentChamber);
                    }
                    else if (manualGun.gameObject.activeSelf && player.Points >= 1200)
                    {
                        player.Points -= 1200;
                        manualGun.CurrentChamber += myDataPistol.chamberBullets;
                        txtPoints.text = ("" + player.Points);
                        txtCurrentChamberPistol.text = ("" + manualGun.CurrentChamber);
                    }
                }
            }
            if (hit.transform.TryGetComponent(out WeaponShop scriptShop))
            {
                //si lo lleva es un interactuable  
                actualInteract = scriptShop.transform;
                //activar outline
                actualInteract.GetComponent<Outline>().enabled = true;
                //mostrar el precio del arma
                txtWeaponPrice.text = ("Purchase " + scriptShop.WeaponPrice1 + " Points");

                if (Input.GetKeyDown(KeyCode.E) && player.Points >= scriptShop.WeaponPrice1)
                {
                    player.Points -= scriptShop.WeaponPrice1;
                    txtPoints.text = ("" + player.Points);
                    scriptShop.UnlockWeapon();
                    //si compras se limpia el texto
                    txtWeaponPrice.text = ("");
                }
            }
            if (hit.transform.TryGetComponent(out FirstAid scriptAid))
            {
                //si lo lleva es un interactuable  
                actualInteract = scriptAid.transform;
                //activar outline
                actualInteract.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E) && player.Points >= 2500 && player.Lives < 100) 
                {
                    player.Lives += 33;
                    player.Points -= 2500;
                    txtPoints.text = ("" + player.Points);
                    txtLives.text = ("" + player.Lives);

                    if (player.Lives > 100)
                    {
                        player.Lives = 100;
                    }
                }
            }
        }
        //si no ves nada
        else if (actualInteract != null)
        {
            //quitar outline
            actualInteract.GetComponent<Outline>().enabled = false;
            //ya no ves interactuable
            actualInteract = null;
            txtWeaponPrice.text = "";
        }
    }
}
