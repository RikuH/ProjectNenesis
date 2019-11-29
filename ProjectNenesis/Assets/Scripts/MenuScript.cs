using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject infoPanel;
    public TextMeshProUGUI getName;
    public string experimentsName = "";
    public string gamingLevel = "";
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1680, 1050, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void novice(){
        gamingLevel = "Novice";
    }
    public void beginner(){
        gamingLevel = "Beginner";
    }
    public void casual(){
        gamingLevel = "Casual";
    }
    public void trueGamer(){
        gamingLevel = "True Gamer";
    }
    public void hardCore(){
        gamingLevel = "HardCore";
    }


    public void startGame(){
        //Save all the infos
        experimentsName = getName.text;
        DontDestroyOnLoad(this.gameObject);
        Debug.Log(gamingLevel + " " + experimentsName);

        infoPanel.SetActive(false);

        //Change scene to game scene
        SceneManager.LoadScene("GameScene");
    }
}
