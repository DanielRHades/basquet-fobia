using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallManagerP1 : MonoBehaviour
{
    public Transform puntoLanzamiento;    // Punto de lanzamiento
    public Vector3 cesta;                 // Posición de la cesta
    public float alturaMaxima = 5f;       // Altura máxima
    private GameObject balonCancha;       // Referencia al balón de la cancha
    private bool tieneBalon = false;      // Si el jugador tiene un balón en la mano

    void Start()
    {
        if (puntoLanzamiento == null)
        {
            puntoLanzamiento = this.transform; // Asignar punto de lanzamiento si es null
        }

        // Buscar el BalonCancha por su tag
        balonCancha = GameObject.FindGameObjectWithTag("BalonCancha");

        // Asegúrate de que el balonCancha esté activo al inicio
        if (balonCancha != null)
        {
            balonCancha.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se encontró el BalonCancha con el tag especificado.");
        }
    }

    void Update()
    {
        // Recoger balón con el botón "O" o "buttonEast"
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonEast.wasPressedThisFrame && !tieneBalon)
        {
            Debug.Log("Intentando recoger la pelota...");
            RecogerBalon();
        }

        // Lanzar balón con el botón "X" o "buttonSouth"
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonSouth.wasPressedThisFrame && tieneBalon)
        {
            LanzarBalon();
            DesactivarBalonMano();
        }
    }

    void RecogerBalon()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f); // Rango de recogida
        foreach (Collider col in colliders)
        {
            if (col.gameObject == balonCancha)
            {
                balonCancha.SetActive(false); // Desactivar el balón de la cancha
                tieneBalon = true; // Indicar que el jugador ahora tiene el balón

                // Activar balón en la mano del jugador
                foreach (Transform child in transform)
                    {
                        if (child.CompareTag("BalonManoP1"))
                        {
                            child.gameObject.SetActive(true); // Activar el balón en la mano
                        }
                    }
            }
        }

    }

    void LanzarBalon()
    {
        // Colocar el balón de la cancha 2 unidades más lejos en la dirección de frente del jugador
        Vector3 lanzamientoPosicion = puntoLanzamiento.position + puntoLanzamiento.forward * 2; // Aumentar 2 unidades
        balonCancha.SetActive(true);
        balonCancha.transform.position = lanzamientoPosicion;

        Rigidbody rb = balonCancha.GetComponent<Rigidbody>();

        // Calcular la trayectoria del lanzamiento
        Vector3 toCesta = cesta - lanzamientoPosicion; // Usar la nueva posición de lanzamiento
        Vector3 toCestaXZ = new Vector3(toCesta.x, 0, toCesta.z);
        float distanciaHorizontal = toCestaXZ.magnitude;
        float tiempo = Mathf.Sqrt(-2 * alturaMaxima / Physics.gravity.y) +
                       Mathf.Sqrt(2 * (toCesta.y - alturaMaxima) / Physics.gravity.y);
        Vector3 velocidadXZ = toCestaXZ / tiempo;
        float velocidadY = Mathf.Sqrt(-2 * Physics.gravity.y * alturaMaxima);
        Vector3 velocidadInicial = velocidadXZ + Vector3.up * velocidadY;
        rb.velocity = velocidadInicial;

        tieneBalon = false; // Resetear el estado de tener balón
    }

    void DesactivarBalonMano()
    {
        // Desactivar el balón en la mano del jugador
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BalonManoP1"))
            {
                child.gameObject.SetActive(false); // Desactivar el balón en la mano
            }
        }
    }
}
