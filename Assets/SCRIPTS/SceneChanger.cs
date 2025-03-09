using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton.onClick.AddListener(() => ChangeScene("Analytics Scene"));
    }

    void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
