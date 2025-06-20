using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceroAnimationEvents : MonoBehaviour
{
    [Tooltip("Referencia al script LanceroAI en el padre")]
    public LanceroAI ai;

    [Tooltip("Referencia al script LanceroHealth en el padre")]
    public LanceroHealth health;

    public void OnDealMeleeDamage()
    {
        ai?.OnDealMeleeDamage();
    }

    public void OnAttackEnd()
    {
        ai?.OnAttackEnd();
    }

    public void OnDeathEnd()
    {
        health?.OnDeathAnimationEnd();
    }
}
