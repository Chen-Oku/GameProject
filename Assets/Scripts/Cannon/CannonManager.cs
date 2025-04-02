using System.Collections;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    public GameObject player; // Jugador
    public Transform firePoint; // Punto de disparo del ca��n
    public GameObject projectilePrefab; // Prefab del proyectil
    public float fireForce = 20f; // Fuerza del disparo
    public GameObject fireButton; // Bot�n en el mapa para disparar el ca��n
    public float rotationStep = 30f; // Grados de rotaci�n por golpe

    private PlayerAttack playerAttack; // Referencia al script PlayerAttack

    void Start()
    {
        playerAttack = player.GetComponent<PlayerAttack>();
        if (playerAttack == null)
        {
            Debug.LogError("No se encontr� el componente PlayerAttack en " + player.name);
        }
    }

    void Update()
    {
        // No es necesario actualizar nada en cada frame en este caso
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            playerAttack.PerformAttack();
            RotateCannon();
            print("CannonManager: Cannon rotated");
        }
    }

    void RotateCannon()
    {
        transform.Rotate(0, rotationStep, 0); // Rotar el ca��n 30 grados en el eje Y
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == fireButton && Input.GetKeyDown(KeyCode.Space))
        {
            FireCannon();
        }
    }

    public void FireCannon()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);
    }
}





