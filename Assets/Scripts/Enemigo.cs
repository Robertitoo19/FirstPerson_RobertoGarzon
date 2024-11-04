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

    [Header("-----Sistema Combate-----")]
    private bool OpenWindow;
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float detectionRatio;
    [SerializeField] private LayerMask WhatIsDamagable;
    [SerializeField] private float enemyDamage;
    private bool canDamage;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindObjectOfType<FP>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Follow();
        if (OpenWindow && canDamage)
        {
            DetectImpact();
        }
    }

    private void DetectImpact()
    {
        //detectar
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
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            //se para el enemigo.
            agent.isStopped = true;
            //activar anim.
            anim.SetBool("isAttacking", true);
        }
    }

    private void FinAtack()
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
}
