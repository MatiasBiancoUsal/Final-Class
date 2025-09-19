using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistTrap : MonoBehaviour
{
    [Header("Tiempos")]
    public float preAttackDelay = 0.75f;
    public float riseDuration = 0.25f;
    public float lingerAfter = 0.2f;

    [Header("Movimiento")]
    public float riseHeight = 3f;

    [Header("Daño")]
    public int damage = 30;
    public float hitRadius = 1.5f;
    public LayerMask playerMask;

    [Header("VFX opcional")]
    public GameObject preAttackVFX;
    public GameObject hitVFX;

    Vector3 _startPos;
    Vector3 _endPos;
    bool _attacked;

    void Start()
    {
        _startPos = transform.position;
        _endPos = _startPos + Vector3.up * riseHeight;
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        if (preAttackVFX) preAttackVFX.SetActive(true);
        yield return new WaitForSeconds(preAttackDelay);

        // Subida
        if (preAttackVFX) preAttackVFX.SetActive(false);

        float t = 0f;
        while (t < riseDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / riseDuration);
            transform.position = Vector3.LerpUnclamped(_startPos, _endPos, k);
            yield return null;
        }
        transform.position = _endPos;

        // Golpe (una vez)
        if (!_attacked)
        {
            _attacked = true;
            DoDamage();
            if (hitVFX) Instantiate(hitVFX, _endPos, Quaternion.identity);
        }

        // Espera y destruye
        yield return new WaitForSeconds(lingerAfter);
        Destroy(gameObject);
    }

    private void DoDamage()
    {
        Collider[] hits = Physics.OverlapSphere(_endPos, hitRadius, playerMask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].GetComponentInParent<IDamageable>()?.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = Application.isPlaying ? _endPos : transform.position + Vector3.up * riseHeight;
        Gizmos.DrawWireSphere(center, hitRadius);
    }
}
