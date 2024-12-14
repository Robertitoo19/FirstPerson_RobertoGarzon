using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FP : MonoBehaviour
{
    [SerializeField] private float lives;

    [Header("-----Movimiento-----")]
    [SerializeField] private float speedMov;
    CharacterController controller;
    [SerializeField] private float gravityFactor;
    private Vector3 verticalMovement;
    [SerializeField] private float jumpHeight;

    [Header("-----Detección suelo-----")]
    [SerializeField] private float detectionRatio;
    //para acceder directo al transform
    [SerializeField] private Transform feets;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float pushForce;

    [Header("-----Puntos-----")]
    [SerializeField] private float points;
    [SerializeField] private TMP_Text txtPoints;

    public float Lives { get => lives; set => lives = value; }
    public float Points { get => points; set => points = value; }

    void Start()
    {
        //coger componente de character controller
        controller = GetComponent<CharacterController>();
        //esconder mouse.
        Cursor.lockState = CursorLockMode.Locked;

        txtPoints.text = ("" + points);
    }
    void Update()
    {
        MovYRotate();
        applyGravity();

        if (isGrounded())
        {
            //resetear gravedad
            verticalMovement.y = 0;
            Jump();
        }
    }
    private void MovYRotate()
    {
        //coger datos de donde te mueves
        float movH = Input.GetAxisRaw("Horizontal");
        float movV = Input.GetAxisRaw("Vertical");

        //movimiento personaje
        Vector2 input = new Vector2 (movH,movV).normalized;

        //sacar arcotangente del mov en x entre el mov en z, convertir radianes a grados y alinear angulo de la cam con el personaje.
        float rotateAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

        //cuerpo gire con la camara
        transform.rotation = quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        if (input.magnitude > 0)
        {
            //movimiento queda rotado con el angulo de rotacion de la camara (tu frontal es donde apunta la camara).
            Vector3 movement = Quaternion.Euler(0, rotateAngle, 0) * Vector3.forward;

            //movimiento controller
            controller.Move(movement * speedMov * Time.deltaTime);
        }
    }
    private void applyGravity()
    {
        //mi velocidad vertical crece en Y a cierta velocidad x segundo.
        verticalMovement.y += gravityFactor * Time.deltaTime;
        //movimiento en y, otra vez x delta xq la gravedad es al cuadrado.
        controller.Move(verticalMovement * Time.deltaTime);
        //(se acumula la gravedad y baja raro)
    }
    private bool isGrounded()
    {
        //hacer esfera de detencion en los pies para saber si estoy suelo.
        //detectar posicion de los pies con transform.
        bool result = Physics.CheckSphere(feets.position, detectionRatio, whatIsGround);
        return result;
    }
    //metodo automatico para dibujar figuras.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(feets.position, detectionRatio);
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //formula para saltar la altura que quiera.
            verticalMovement.y = math.sqrt(-2 * gravityFactor * jumpHeight);
        }
    }
    public void ReceiveDamage(float enemyDamage)
    {
        lives -= enemyDamage;

        if (lives <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }
    public void ReceivePoints(float enemyPoints)
    {
        points += enemyPoints;
        txtPoints.text = ("" + points);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("EnemyPart"))
        {
            //restar posicion enemigo - posicion player
            Vector3 Push = hit.gameObject.transform.position - transform.position;
            hit.gameObject.GetComponent<Rigidbody>().AddForce(Push.normalized * pushForce, ForceMode.Impulse);
        }
    }
}
