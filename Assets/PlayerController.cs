using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject gun;
    
    

    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float verticalLimit = 80f; 

    public Transform cameraPivot; 
    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (controladorPowerUps.Instance.GetComponent<controladorPowerUps>().descargaInsaciableIsStored)
        {
            ActivateDescargaInsaciable();
            Debug.Log("funcooooooooooo");
        }
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * v + transform.right * h;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLimit, verticalLimit);
        cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);


        //esto es para probar algo, borrar despues
        if (Input.GetKey("p"))
        {
            SceneManager.LoadScene("prueba 1");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;

        if(other.gameObject.tag == "DescargaInsaciable")
        {
            Destroy(other.gameObject);
            controladorPowerUps.Instance.agarroDI();
            ActivateDescargaInsaciable();
            

        }
    }

    


    #region DescargaInsaciable
    private void ActivateDescargaInsaciable()
    {
        DescargaInsaciable();
        Invoke("StopDescargaInsaciable", 15.0f);
    }
    private void DescargaInsaciable()
    {
        gun.GetComponent<gun>().timeBetweenShooting = gun.GetComponent<gun>().timeBetweenShooting / 1.3f;
        Debug.Log(gun.GetComponent<gun>().timeBetweenShooting);
    }
    private void StopDescargaInsaciable()
    {
        gun.GetComponent<gun>().timeBetweenShooting = gun.GetComponent<gun>().timeBetweenShooting * 1.3f;
        Debug.Log(gun.GetComponent<gun>().timeBetweenShooting);
    }

    #endregion
}
