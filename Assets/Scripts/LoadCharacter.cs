using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabsP1; // Array de prefabs para el jugador 1
    public GameObject[] characterPrefabsP2; // Array de prefabs para el jugador 2
    public Transform spawnPointP1; // Posición de spawn para el jugador 1
    public Transform spawnPointP2; // Posición de spawn para el jugador 2

    void Start()
    {
        // Cargar el personaje seleccionado del jugador 1
        int selectedCharacterP1 = PlayerPrefs.GetInt("selectedCharacter1");
        if (selectedCharacterP1 >= 0 && selectedCharacterP1 < characterPrefabsP1.Length)
        {
            Instantiate(characterPrefabsP1[selectedCharacterP1], spawnPointP1.position, Quaternion.identity);
        }

        // Cargar el personaje seleccionado del jugador 2
        int selectedCharacterP2 = PlayerPrefs.GetInt("selectedCharacter2");
        if (selectedCharacterP2 >= 0 && selectedCharacterP2 < characterPrefabsP2.Length)
        {
            Instantiate(characterPrefabsP2[selectedCharacterP2], spawnPointP2.position, Quaternion.identity);
        }
    }
}
