using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballThrow : MonoBehaviour
{
    public Transform puntolanzamiento; // Punto de lanzamiento
    public Transform cesta;            // Punto donde está la cesta
    public GameObject balonpref;       // Prefab del balón
    public float alturaMaxima = 5f;    // Altura máxima que debe alcanzar el balón

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Lanzar balón al presionar espacio
        {
            GameObject balon = Instantiate(balonpref, puntolanzamiento.position, Quaternion.identity);
            LaunchBall(balon);
        }
    }

    void LaunchBall(GameObject balon)
    {
        Rigidbody rb = balon.GetComponent<Rigidbody>();

        // Calcular la distancia entre el punto de lanzamiento y la cesta
        Vector3 toCesta = cesta.position - puntolanzamiento.position;
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