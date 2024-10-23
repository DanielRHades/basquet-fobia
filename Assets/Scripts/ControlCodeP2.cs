using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCodeP2 : MonoBehaviour
{
    private Vector3 ultimaPosicionP2;

    // Variables para las velocidades de trotar y correr
    public float velocidadTrotarP2 = 100f;
    public float velocidadCorrerP2 = 300f;

    // Variable para capturar el valor del gatillo (correr)
    float correrValorP2 = 0f;

    // Variable para capturar el valor de movimiento del stick
    Vector2 direccionMovimientoP2;

    // Velocidad de suavización de rotación
    public float suavizadoRotacionP2 = 5f;

    // Umbral para considerar movimiento
    public float umbralMovimientoP2 = 0.1f;

    public Animator animatorP2;
    public Animator bAnimatorP2;  // Asegúrate de asignar esto desde el inspector

    void Start()
    {
        ultimaPosicionP2 = transform.position;

        if (animatorP2 == null)
        {
            Debug.LogWarning("Animator no asignado en el inspector.");
        }
        else
        {
            // Listar todos los parámetros para verificar cuáles están disponibles
            foreach (var param in animatorP2.parameters)
            {
                Debug.Log("Parámetro encontrado: " + param.name + " - Tipo: " + param.type);
            }
        }
    }

    void Update()
    {
        // Movimiento del personaje
        if (direccionMovimientoP2.magnitude > umbralMovimientoP2)
        {
            float velocidadActual = velocidadTrotarP2;
            if (correrValorP2 > 0)
            {
                velocidadActual = Mathf.Lerp(velocidadTrotarP2, velocidadCorrerP2, correrValorP2);
            }

            Vector3 movimiento = new Vector3(direccionMovimientoP2.x, 0, direccionMovimientoP2.y).normalized;
            transform.Translate(movimiento * velocidadActual * Time.deltaTime, Space.World);

            if (movimiento != Vector3.zero)
            {
                Quaternion rotacion = Quaternion.LookRotation(movimiento);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * suavizadoRotacionP2);
            }

            AnimatotionPlay();
            ultimaPosicionP2 = transform.position;
        }
        else
        {
            // Si no se mueve, actualizar el estado
            AnimatotionPlay();
        }
    }

    // Función para cuando se mueve el personaje con el stick
    public void EnMovimiento(InputAction.CallbackContext ctx)
    {
        direccionMovimientoP2 = ctx.ReadValue<Vector2>();
    }

    // Función para cuando se presiona el gatillo (Correr)
    public void EnCorrer(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            correrValorP2 = ctx.ReadValue<float>();
        }
        else if (ctx.canceled)
        {
            correrValorP2 = 0f;
        }
    }

    public void AnimatotionPlay()
    {
        if (animatorP2 != null)
        {
            // Cambia el parámetro según el movimiento
            animatorP2.SetBool("IsMovingP2", direccionMovimientoP2.magnitude > umbralMovimientoP2);
        }

        if (bAnimatorP2 != null)
        {
            // Cambia el parámetro según el movimiento
            bAnimatorP2.SetBool("IsMovingP2", direccionMovimientoP2.magnitude > umbralMovimientoP2);
        }
    }

    public void CambiarEstadoBalon(bool tieneBalon)
    {
        if (animatorP2 != null)
        {
            animatorP2.SetBool("TieneBalonP2", tieneBalon);
            Debug.Log("Estado de balón actualizado: " + tieneBalon);
        }

        if (bAnimatorP2 != null)
        {
            bAnimatorP2.SetBool("TieneBalonP2", tieneBalon);
            Debug.Log("Estado de balón actualizado: " + tieneBalon);
        }
    }

    public void LanzarBalon()
    {
        if (animatorP2 != null)
        {
            animatorP2.SetTrigger("Lanzar"); // Asegúrate de que este trigger esté definido en tu Animator
        }
    }
}
