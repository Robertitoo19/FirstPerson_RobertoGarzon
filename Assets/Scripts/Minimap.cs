using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform player;

    //se llama despues del fixed y del update normal. Es decir, se actualiza después de q el player se mueva.
    private void LateUpdate()
    {
        //nuevo vector en posicion player
        Vector3 newPosition = player.position;
        //dar la posicion de la cam en y
        newPosition.y = transform.position.y;
        //su nueva posicion en la de la camara
        transform.position = newPosition;
    }
}
