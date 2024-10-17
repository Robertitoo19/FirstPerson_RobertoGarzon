using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FP : MonoBehaviour
{
    [SerializeField] private float speedMov;
    CharacterController controller;
    void Start()
    {
        //coger componente de character controller
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        MovYRotate();
    }
    void MovYRotate()
    {
        //coger datos de donde te mueves
        float movH = Input.GetAxisRaw("Horizontal");
        float movV = Input.GetAxisRaw("Vertical");

        //movimiento personaje
        Vector3 movement = new Vector3(movH, 0, movV).normalized;

        //sacar arcotangente del mov en x entre el mov en z, convertir radianes a grados y alinear angulo de la cam con el personaje.
        float rotateAngle = Mathf.Atan2(movement.x,movement.z) + Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

        if (movement.magnitude > 0)
        {
            //orientar cuerpo hacia donde apunta la camara
            transform.eulerAngles = new Vector3(0, rotateAngle, 0);
            //movimiento controller
            controller.Move(movement * speedMov * Time.deltaTime);
        }
    }
}
