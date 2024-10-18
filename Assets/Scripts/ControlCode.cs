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

    public Animator animator;

    void Start()
    {
        ultimaPosicion = transform.position;
        animator = GetComponentInChildren<Animator>();

        if (animator != null)
        {
            // Listar todos los parámetros para verificar cuáles están disponibles
            foreach (var param in animator.parameters)
            {
                Debug.Log("Parámetro encontrado: " + param.name + " - Tipo: " + param.type);
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el Animator en los hijos del objeto.");
        }
    }

    void Update()
    {
        // Movimiento del personaje
        if (direccionMovimiento.magnitude > umbralMovimiento)
        {
            if (HasParameter(animator, "mover"))
            {
                animator.SetBool("mover", true);
                Debug.Log("El personaje está en movimiento.");
            }
            else
            {
                Debug.LogWarning("El parámetro 'mover' no existe en el Animator Controller.");
            }

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
        }
        else
        {
            if (HasParameter(animator, "mover"))
            {
                animator.SetBool("mover", false);
                Debug.Log("El personaje está en reposo.");
            }
            else
            {
                Debug.LogWarning("El parámetro 'mover' no existe en el Animator Controller.");
            }
        }
    }

    private bool HasMoved()
    {
        return Vector3.Distance(transform.position, ultimaPosicion) > 0.01f;
    }

    // Función auxiliar para verificar si un parámetro existe en el Animator
    private bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    // Función para cuando se mueve el personaje con el stick
    public void EnMovimiento(InputAction.CallbackContext ctx)
    {
        direccionMovimiento = ctx.ReadValue<Vector2>();
        Debug.Log("Dirección de movimiento: " + direccionMovimiento);
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
