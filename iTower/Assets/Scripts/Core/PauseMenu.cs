using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsUI;
    public float prevGameSpeed = 1f;
    IEnumerator Start(){
        yield return new WaitForSeconds(0.05f);
        Resume();
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                if(pauseMenuUI.activeSelf)Resume();
                if(optionsUI.transform.GetChild(0).gameObject.activeSelf){GameManager.instance.SaveSettings();GameManager.instance.CloseSettings(true);pauseMenuUI.SetActive(true);}
                if(optionsUI.transform.GetChild(1).gameObject.activeSelf){optionsUI.GetComponent<SettingsMenu>().OpenSettings();PauseEmpty();}
            }else{
                Pause();
            }
        }
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        if(optionsUI.transform.GetChild(0).gameObject.activeSelf){GameManager.instance.CloseSettings(false);}
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        GameManager.instance.gameSpeed=1;
        GameIsPaused=false;
    }
    public void PauseEmpty(){
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        GameIsPaused=true;
        GameManager.instance.gameSpeed=0;
    }
    public void Pause(){
        prevGameSpeed = GameManager.instance.gameSpeed;
        pauseMenuUI.SetActive(true);
        PauseEmpty();
    }
    public void Menu(){
        GameManager.instance.gameSpeed = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void OpenOptions(){
        optionsUI.GetComponent<SettingsMenu>().OpenSettings();
        pauseMenuUI.SetActive(false);
    }
    public void PreviousGameSpeed(){GameManager.instance.gameSpeed = prevGameSpeed;}
}