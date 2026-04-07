using UnityEngine;
using UnityEngine.SceneManagement; // necessário para mudar de cena

public class ChangeScene : MonoBehaviour
{
    [Header("Nome da cena que será carregada")]
    public string sceneName;

    // Função chamada pelo botão
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Nenhuma cena atribuída no botão!");
        }
    }
}