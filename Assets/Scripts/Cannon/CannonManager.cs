using System.Collections;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    public Camera cannonCamera; // C�mara del ca��n
    public GameObject player; // Jugador
    public Transform firePoint; // Punto de disparo del ca��n
    public GameObject projectilePrefab; // Prefab del proyectil
    public float fireForce = 20f; // Fuerza del disparo

    private Camera playerCamera; // C�mara del jugador
    private bool isControllingCannon = false;

    void Start()
    {
        playerCamera = Camera.main; // Asignar la c�mara principal del jugador
        cannonCamera.gameObject.SetActive(false); // Desactivar la c�mara del ca��n al inicio
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
        playerCamera.gameObject.SetActive(false); // Desactivar la c�mara del jugador
        cannonCamera.gameObject.SetActive(true); // Activar la c�mara del ca��n
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
        playerCamera.gameObject.SetActive(true); // Activar la c�mara del jugador
        cannonCamera.gameObject.SetActive(false); // Desactivar la c�mara del ca��n
    }
}



