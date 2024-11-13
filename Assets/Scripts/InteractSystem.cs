using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class InteractSystem : MonoBehaviour
{
    private Camera cam;
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
                    scritpAmmoBox.OpneBox();
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
