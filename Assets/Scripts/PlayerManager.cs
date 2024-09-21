using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] characterPrefabsP1; // Array de prefabs para el jugador 1
    public GameObject[] characterPrefabsP2; // Array de prefabs para el jugador 2
    private bool player1Joined = false;
    private bool player2Joined = false;

    private void Update()
    {
        // Esperar a que el jugador 1 se una
        if (!player1Joined && Gamepad.all.Count > 0 && Gamepad.all[0].startButton.wasPressedThisFrame) // Usa el botón Start
        {
            player1Joined = true; // Marca que el jugador 1 se ha unido
            PlayerInput.Instantiate(characterPrefabsP1[PlayerPrefs.GetInt("selectedCharacter1")], 0, "Gamepad", 0, Gamepad.all[0]);
        }

        // Esperar a que el jugador 2 se una
        if (!player2Joined && Gamepad.all.Count > 1 && Gamepad.all[1].startButton.wasPressedThisFrame) // Usa el botón Start
        {
            player2Joined = true; // Marca que el jugador 2 se ha unido
            PlayerInput.Instantiate(characterPrefabsP2[PlayerPrefs.GetInt("selectedCharacter2")], 1, "Gamepad", 0, Gamepad.all[1]);
        }
    }
}
