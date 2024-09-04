using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCode : MonoBehaviour
{
    Vector2 Movimiento;
    private Vector3 ultimaPosicion;

   private void Start()
    {
        ultimaPosicion = transform.position;
    }
    private void Update()
    {
        transform.Translate(Movimiento.x * Time.deltaTime, 0, Movimiento.y * Time.deltaTime);
        if (HasMoved())
        {
        Debug.Log("Posición X: " + transform.position.x + ", Posición Z: " + transform.position.z);
        ultimaPosicion = transform.position;
        }
    }
    
    private bool HasMoved()
    {
        return Vector3.Distance(transform.position, ultimaPosicion) > 0.01f; // Ajusta el umbral según sea necesario
    }

    public void EnMovimiento(InputAction.CallbackContext ctx) => Movimiento = ctx.ReadValue<Vector2>();

    public void Aumentar(InputAction.CallbackContext ctx)
    {
        transform.localScale += Vector3.one / 10;
    }
}

