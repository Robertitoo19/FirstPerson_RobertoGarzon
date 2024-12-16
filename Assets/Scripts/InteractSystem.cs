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
                    if (rocket.enabled && player.Points >= 5000)
                    {
                        player.Points -= 5000;
                        rocket.CurrentChamber += myDataRL.chamberBullets;
                        txtCurrentChamberRL.text = ("" + rocket.CurrentChamber);
                    }
                    else if (manualGun.enabled && player.Points >= 1800)
                    {
                        player.Points -= 1800;
                        manualGun.CurrentChamber += myDataPistol.chamberBullets;
                        txtCurrentChamberPistol.text = ("" + manualGun.CurrentChamber);
                    }
                    else if (automaticGun.enabled && player.Points >= 2000)
                    {
                        player.Points -= 2000;
                        automaticGun.CurrentChamber += myDataAR.chamberBullets;
                        txtCurrentChamberAR.text = ("" + automaticGun.CurrentChamber);
                    }
                }
            }
            if (hit.transform.TryGetComponent(out M4 scriptM4Wall))
            {
                //si lo lleva es un interactuable  
                actualInteract = scriptM4Wall.transform;
                //activar outline
                actualInteract.GetComponent<Outline>().enabled = true;
            }
            if (hit.transform.TryGetComponent(out FirstAid scriptAid))
            {
                //si lo lleva es un interactuable  
                actualInteract = scriptAid.transform;
                //activar outline
                actualInteract.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E)&&player.Points >= 3000) 
                {
                    player.Lives += 25;
                    if(player.Lives > 100)
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
        }
    }
}
