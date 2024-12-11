using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class InteractSystem : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private AutomaticGun automaticGun;
    [SerializeField] private ManualGun manualGun;
    [SerializeField] private RocketLauncher rocket;
    [SerializeField] private FP player;

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
                    if (rocket.enabled)
                    {
                        rocket.CurrentAmmo++;
                    }
                    else if (manualGun.enabled)
                    {
                        Debug.Log("suma muni");
                        manualGun.CurrentAmmo++;
                    }
                    else if (automaticGun.enabled)
                    {
                        automaticGun.CurrentAmmo++;
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

                if (Input.GetKeyDown(KeyCode.E) && player.Lives < 100) 
                {
                    player.Lives += 25;
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
