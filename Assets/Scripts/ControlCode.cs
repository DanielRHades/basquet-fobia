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
        // Movimiento del personaje
        if (direccionMovimiento.magnitude > umbralMovimiento)
        {
            float velocidadActual = velocidadTrotar;
            if (correrValor > 0)
            {
                velocidadActual = Mathf.Lerp(velocidadTrotar, velocidadCorrer, correrValor);
            }

            Vector3 movimiento = new Vector3(direccionMovimiento.x, 0, direccionMovimiento.y).normalized;
            transform.Translate(movimiento * velocidadActual * Time.deltaTime, Space.World);

            if (movimiento != Vector3.zero)
            {
                Quaternion rotacion = Quaternion.LookRotation(movimiento);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * suavizadoRotacion);
            }

            if (HasMoved())
            {
                ultimaPosicion = transform.position;
            }
        } // <-- Este es el corchete de cierre que falta
    } // <-- Y aquí cerramos el método Update

    private bool HasMoved()
    {
        return Vector3.Distance(transform.position, ultimaPosicion) > 0.01f;
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
        }
        else if (ctx.canceled)
        {
            correrValor = 0f;
        }
    }
}

