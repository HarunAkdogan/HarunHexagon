using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public Text gameOverTxt;

    void Start()
    {
        if (PlayerPrefs.GetString("state") == "gameover" && PlayerPrefs.HasKey("score"))
        {
            gameOverTxt.GetComponent<Text>().text = "Your Score: " + PlayerPrefs.GetInt("score");
        }

        PlayerPrefs.SetString("state", "menu");
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);

    }

    public void FinishGame()
    {
        FindObjectOfType<GameControl>().gameOver = true;
        SceneManager.LoadScene(0);
    }

   
    public void ExitGame()
    {
        Application.Quit();
    }


}
