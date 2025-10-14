using UnityEngine;
using Unity.Services.Analytics;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Look")]
    public float mouseSensitivity = 2f;
    public float verticalLimit = 80f;
    public Transform cameraPivot;

    private CharacterController _cc;
    private float _pitch;


    public DungeonGenerator dungeonGenerator; // arrastrar en el inspector
    public GameObject player; // arrastrar el player aquí

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        if (cameraPivot == null)
        {
            var cam = Camera.main;
            if (cam) cameraPivot = cam.transform;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        Look();
        Move();
    }

    private void Look()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0f, mx, 0f);

        _pitch = Mathf.Clamp(_pitch - my, -verticalLimit, verticalLimit);
        if (cameraPivot) cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0f, v);
        input = Vector3.ClampMagnitude(input, 1f);

        Vector3 world = transform.TransformDirection(input);
        _cc.SimpleMove(world * moveSpeed);
    }


}
