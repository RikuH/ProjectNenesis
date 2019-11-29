using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //This is shieet mon

        Cursor.visible = false;
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Time.timeScale = 0;

            Cursor.visible = true;
            panel.SetActive(true);
        }
    }

    public void continueGame(){
        Cursor.visible = false;
        panel.SetActive(false);
        Time.timeScale = 1.0f;

    }

    public void ExitGame(){
        Application.Quit();
    }
}
