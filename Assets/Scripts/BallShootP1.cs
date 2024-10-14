using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasketballThrowP1 : MonoBehaviour
{
    public Transform puntoLanzamiento; // Punto de lanzamiento
    public Vector3 cesta;               // Posición de la cesta como Vector3
    public GameObject balonPref;        // Prefab del balón
    public GameObject pelotaMano;       // Pelota que aparece en la mano del jugador
    public float alturaMaxima = 5f;     // Altura máxima que debe alcanzar el balón

    private bool tieneBalon = false;    // Verifica si el jugador ha recogido la pelota del centro

    void Start()
    {
        // Inicializar puntoLanzamiento si es null
        if (puntoLanzamiento == null)
        {
            puntoLanzamiento = this.transform; // Establecer a la posición del prefab
        }

        // Asegúrate de que la pelota en la mano esté oculta al inicio
        if (pelotaMano != null)
        {
            pelotaMano.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void Update()
    {
        // Intento de recogida usando botón "O" o "buttonEast" para pruebas
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonEast.wasPressedThisFrame && !tieneBalon)
        {
            Debug.Log("Intentando recoger la pelota...");
            RecogerBalon();
        }

        // Lanzamiento usando botón "X" o "buttonSouth"
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonSouth.wasPressedThisFrame && tieneBalon)
        {
            GameObject balon = Instantiate(balonPref, puntoLanzamiento.position, Quaternion.identity);
            LaunchBall(balon);
        }
    }

    void RecogerBalon()
    {
        // Buscar el balón más cercano con el tag "pelota"
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f); // 1.5f es el rango de recogida
        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("pelota"))
            {
                // Desactivar la pelota en la cancha
                col.gameObject.SetActive(false);
                tieneBalon = true;

                // Activar la visibilidad de la pelota en la mano
                if (pelotaMano != null)
                {
                    pelotaMano.SetActive(true); // Activa la pelota completa, no solo el renderer
                }
                break; // Deja de buscar después de encontrar el primer balón
            }
        }
    }




    void LaunchBall(GameObject balon)
    {
        Rigidbody rb = balon.GetComponent<Rigidbody>();

        // Calcular la distancia entre el punto de lanzamiento y la cesta
        Vector3 toCesta = cesta - puntoLanzamiento.position;
        Vector3 toCestaXZ = new Vector3(toCesta.x, 0, toCesta.z);

        // Calcula el tiempo de vuelo en función de la distancia horizontal
        float distanciaHorizontal = toCestaXZ.magnitude;
        float tiempo = Mathf.Sqrt(-2 * alturaMaxima / Physics.gravity.y) +
                   Mathf.Sqrt(2 * (toCesta.y - alturaMaxima) / Physics.gravity.y);

        // Ajustar la velocidad XZ de acuerdo con la distancia
        Vector3 velocidadXZ = toCestaXZ / tiempo;

        // Calcular la velocidad en Y (vertical) usando la altura máxima
        float velocidadY = Mathf.Sqrt(-2 * Physics.gravity.y * alturaMaxima);

        // Aplicar la velocidad calculada al balón
        Vector3 velocidadInicial = velocidadXZ + Vector3.up * velocidadY;
        rb.velocity = velocidadInicial;

        // Desactivar la pelota en la mano al lanzar
        if (pelotaMano != null)
        {
            pelotaMano.GetComponent<MeshRenderer>().enabled = false;
        }

        // Resetear la variable tieneBalon
        tieneBalon = false;
    }
}