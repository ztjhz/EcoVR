using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour, IPointerEnterHandler
{
    public float delay = 1.0f; // Delay for scene transition

    // Scene transition on click
    public void GoToScene(string sceneName)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName, delay));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("You have quit the game!");
    }

    // Play sound on hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }
    }
}
