using UnityEngine;
using UnityEngine.InputSystem;

public class BallManagerP2 : MonoBehaviour
{
    public Transform puntoLanzamientoP2; // Punto de lanzamiento para el segundo jugador
    public Vector3 cestaP2;               // Posición de la cesta para el segundo jugador
    public float alturaMaximaP2 = 5f;     // Altura máxima que debe alcanzar el balón
    private GameObject balonCancha;       // Referencia al balón de la cancha
    public bool tieneBalonP2 = false;
    private ControlCodeP2 controlCode;

    void Start()
    {
        // Inicializar puntoLanzamientoP2 si es null
        if (puntoLanzamientoP2 == null)
        {
            puntoLanzamientoP2 = this.transform; // Establecer a la posición del prefab
        }

        // Buscar el BalonCancha por su tag
        balonCancha = GameObject.FindGameObjectWithTag("BalonCancha");

        // Asegúrate de que el balonCancha esté activo al inicio
        if (balonCancha != null)
        {
            balonCancha.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se encontró el BalonCancha con el tag especificado.");
        }

        // Obtener la referencia al ControlCode en el mismo GameObject
        controlCode = GetComponent<ControlCodeP2>();
    }

    void Update()
    {
        // Recoger balón con el botón "O" o "buttonEast"
        if (Gamepad.all.Count > 0 && Gamepad.all[1].buttonEast.wasPressedThisFrame && !tieneBalonP2)
        {
            Debug.Log("Jugador 2 intentando recoger la pelota...");
            RecogerBalonP2();
        }

        // Lanzar balón con el botón "X" o "buttonSouth"
        if (Gamepad.all.Count > 0 && Gamepad.all[1].buttonSouth.wasPressedThisFrame && tieneBalonP2)
        {
            LanzarBalonP2();
            DesactivarBalonManoP2();
        }

        // Robo de balón con el botón "Y" o "buttonNorth" para el Jugador 2
        if (Gamepad.all.Count > 0 && Gamepad.all[1].buttonNorth.wasPressedThisFrame && !tieneBalonP2)
        {
            // Detectar colisiones cercanas
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (Collider col in colliders)
            {
                // Si el objeto tiene el componente BallManagerP1 (para el Jugador 1)
                BallManagerP1 jugador2 = col.GetComponent<BallManagerP1>();
                if (jugador2 != null && !tieneBalonP2)
                {
                    RobarBalonP2(jugador2.gameObject);
                }
            }

        }
    }

    void RecogerBalonP2()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f); // Rango de recogida
        foreach (Collider col in colliders)
        {
            if (col.gameObject == balonCancha)
            {
                balonCancha.SetActive(false); // Desactivar el balón de la cancha
                tieneBalonP2 = true; // Indicar que el jugador ahora tiene el balón

                // Activar balón en la mano del jugador
                foreach (Transform child in transform)
                {
                    if (child.CompareTag("BalonManoP2"))
                    {
                        child.gameObject.SetActive(true); // Activar el balón en la mano
                    }
                }
            }
        }
    }

    void LanzarBalonP2()
    {
        // Colocar el balón de la cancha 2 unidades más lejos en la dirección de frente del jugador
        Vector3 lanzamientoPosicion = puntoLanzamientoP2.position + puntoLanzamientoP2.forward * 2; // Aumentar 2 unidades
        balonCancha.SetActive(true);
        balonCancha.transform.position = lanzamientoPosicion;

        Rigidbody rb = balonCancha.GetComponent<Rigidbody>();

        // Calcular la trayectoria del lanzamiento
        Vector3 toCesta = cestaP2 - lanzamientoPosicion; // Usar la nueva posición de lanzamiento
        Vector3 toCestaXZ = new Vector3(toCesta.x, 0, toCesta.z);
        float distanciaHorizontal = toCestaXZ.magnitude;
        float tiempo = Mathf.Sqrt(-2 * alturaMaximaP2 / Physics.gravity.y) +
                       Mathf.Sqrt(2 * (toCesta.y - alturaMaximaP2) / Physics.gravity.y);
        Vector3 velocidadXZ = toCestaXZ / tiempo;
        float velocidadY = Mathf.Sqrt(-2 * Physics.gravity.y * alturaMaximaP2);
        Vector3 velocidadInicial = velocidadXZ + Vector3.up * velocidadY;
        rb.velocity = velocidadInicial;

        tieneBalonP2 = false; // Resetear el estado de tener balón
    }

    public void DesactivarBalonManoP2()
    {
        // Desactivar el balón en la mano del jugador
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BalonManoP2"))
            {
                child.gameObject.SetActive(false); // Desactivar el balón en la mano
            }
        }
    }

    public void RobarBalonP2(GameObject oponente)
    {
        // Verificar si el oponente (Jugador 1) tiene el balón
        BallManagerP1 managerOponente = oponente.GetComponent<BallManagerP1>();
        if (managerOponente != null && managerOponente.tieneBalon)
        {
            // Robar el balón
            managerOponente.DesactivarBalonMano(); // Desactivar el balón en el jugador 1
            managerOponente.tieneBalon = false; // El jugador 1 ya no tiene el balón

            tieneBalonP2 = true; // Indicar que el jugador ahora tiene el balón

            // Activar balón en la mano del jugador
            foreach (Transform child in transform)
            {
                if (child.CompareTag("BalonManoP2"))
                {
                    child.gameObject.SetActive(true); // Activar el balón en la mano
                }
            }

            Debug.Log("Jugador 2 ha robado el balón del Jugador 1.");
        }
    }

}
