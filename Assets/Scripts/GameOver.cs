using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI display;

    public void Setup(int score)
    {
        FindObjectOfType<AudioManager>().Play("gameOver");
        gameObject.SetActive(true);
        display.SetText("Best harvest: " + score);
    }

    public void TryAgain()
    {
        //FindObjectOfType<Game>().endGame = false;
        SceneManager.LoadScene("Game");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ButtonHover()
    {
        FindObjectOfType<AudioManager>().Play("hover");
    }

    public void ButtonPress()
    {
        FindObjectOfType<AudioManager>().Play("press");
    }

}
