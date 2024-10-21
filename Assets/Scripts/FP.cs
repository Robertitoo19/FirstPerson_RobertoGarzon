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
        Vector2 input = new Vector2 (movH,movV).normalized;

        //sacar arcotangente del mov en x entre el mov en z, convertir radianes a grados y alinear angulo de la cam con el personaje.
        float rotateAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        //orientar cuerpo hacia donde apunta la camara
        transform.eulerAngles = new Vector3(0, rotateAngle, 0);

        if (input.magnitude > 0)
        {
            //movimiento queda rotado con el angulo de rotacion de la camara (tu frontal es donde apunta la camara).
            Vector3 movement = Quaternion.Euler(0, rotateAngle, 0) * Vector3.forward;

            //movimiento controller
            controller.Move(movement * speedMov * Time.deltaTime);
        }
    }
}
