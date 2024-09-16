using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;
    public Transform spawnPoint; // Asume que tienes un Transform para definir la posición de visualización

    private void Start()
    {
        UpdateCharacter();
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
        characters[selectedCharacter].transform.position = spawnPoint.position; // Coloca el personaje en el punto de spawn
        characters[selectedCharacter].transform.rotation = spawnPoint.rotation; // Asegúrate de mantener la rotación
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
