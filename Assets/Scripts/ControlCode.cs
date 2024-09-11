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

   private void Start()
    {
        ultimaPosicion = transform.position;
    }

    /*private void Update()
    {
        transform.Translate(Movimiento.x * Time.deltaTime, 0, Movimiento.y * Time.deltaTime);
        if (HasMoved())
        {
        Debug.Log("Posición X: " + transform.position.x + ", Posición Z: " + transform.position.z);
        ultimaPosicion = transform.position;
        }
    }*/

        void Update()
    {
        // Si se está moviendo con el stick (dirección de movimiento distinta de cero)
        if (direccionMovimiento != Vector2.zero)
        {
            // Determina la velocidad dependiendo de si el gatillo está presionado o no
            float velocidadActual = velocidadTrotar;
            // Si el gatillo está siendo presionado, ajusta la velocidad a correr tomando en cuenta el valor del gatillo
            if (correrValor > 0)
            {
                velocidadActual = Mathf.Lerp(velocidadTrotar, velocidadCorrer, correrValor);
            }
            // Mueve al personaje según la dirección y la velocidad actual
            //Vector3 movimiento = new Vector3(direccionMovimiento.x, 0, direccionMovimiento.y);
            transform.Translate(direccionMovimiento.x * velocidadActual * Time.deltaTime, 0, direccionMovimiento.y * velocidadActual * Time.deltaTime);

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
        //Debug.Log("Moviendo con el stick: " + direccionMovimiento);
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

