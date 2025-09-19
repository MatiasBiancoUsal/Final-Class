using System.Collections;
using System.Collections.Generic;
// ManuscriptorAnimationEvents.cs
using UnityEngine;

public class Boss3AnimationEvents : MonoBehaviour
{
    [Tooltip("Referencia al ManuscriptorHealth en el parent")]
    public Boss3Health health;

    [Tooltip("Referencia al ManuscriptorAI en el parent")]
    public Boss3AI ai;

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
