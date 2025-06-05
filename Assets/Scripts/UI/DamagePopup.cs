using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Popup flotante que muestra el daño infligido. Se instancia con
/// DamagePopup.Create(prefab, enemigo, daño);
/// </summary>
public class DamagePopup : MonoBehaviour
{
    /* Ajustes en Inspector --------------------------------------- */
    [Header("Movimiento")]
    [SerializeField] private float floatSpeed = 1f;   // subida vertical
    [SerializeField] private float lifeTime = 1f;   // tiempo hasta empezar fade-out
    [SerializeField] private float fadeTime = 0.5f; // duración del fade-out
    [SerializeField] private float spawnRadius = 0.3f; // dispersión horizontal (XZ)
    [SerializeField] private float yOffset = 1.8f; // altura base sobre el enemigo
    /* ------------------------------------------------------------ */

    private TMP_Text _text;
    private Transform _target; // enemigo a seguir
    private Vector3 _offset;   // offset aleatorio fijo
    private float _timer;
    private Color _startColor;
    private Transform _cam;

    /* ---------- FÁBRICA ESTÁTICA ---------- */
    public static void Create(DamagePopup prefab, Transform enemy, int dmg)
    {
        if (!prefab || !enemy) return;

        // offset aleatorio en círculo (plano X-Z) + altura
        Vector2 rnd = Random.insideUnitCircle * prefab.spawnRadius;
        Vector3 offset = new Vector3(rnd.x, prefab.yOffset, rnd.y);

        DamagePopup popup =
            Instantiate(prefab, enemy.position + offset, Quaternion.identity);
        popup.Init(enemy, offset, dmg);
    }
    /* -------------------------------------- */

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _startColor = _text.color;
        _cam = Camera.main ? Camera.main.transform : null;

        // Render por encima del mundo
        Renderer rend = _text.GetComponentInChildren<Renderer>();
        rend.sortingOrder = 500;                       // o Sorting Layer propia
    }

    private void Init(Transform enemy, Vector3 offset, int dmg)
    {
        _target = enemy;
        _offset = offset;

        _text.SetText(dmg.ToString());

        AlignToCamera();
    }

    private void Update()
    {
        // Seguir al objetivo
        if (_target) transform.position = _target.position + _offset;

        // Flotar hacia arriba
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        AlignToCamera();

        // Temporizador & fade-out
        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            float t = (_timer - lifeTime) / fadeTime;
            _text.color = Color.Lerp(_startColor, Color.clear, t);
            if (t >= 1f) Destroy(gameObject);
        }
    }

    private void AlignToCamera()
    {
        if (_cam) transform.forward = _cam.forward;    // billboarding limpio
    }
}
