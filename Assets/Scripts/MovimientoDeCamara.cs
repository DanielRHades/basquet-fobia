using System.Collections;
using System.Collections.Generic;
/*
Resumen del Script de Movimiento de Cámara:

Este script controla la cámara en un juego de Unity, haciendo que siga a dos jugadores (etiquetados como "Player1" y "Player2"). 
La cámara ajusta su posición entre un punto máximo (maxPoint) y un punto medio basado en la distancia entre los jugadores.

Funcionalidades Clave:
1. **Buscar Jugadores**: El script busca a los jugadores en cada actualización mediante sus etiquetas.
2. **Calcular Posición Media**: Calcula el punto medio entre los dos jugadores.
3. **Calcular Distancia**: Determina la distancia entre los jugadores.
4. **Interpolación**: Utiliza `Mathf.InverseLerp` para ajustar la posición de la cámara entre `maxPoint` y el `midpoint` según la distancia.
5. **Movimiento Suave**: La cámara se mueve suavemente hacia su nueva posición utilizando `Vector3.Lerp`.
6. **Orientación de la Cámara**: La cámara siempre mira hacia el punto medio entre los jugadores.

Configuraciones Importantes en el Inspector:
- `minDistance`: Distancia mínima para acercar la cámara (ej. 2).
- `maxDistance`: Distancia máxima para alejar la cámara (ej. 10).
- `offset`: Offset constante desde el punto medio (ej. 5).
- No se asi funciona bien con los valores del codigo, estan como cambiados xd

Este script permite que la cámara se ajuste dinámicamente a la posición de los jugadores, manteniéndolos siempre en el encuadre adecuado.
*/

using UnityEngine;

public class MovimientoDeCamara : MonoBehaviour
{
    public Camera camera; // Asigna la cámara desde el Inspector
    public Transform maxPoint; // Objeto vacío como el punto máximo
    public float minDistance = 20f; // Distancia mínima para acercar la cámara
    public float maxDistance = 0f; // Distancia máxima para alejar la cámara
    public float offset = 0f; // Offset desde el punto medio

    private Transform player1; // Jugador 1
    private Transform player2; // Jugador 2

    void LateUpdate()
    {
        // Buscar jugadores por tag en cada actualización
        player1 = GameObject.FindGameObjectWithTag("Player1")?.transform;
        player2 = GameObject.FindGameObjectWithTag("Player2")?.transform;

        // Verificar que ambos jugadores estén activos
        if (player1 == null || player2 == null) return;

        // Calcular la posición media entre los dos jugadores
        Vector3 midpoint = (player1.position + player2.position) / 2;

        // Calcular la distancia entre los jugadores
        float distance = Vector3.Distance(player1.position, player2.position);

        // Invertir la lógica: si están cerca, queremos que la cámara esté más cerca del midpoint
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        Vector3 targetPosition = Vector3.Lerp(maxPoint.position, midpoint, t);

        // Establecer la nueva posición de la cámara usando el offset
        Vector3 newCameraPosition = targetPosition - (targetPosition - midpoint).normalized * offset;

        // Mover la cámara suavemente a la nueva posición
        camera.transform.position = Vector3.Lerp(camera.transform.position, newCameraPosition, Time.deltaTime * 5f);

        // Hacer que la cámara mire hacia el punto medio
        camera.transform.LookAt(midpoint);
    }
}
