using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform spawnPoint;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //instanciar copia del prefab granada en la boca del bazoka. misma rotacion q el bazooka.
            Instantiate(grenadePrefab, spawnPoint.position, transform.rotation);
        }
    }
}
