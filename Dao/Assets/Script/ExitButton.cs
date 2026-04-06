using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public int sceneIndex;

    public void OnClickGoToScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}