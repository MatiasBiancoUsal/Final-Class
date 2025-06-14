using System.Collections;
using System.Collections.Generic;
// EnemyAnimationEvents.cs
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [Tooltip("Referencia al script EnemyChase en el padre o raíz")]
    public EnemyChase enemyChase;

    // Llamar desde el frame de disparo ranged
    public void OnSpawnProjectile()
    {
        enemyChase.SpawnProjectile();
    }

    // Llamar desde el frame de impacto melee
    public void OnDealMelee()
    {
        enemyChase.DealMeleeDamage();
    }

    // Llamar en el último frame de MeleeAttack
    public void OnMeleeAttackEnd()
    {
        enemyChase.OnMeleeAttackEnd();
    }

    // Llamar en el último frame de RangedAttack
    public void OnRangedAttackEnd()
    {
        enemyChase.OnRangedAttackEnd();
    }

    // Opcional: si tienes animación de muerte y quieres destruir al final
    public void OnDeathEnd()
    {
        Destroy(enemyChase.gameObject);
    }
}
