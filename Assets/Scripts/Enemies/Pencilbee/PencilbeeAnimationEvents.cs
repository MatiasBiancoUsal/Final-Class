using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilbeeAnimationEvents : MonoBehaviour
{
    [Tooltip("Referencia al PencilbeeHealth en el parent")]
    public PencilbeeHealth health;

    [Tooltip("Referencia al PencilbeeAI en el parent")]
    public PencilbeeAI ai;

    public void OnDealMeleeDamage()
        => ai.OnDealMeleeDamage();

    public void OnMeleeAttackEnd()
        => ai.OnMeleeAttackEnd();

    public void OnDeathEnd()
    {
        if (health != null)
            health.OnDeathAnimationEnd();
    }
}

