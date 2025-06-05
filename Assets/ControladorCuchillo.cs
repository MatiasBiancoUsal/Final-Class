using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    public Animator animator;        
    public GameObject knifeObject;  

    public float attackDuration = 0.5f; 
    public float cooldown = 3f;          // Tiempo de espera entre ataques

    private bool canAttack = true;

    void Start()
    {
        knifeObject.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canAttack)
        {
            StartCoroutine(AttackSequence());
        }
    }

    IEnumerator AttackSequence()
    {
        canAttack = false;

        knifeObject.SetActive(true); 
        animator.SetTrigger("atacar");

        yield return new WaitForSeconds(attackDuration);

        knifeObject.SetActive(false); 

        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
