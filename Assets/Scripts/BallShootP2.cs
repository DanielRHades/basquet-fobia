using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasketballThrowP2 : MonoBehaviour
{
    public Transform puntoLanzamientoP2; // Punto de lanzamiento para el segundo jugador
    public Vector3 cestaP2;               // Posición de la cesta para el segundo jugador
    public GameObject balonPrefP2;        // Prefab del balón para el segundo jugador
    public GameObject pelotaManoP2;       // Pelota que aparece en la mano del segundo jugador
    public float alturaMaximaP2 = 5f;     // Altura máxima que debe alcanzar el balón

    private bool tieneBalonP2 = false;    // Verifica si el segundo jugador ha recogido la pelota del centro

    void Start()
    {
        // Inicializar puntoLanzamientoP2 si es null
        if (puntoLanzamientoP2 == null)
        {
            puntoLanzamientoP2 = this.transform; // Establecer a la posición del prefab
        }

        // Asegúrate de que la pelota en la mano esté oculta al inicio
        if (pelotaManoP2 != null)
        {
            pelotaManoP2.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void Update()
    {
        // Intento de recogida usando botón "O" o "buttonEast" para pruebas
        if (Gamepad.all.Count > 0 && Gamepad.all[1].buttonEast.wasPressedThisFrame && !tieneBalonP2)
        {
            Debug.Log("Jugador 2 intentando recoger la pelota...");
            RecogerBalonP2();
        }

        // Lanzamiento usando botón "X" o "buttonSouth"
        if (Gamepad.all.Count > 0 && Gamepad.all[1].buttonSouth.wasPressedThisFrame && tieneBalonP2)
        {
            GameObject balon = Instantiate(balonPrefP2, puntoLanzamientoP2.position, Quaternion.identity);
            LaunchBallP2(balon);
        }
    }

    void RecogerBalonP2()
    {
        // Buscar el balón más cercano con el tag "pelota"
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f); // 1.5f es el rango de recogida
        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("pelota"))
            {
                // Desactivar la pelota en la cancha
                col.gameObject.SetActive(false);
                tieneBalonP2 = true;

                // Activar la visibilidad de la pelota en la mano
                if (pelotaManoP2 != null)
                {
                    pelotaManoP2.SetActive(true);
                }

                break; // Deja de buscar después de encontrar el primer balón
            }
        }
    }

    void LaunchBallP2(GameObject balon)
    {
        Rigidbody rb = balon.GetComponent<Rigidbody>();

        // Calcular la distancia entre el punto de lanzamiento y la cesta
        Vector3 toCesta = cestaP2 - puntoLanzamientoP2.position;
        Vector3 toCestaXZ = new Vector3(toCesta.x, 0, toCesta.z);

        // Calcula el tiempo de vuelo en función de la distancia horizontal
        float distanciaHorizontal = toCestaXZ.magnitude;
        float tiempo = Mathf.Sqrt(-2 * alturaMaximaP2 / Physics.gravity.y) +
                   Mathf.Sqrt(2 * (toCesta.y - alturaMaximaP2) / Physics.gravity.y);

        // Ajustar la velocidad XZ de acuerdo con la distancia
        Vector3 velocidadXZ = toCestaXZ / tiempo;

        // Calcular la velocidad en Y (vertical) usando la altura máxima
        float velocidadY = Mathf.Sqrt(-2 * Physics.gravity.y * alturaMaximaP2);

        // Aplicar la velocidad calculada al balón
        Vector3 velocidadInicial = velocidadXZ + Vector3.up * velocidadY;
        rb.velocity = velocidadInicial;

        // Desactivar la pelota en la mano al lanzar
        if (pelotaManoP2 != null)
        {
            pelotaManoP2.GetComponent<MeshRenderer>().enabled = false;
        }

        // Resetear la variable tieneBalonP2
        tieneBalonP2 = false;
    }
}

