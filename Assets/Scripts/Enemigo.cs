using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    private NavMeshAgent agent;
    private FP player;
    private Animator anim;
    private AudioSource source;

    private bool OpenWindow;
    [Header("-----SistemaCombate-----")]
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float detectionRatio;
    [SerializeField] private LayerMask WhatIsDamagable;
    [SerializeField] private float enemyDamage;
    private bool canDamage;

    Rigidbody[] joints;

    [SerializeField] private float enemyPoints;
    private bool isDead = false;

    [SerializeField] private float livesEnemy;

    [Header("-----Audio-----")]
    [SerializeField] AudioManager audioManager;
    public AudioClip[] sonidos;
    public float LivesEnemy { get => livesEnemy; set => livesEnemy = value; }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindObjectOfType<FP>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        joints = GetComponentsInChildren<Rigidbody>();

        ChangeJointsState(true);
    }
    void Update()
    {
        if(agent.enabled)
        {
            Follow();
        }
        if (OpenWindow && canDamage)
        {
            DetectImpact();
        }
    }

    private void DetectImpact()
    {
        //detectar colliders.

        Collider[] collDetectates = Physics.OverlapSphere(AttackPoint.position, detectionRatio, WhatIsDamagable);
        //si ha detectado algo en el ataque.
        if(collDetectates.Length > 0)
        {
            //aplicas daño a cada colllider
            for(int i = 0; i < collDetectates.Length; i++)
            {
                collDetectates[i].GetComponent<FP>().ReceiveDamage(enemyDamage);
            }
            canDamage = false;
        }
    }

    private void Follow()
    {
        //ir a la posicion del player.
        agent.SetDestination(player.transform.position);
        //el enemigo esta cerca del player.
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            //se para el enemigo.
            agent.isStopped = true;
            //activar anim.
            anim.SetBool("isAttacking", true);
            AimObjetive();
        }
    }

    private void AimObjetive()
    {
        //sacar vector que mira al player desde nuestra posicion.
        Vector3 objetiveDirection = (player.transform.position - transform.position).normalized;
        //enemigo no se tumbe.
        objetiveDirection.y = 0;
        //calcular rotacion para conseguir rotacion.
        Quaternion objetiveRotation = Quaternion.LookRotation(objetiveDirection);

        transform.rotation = objetiveRotation;
    }

    private void EndAttack()
    {
        agent.isStopped = false;
        anim.SetBool("isAttacking", false);
        canDamage = true;
    }
    private void OpenAttackWindow()
    {
        OpenWindow = true;
    }
    private void CloseAttackWindow()
    {
        OpenWindow = false;
    }
    private void ChangeJointsState(bool State)
    {
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].isKinematic = State;
        }
    }
    public void Dead()
    {
        if(!isDead) //Si aún no estoy muerto...
        {
            ChangeJointsState(false);
            anim.enabled = false;
            agent.enabled = false;
            source.enabled = false;
            player.GetComponent<FP>().ReceivePoints(enemyPoints);
            FindObjectOfType<RoundsManager>().EnemyDefeated();
            Destroy(gameObject, 15);
            isDead = true;
        }
    }
}
