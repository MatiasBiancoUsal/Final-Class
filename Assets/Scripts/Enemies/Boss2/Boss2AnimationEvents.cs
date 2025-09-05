using System.Collections;
using System.Collections.Generic;
// ManuscriptorAnimationEvents.cs
using UnityEngine;

public class Boss2AnimationEvents : MonoBehaviour
{
    [Tooltip("Referencia al ManuscriptorHealth en el parent")]
    public Boss2Health health;

    [Tooltip("Referencia al ManuscriptorAI en el parent")]
    public Boss2AI ai;

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
