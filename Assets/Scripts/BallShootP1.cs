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
    public ControlCode control;          // Referencia al script ControlCode

    void Start()
    {
        if (puntoLanzamiento == null)
        {
            puntoLanzamiento = this.transform; // Establecer a la posición del prefab
        }
    }

    void Update()
    {
        if (Gamepad.all.Count > 0 && Gamepad.all[0].buttonSouth.wasPressedThisFrame)
        {
            GameObject balon = Instantiate(balonPref, puntoLanzamiento.position, Quaternion.identity);
            LaunchBall(balon);
        }
    }

    void LaunchBall(GameObject balon)
    {
        Rigidbody rb = balon.GetComponent<Rigidbody>();

        Vector3 toCesta = cesta - puntoLanzamiento.position;
        Vector3 toCestaXZ = new Vector3(toCesta.x, 0, toCesta.z);

        float tiempo = Mathf.Sqrt(-2 * alturaMaxima / Physics.gravity.y) +
                       Mathf.Sqrt(2 * (toCesta.y - alturaMaxima) / Physics.gravity.y);

        Vector3 velocidadXZ = toCestaXZ / tiempo;
        float velocidadY = Mathf.Sqrt(-2 * Physics.gravity.y * alturaMaxima);

        Vector3 velocidadInicial = velocidadXZ + Vector3.up * velocidadY;
        rb.velocity = velocidadInicial;

        // Llamar al método para lanzar en el ControlCode
        if (control != null)
        {
            control.LanzarBalon();
        }
    }
}
