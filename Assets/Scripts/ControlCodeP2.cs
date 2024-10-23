using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCodeP2 : MonoBehaviour
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

    public Animator P2Animator;
    public Animator B2Animator;  // Asegúrate de asignar esto desde el inspector

    void Start()
    {
        ultimaPosicion = transform.position;

        if (P2Animator == null)
        {
            Debug.LogWarning("Animator no asignado en el inspector.");
        }
        else
        {
            // Listar todos los parámetros para verificar cuáles están disponibles
            foreach (var param in P2Animator.parameters)
            {
                Debug.Log("Parámetro encontrado: " + param.name + " - Tipo: " + param.type);
            }
        }
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

            AnimatotionPlay();
            ultimaPosicion = transform.position;
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

    public void AnimatotionPlay()
    {
        if (P2Animator != null)
        {
            // Cambia el parámetro según el movimiento
            P2Animator.SetBool("IsMovingP2", direccionMovimiento.magnitude > umbralMovimiento);
            
        }

        if (B2Animator != null)
        {
            // Cambia el parámetro según el movimiento
            B2Animator.SetBool("IsMovingP2", direccionMovimiento.magnitude > umbralMovimiento);
            
        }
    }
    public void CambiarEstadoBalon(bool tieneBalon)
    {
        if (P2Animator != null)
        {
            P2Animator.SetBool("TieneBalonP2", tieneBalon);
            Debug.Log("Estado de balón actualizado: " + tieneBalon);
        }

        if (B2Animator != null)
        {
            B2Animator.SetBool("TieneBalonP2", tieneBalon);
            Debug.Log("Estado de balón actualizado: " + tieneBalon);
        }
    }

    public void LanzarBalon()
    {
        if (P2Animator != null)
        {
            P2Animator.SetTrigger("Lanzar"); // Asegúrate de que este trigger esté definido en tu Animator
        }
    }
}
