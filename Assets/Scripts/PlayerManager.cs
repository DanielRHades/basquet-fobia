using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] characterPrefabsP1; // Array de prefabs para el jugador 1
    public GameObject[] characterPrefabsP2; // Array de prefabs para el jugador 2
    private void Start()
    {
        
        PlayerInput.Instantiate(characterPrefabsP1[PlayerPrefs.GetInt("selectedCharacter1")], 0, "Gamepad", 0, Gamepad.all[0]);
        
        PlayerInput.Instantiate(characterPrefabsP2[PlayerPrefs.GetInt("selectedCharacter2")], 1, "Gamepad", 0, Gamepad.all[1]);
        
    }
}
