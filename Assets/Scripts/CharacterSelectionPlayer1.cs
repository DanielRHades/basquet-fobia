using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionPlayer1 : MonoBehaviour
{
    public GameObject[] characters; // Array de prefabs del jugador 1
    public Transform spawnPoint; // Posición de visualización
    public Button nextButton; // Botón para seleccionar el siguiente personaje
    public Button previousButton; // Botón para seleccionar el anterior personaje

    private int selectedCharacter = 0; // Índice del personaje seleccionado

    private void Start()
    {
        UpdateCharacter();

        // Configuración de eventos para los botones
        nextButton.onClick.AddListener(NextCharacter);
        previousButton.onClick.AddListener(PreviousCharacter);
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        UpdateCharacter();
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        UpdateCharacter();
    }

    private void UpdateCharacter()
    {
        foreach (var character in characters)
        {
            character.SetActive(false);
        }
        characters[selectedCharacter].SetActive(true);
        characters[selectedCharacter].transform.position = spawnPoint.position;
        characters[selectedCharacter].transform.rotation = spawnPoint.rotation;
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter1", selectedCharacter); // Guardar personaje del jugador 1
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
