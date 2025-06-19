using System.Collections;
using System.Collections.Generic;
// ManuscriptorAnimationEvents.cs
using UnityEngine;

public class ManuscriptorAnimationEvents : MonoBehaviour
{
    [Tooltip("Referencia al ManuscriptorHealth en el parent")]
    public ManuscriptorHealth health;

    [Tooltip("Referencia al ManuscriptorAI en el parent")]
    public ManuscriptorAI ai;

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
