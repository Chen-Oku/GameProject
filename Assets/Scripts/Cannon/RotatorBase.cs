/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorBase : MonoBehaviour
{
    public float torqueAmount = 10f; // Amount of torque to apply
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontró el componente Rigidbody en " + gameObject.name);
        }
    }

    void FixedUpdate()
    {
        // Apply torque to rotate the base around its own Y axis
        rb.AddTorque(Vector3.up * torqueAmount);
    }
}*/
using Unity.VisualScripting;
using UnityEngine;

public class RotatingBase : MonoBehaviour
{
    public float rotationSpeed = 50f; // Velocidad de giro

  /*  void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga la etiqueta "Player"
        {
            float direction = other.transform.position.x > transform.position.x ? 1f : -1f;
            transform.Rotate(Vector3.up, direction * rotationSpeed * Time.deltaTime);
        }
    }*/
    private void Update()
    {
        //OnCollisionEnter();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float direction = collision.transform.position.x > transform.position.x ? 1f : -1f;
            transform.Rotate(Vector3.up, direction * rotationSpeed * Time.deltaTime);
        }
    }
}