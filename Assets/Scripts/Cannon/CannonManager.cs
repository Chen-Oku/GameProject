using System.Collections;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    public Camera cannonCamera; // Cámara del cañón
    public GameObject player; // Jugador
    public Transform firePoint; // Punto de disparo del cañón
    public GameObject projectilePrefab; // Prefab del proyectil
    public float fireForce = 20f; // Fuerza del disparo

    private Camera playerCamera; // Cámara del jugador
    private bool isControllingCannon = false;

    void Start()
    {
        playerCamera = Camera.main; // Asignar la cámara principal del jugador
        cannonCamera.gameObject.SetActive(false); // Desactivar la cámara del cañón al inicio
    }

    void Update()
    {
        if (isControllingCannon)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                FireCannon();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                EnterCannon();
                print("Enter Cannon");
            }
        }
    }

    void EnterCannon()
    {
        isControllingCannon = true;
        player.SetActive(false); // Desactivar el jugador
        playerCamera.gameObject.SetActive(false); // Desactivar la cámara del jugador
        cannonCamera.gameObject.SetActive(true); // Activar la cámara del cañón
    }

    void FireCannon()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);
    }

    public void ExitCannon()
    {
        isControllingCannon = false;
        player.SetActive(true); // Activar el jugador
        playerCamera.gameObject.SetActive(true); // Activar la cámara del jugador
        cannonCamera.gameObject.SetActive(false); // Desactivar la cámara del cañón
    }
}



