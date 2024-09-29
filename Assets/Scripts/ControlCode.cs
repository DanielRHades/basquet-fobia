using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCode : MonoBehaviour
{
    private Vector3 ultimaPosicion;

    // Variables para las velocidades de trotar y correr
    public float velocidadTrotar = 100f;
    public float velocidadCorrer = 300f;

    // Variable para capturar el valor del gatillo (correr)
    float correrValor = 0f;

    // Variable para capturar el valor de movimiento del stick
    Vector2 direccionMovimiento;

    // Velocidad de suavización de rotación
    public float suavizadoRotacion = 5f;

    // Umbral para considerar movimiento
    public float umbralMovimiento = 0.1f;

    private void Start()
    {
        ultimaPosicion = transform.position;
    }

    void Update()
    {
        // Si se está moviendo con el stick (dirección de movimiento distinta de cero con un umbral)
        if (direccionMovimiento.magnitude > umbralMovimiento)
        {
            // Determina la velocidad dependiendo de si el gatillo está presionado o no
            float velocidadActual = velocidadTrotar;

            if (correrValor > 0)
            {
                velocidadActual = Mathf.Lerp(velocidadTrotar, velocidadCorrer, correrValor);
            }

            // Calcula la dirección del movimiento en el espacio 3D con normalización para un movimiento suave
            Vector3 movimiento = new Vector3(direccionMovimiento.x, 0, direccionMovimiento.y).normalized;

            // Mueve el objeto según la dirección y la velocidad actual
            transform.Translate(movimiento * velocidadActual * Time.deltaTime, Space.World);

            // Rotar al personaje en la dirección del movimiento
            if (movimiento != Vector3.zero)
            {
                Quaternion rotacion = Quaternion.LookRotation(movimiento);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * suavizadoRotacion);
            }

            if (HasMoved())
            {
                Debug.Log("Posición X: " + transform.position.x + ", Posición Z: " + transform.position.z);
                ultimaPosicion = transform.position;
            }
        }
    }

    private bool HasMoved()
    {
        return Vector3.Distance(transform.position, ultimaPosicion) > 0.01f; // Ajusta el umbral según sea necesario
    }

    // Función para cuando se mueve el personaje con el stick
    public void EnMovimiento(InputAction.CallbackContext ctx)
    {
        direccionMovimiento = ctx.ReadValue<Vector2>();
    }

    // Función para cuando se presiona el gatillo (Correr)
    public void EnCorrer(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            correrValor = ctx.ReadValue<float>();
            Debug.Log("Gatillo presionado, valor: " + correrValor);
        }
        else if (ctx.canceled)
        {
            correrValor = 0f;
            Debug.Log("Gatillo soltado, valor: " + correrValor);
        }
    }
}
