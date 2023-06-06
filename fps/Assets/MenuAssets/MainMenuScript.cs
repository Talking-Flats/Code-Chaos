using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject settingsMenuUI;
    public GameObject mainMenuUI;
    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadSetting(){
        mainMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void СloseSetting(){
        mainMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }

    public void BeginGame(){
        Debug.Log("Нажал на вход");
         Application.LoadLevel("SampleScene");
    }


    public void QuitGame(){
        Debug.Log("Нажал на выход");
        Application.Quit();
    }
}


