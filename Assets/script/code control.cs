using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class codecontrol : MonoBehaviour
{
    Vector2 Movimiento;

    void Update()
    {
        // Mueve el personaje en los ejes X y Z
        transform.Translate(Movimiento.x * Time.deltaTime, 0, Movimiento.y * Time.deltaTime);

        // Muestra la posición actual en los ejes X y Z en la consola
        Debug.Log("Posición X: " + transform.position.x + ", Posición Z: " + transform.position.z);
    }

    public void EnMovimiento(InputAction.CallbackContext ctx) => Movimiento = ctx.ReadValue<Vector2>();

    public void Aumentar(InputAction.CallbackContext ctx)
    {
        transform.localScale += Vector3.one / 10;
    }
}

