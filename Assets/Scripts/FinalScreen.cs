using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScreen : MonoBehaviour
{
    public string mainMenuScene;

    public TextMeshProUGUI killedEnemiesText;

    public static FinalScreen instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FinalScreen.instance.killedEnemiesText.text = "KILLED ENEMIES: " + 0;
    }

    // Update is called once per frame
    void Update()
    {
        FinalScreen.instance.killedEnemiesText.text = "KILLED ENEMIES: " + GameManager.instance.killedEnemies;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
