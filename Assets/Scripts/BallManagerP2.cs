using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallManagerP2 : MonoBehaviour
{
    public Transform puntoLanzamientoP2; // Punto de lanzamiento para el segundo jugador
    public Vector3 cestaP2;               // Posición de la cesta para el segundo jugador
    public float alturaMaximaP2 = 5f;     // Altura máxima que debe alcanzar el balón
    private GameObject balonCancha;       // Referencia al balón de la cancha
    private bool tieneBalonP2 = false;    // Verifica si el segundo jugador ha recogido la pelota del centro

    void Start()
    {
        // Inicializar puntoLanzamientoP2 si es null
        if (puntoLanzamientoP2 == null)
        {
            puntoLanzamientoP2 = this.transform; // Establecer a la posición del prefab
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
        if (Gamepad.all.Count > 0 && Gamepad.all[1].buttonEast.wasPressedThisFrame && !tieneBalonP2)
        {
            Debug.Log("Jugador 2 intentando recoger la pelota...");
            RecogerBalonP2();
        }

        // Lanzar balón con el botón "X" o "buttonSouth"
        if (Gamepad.all.Count > 0 && Gamepad.all[1].buttonSouth.wasPressedThisFrame && tieneBalonP2)
        {
            LanzarBalonP2();
            DesactivarBalonManoP2();
        }
    }

    void RecogerBalonP2()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f); // Rango de recogida
        foreach (Collider col in colliders)
        {
            if (col.gameObject == balonCancha)
            {
                balonCancha.SetActive(false); // Desactivar el balón de la cancha
                tieneBalonP2 = true; // Indicar que el jugador ahora tiene el balón

                // Activar balón en la mano del jugador
                foreach (Transform child in transform)
                {
                    if (child.CompareTag("BalonManoP2"))
                    {
                        child.gameObject.SetActive(true); // Activar el balón en la mano
                    }
                }
            }
        }
    }

    void LanzarBalonP2()
    {
        // Colocar el balón de la cancha 2 unidades más lejos en la dirección de frente del jugador
        Vector3 lanzamientoPosicion = puntoLanzamientoP2.position + puntoLanzamientoP2.forward * 2; // Aumentar 2 unidades
        balonCancha.SetActive(true);
        balonCancha.transform.position = lanzamientoPosicion;

        Rigidbody rb = balonCancha.GetComponent<Rigidbody>();

        // Calcular la trayectoria del lanzamiento
        Vector3 toCesta = cestaP2 - lanzamientoPosicion; // Usar la nueva posición de lanzamiento
        Vector3 toCestaXZ = new Vector3(toCesta.x, 0, toCesta.z);
        float distanciaHorizontal = toCestaXZ.magnitude;
        float tiempo = Mathf.Sqrt(-2 * alturaMaximaP2 / Physics.gravity.y) +
                       Mathf.Sqrt(2 * (toCesta.y - alturaMaximaP2) / Physics.gravity.y);
        Vector3 velocidadXZ = toCestaXZ / tiempo;
        float velocidadY = Mathf.Sqrt(-2 * Physics.gravity.y * alturaMaximaP2);
        Vector3 velocidadInicial = velocidadXZ + Vector3.up * velocidadY;
        rb.velocity = velocidadInicial;

        tieneBalonP2 = false; // Resetear el estado de tener balón
    }

    void DesactivarBalonManoP2()
    {
        // Desactivar el balón en la mano del jugador
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BalonManoP2"))
            {
                child.gameObject.SetActive(false); // Desactivar el balón en la mano
            }
        }
    }
}
