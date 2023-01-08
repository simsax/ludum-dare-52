using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ButtonHover()
    {
        FindObjectOfType<AudioManager>().Play("hover");
    }

    public void ButtonPress()
    {
        FindObjectOfType<AudioManager>().Play("press");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
