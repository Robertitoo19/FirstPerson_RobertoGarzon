using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FP : MonoBehaviour
{
    [SerializeField] private float pushForce;

    [Header("-----Sistema Movimiento-----")]
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

    [Header("-----Sistema Sprint-----")]
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float sprintDuration;
    [SerializeField] private float sprintCooldown;

    private float currentSprintTime = 0f;
    private bool isSprinting = false;
    private bool canSprint = true;

    [SerializeField] private Slider sprintSlider;

    [Header("-----Sistema Puntos-----")]
    [SerializeField] private float points;
    [SerializeField] private TMP_Text txtPoints;

    [Header("-----Sistema Daño-----")]
    [SerializeField] private float lives;
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private Image damageOverlay;
    //alpha maximo de la imagen.
    [SerializeField] private float overlayMaxAlpha;
    //velocidad transicion de la imagen.
    [SerializeField] private float overlayFadeSpeed;
    private Color overlayColor;

    [Header("-----Audio-----")]
    [SerializeField] AudioManager audioManager;
    public AudioClip[] sonidos;

    public float Lives { get => lives; set => lives = value; }
    public float Points { get => points; set => points = value; }

    void Start()
    {
        //coger componente de character controller
        controller = GetComponent<CharacterController>();
        //esconder mouse.
        Cursor.lockState = CursorLockMode.Locked;

        txtPoints.text = ("" + points);

        currentSprintTime = sprintDuration;

        sprintSlider.maxValue = sprintDuration;
        sprintSlider.value = sprintDuration;

        //coger color de la imagen.
        overlayColor = damageOverlay.color;
        //empieza transparente
        overlayColor.a = 0;
        damageOverlay.color = overlayColor;
        txtHealth.text = ("" + lives);

    }
    void Update()
    {
        // si se da al shift, puede sprintar y la barra tiene tiempo.
        if (Input.GetKey(KeyCode.LeftShift) && canSprint && currentSprintTime > 0)
        {
            StartSprint();
        }
        else
        {
            StopSprint();
        }
        //actualizar slider segun cuanto tenga de sprint.
        sprintSlider.value = currentSprintTime;

        MovYRotate();
        applyGravity();

        if (isGrounded())
        {
            //resetear gravedad
            verticalMovement.y = 0;
            Jump();
        }
        //si no es transparente
        if(overlayColor.a > 0)
        {
            //va desapareciendo segun el faseSpeed, sin salirse de los limites.
            overlayColor.a -= Time.deltaTime * overlayFadeSpeed;
            overlayColor.a = Mathf.Clamp(overlayColor.a, 0, overlayMaxAlpha);
            damageOverlay.color = overlayColor;
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
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        if (input.magnitude > 0)
        {
            //movimiento queda rotado con el angulo de rotacion de la camara (tu frontal es donde apunta la camara).
            Vector3 movement = Quaternion.Euler(0, rotateAngle, 0) * Vector3.forward;
            //si esta esprintando, el current speed es sprint speed y si no es el speedmove.
            float currentSpeed = isSprinting ? sprintSpeed : speedMov;

            //movimiento controller
            controller.Move(movement * currentSpeed * Time.deltaTime);
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
        //subir opacidad proporcional al daño, y no pasarse de los limites.
        overlayColor.a = Mathf.Clamp(overlayColor.a + (enemyDamage / lives) * overlayMaxAlpha, 0, overlayMaxAlpha);
        damageOverlay.color = overlayColor;
        txtHealth.text = ("" + lives);

        audioManager.ReproducirSFX(sonidos[0]);
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
    private void StartSprint()
    {
        isSprinting = true;
        //bajar tiempo sprint.
        currentSprintTime -= Time.deltaTime;
        //no pasar los limites del sprint.
        currentSprintTime = Mathf.Clamp(currentSprintTime, 0, sprintDuration);

        //si se agota el sprint no puedes sprintar.
        if (currentSprintTime <= 0)
        {
            canSprint = false;
            isSprinting = false;
        }
    }

    private void StopSprint()
    {
        isSprinting = false;

        //si no estas sprintando recargar sprint.
        if(!isSprinting)
        {
            //incrementar suave.
            currentSprintTime += Time.deltaTime / sprintCooldown;
            //asegurar q no pasa el limite.
            currentSprintTime = Mathf.Clamp(currentSprintTime, 0, sprintDuration);

            if(currentSprintTime >= sprintDuration)
            {
                canSprint = true;
            }
        }
    }
}
