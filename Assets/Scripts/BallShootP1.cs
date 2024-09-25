using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasketballThrowP1 : MonoBehaviour
{
    public Transform puntoLanzamiento; // Punto de lanzamiento
    public Vector3 cesta;               // Posición de la cesta como Vector3
    public GameObject balonPref;        // Prefab del balón
    public float alturaMaxima = 5f;     // Altura máxima que debe alcanzar el balón

    void Start()
    {
        // Inicializar puntoLanzamiento si es null
        if (puntoLanzamiento == null)
        {
            puntoLanzamiento = this.transform; // Establecer a la posición del prefab
        }
    }

    void Update()
    {
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonSouth.wasPressedThisFrame) // Lanzar balón al presionar el botón sur
        {
            GameObject balon = Instantiate(balonPref, puntoLanzamiento.position, Quaternion.identity);
            LaunchBall(balon);
        }
    }

    void LaunchBall(GameObject balon)
    {
        Rigidbody rb = balon.GetComponent<Rigidbody>();

        // Calcular la distancia entre el punto de lanzamiento y la cesta
        Vector3 toCesta = cesta - puntoLanzamiento.position; // Usar Vector3 cesta directamente
        Vector3 toCestaXZ = new Vector3(toCesta.x, 0, toCesta.z);  // Proyección en el plano XZ

        // Tiempo para alcanzar la canasta, usando la fórmula del tiro parabólico
        float tiempo = Mathf.Sqrt(-2 * alturaMaxima / Physics.gravity.y) +
                       Mathf.Sqrt(2 * (toCesta.y - alturaMaxima) / Physics.gravity.y);

        // Calcular la velocidad inicial en los ejes XZ
        Vector3 velocidadXZ = toCestaXZ / tiempo;

        // Calcular la velocidad en Y (vertical) usando la altura máxima
        float velocidadY = Mathf.Sqrt(-2 * Physics.gravity.y * alturaMaxima);

        // Aplicar la velocidad calculada al balón
        Vector3 velocidadInicial = velocidadXZ + Vector3.up * velocidadY;
        rb.velocity = velocidadInicial;
    }
}
