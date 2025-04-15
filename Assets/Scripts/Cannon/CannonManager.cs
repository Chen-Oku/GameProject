using System.Collections;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    public GameObject player; // Jugador
    public Transform firePoint; // Punto de disparo del canonn
    public GameObject projectilePrefab; // Prefab del proyectil
    public float fireForce = 20f; // Fuerza del disparo
    public GameObject fireButton; // Boton en el mapa para disparar el cannon
    public float rotationStep = 30f; // Grados de rotacion por golpe

    private PlayerAttack playerAttack; // Referencia al script PlayerAttack

    /// Variables para la trayectoria del proyectil
    public LineRenderer trajectoryLine; // Componente LineRenderer para dibujar la trayectoria
    public int trajectoryPoints = 30; // Numero de puntos en la trayectoria
    public float timeStep = 0.1f; // Tiempo entre cada punto de la trayectoria

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
        if (trajectoryLine == null || trajectoryLine.gameObject == null)
            return;

        DrawTrajectory();
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

    void DrawTrajectory()
    {
        if (trajectoryLine == null || firePoint == null) return;
        if (trajectoryLine.gameObject == null) return;

        Vector3[] points = new Vector3[trajectoryPoints];
        Vector3 startPos = firePoint.position;
        Vector3 startVel = firePoint.forward * fireForce / projectilePrefab.GetComponent<Rigidbody>().mass;

        points[0] = startPos;
        int i;
        for (i = 1; i < trajectoryPoints; i++)
        {
            if (trajectoryLine == null || trajectoryLine.gameObject == null) return;

            float t = i * timeStep;
            Vector3 prevPoint = points[i - 1];
            Vector3 nextPoint = startPos + startVel * t + 0.5f * Physics.gravity * t * t;

            if (Physics.Linecast(prevPoint, nextPoint, out RaycastHit hit))
            {
                points[i] = hit.point;
                i++;
                break;
            }
            else
            {
                points[i] = nextPoint;
            }
        }

        if (trajectoryLine == null || trajectoryLine.gameObject == null) return;

        trajectoryLine.positionCount = i;
        Vector3[] visiblePoints = new Vector3[i];
        for (int j = 0; j < i; j++)
            visiblePoints[j] = points[j];

        trajectoryLine.SetPositions(visiblePoints);
    }
}





