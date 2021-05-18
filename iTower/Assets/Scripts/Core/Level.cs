using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour{
    private void Awake(){
        GameSession.instance.gameSpeed=1f;
        Time.timeScale=1f;
        SetUpSingleton();
    }
    private void SetUpSingleton(){
        if(FindObjectsOfType<Level>().Length>1){Destroy(gameObject);}else{DontDestroyOnLoad(gameObject);}
    }
    void Update(){
        CheckESC();
    }

    public void LoadStartMenu(){
        SaveSerial.instance.Save();
        GameSession.instance.ResetMusicPitch();
        SceneManager.LoadScene("Menu");
        //LoadLevel("Menu");
        if(SceneManager.GetActiveScene().name=="Menu"){GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;}
    }
    public void LoadStartMenuGame(){FindObjectOfType<Level>().StartCoroutine(LoadStartMenuGameI());}
    IEnumerator LoadStartMenuGameI(){
        if(SceneManager.GetActiveScene().name=="Game"){
            //GameSession.instance.SaveHighscore();
            yield return new WaitForSecondsRealtime(0.01f);
            //GameSession.instance.ResetScore();
        }
        yield return new WaitForSecondsRealtime(0.05f);
        SaveSerial.instance.Save();
        GameSession.instance.ResetMusicPitch();
        SceneManager.LoadScene("Menu");
        //LoadLevel("Menu");
        if(SceneManager.GetActiveScene().name=="Menu"){GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;}
        
        //GameSession.instance.savableData.Save();
        //SaveSerial.instance.Save();
    }
    public void LoadGameScene(){
        SceneManager.LoadScene("Game");
        //GameSession.instance.ResetScore();
        GameSession.instance.gameSpeed=1f;
    }
    public void LoadOptionsScene(){SceneManager.LoadScene("Options");}
    public void LoadCreditsScene(){SceneManager.LoadScene("Credits");}
    public void LoadWebsite(string url){Application.OpenURL(url);}
    public void RestartGame(){FindObjectOfType<Level>().StartCoroutine(RestartGameI());}
    IEnumerator RestartGameI(){
        //GameSession.instance.SaveHighscore();
        yield return new WaitForSecondsRealtime(0.01f);
        //GameSession.instance.ResetScore();
        GameSession.instance.ResetMusicPitch();
        yield return new WaitForSecondsRealtime(0.05f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
    }
    public void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
    }
    public void QuitGame(){Application.Quit();}
    public void Restart(){
        SceneManager.LoadScene("Loading");
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
    }
    void CheckESC(){
    if(Input.GetKeyDown(KeyCode.Escape)){
            var scene=SceneManager.GetActiveScene().name;
            if(scene=="Credits"){
                LoadStartMenu();
            }else if(scene=="Options"){
                if(FindObjectOfType<SettingsMenu>()!=null){
                    if(FindObjectOfType<SettingsMenu>().transform.GetChild(1).gameObject.activeSelf==true){
                        FindObjectOfType<SettingsMenu>().OpenSettings();
                    }else{
                        LoadStartMenu(); 
                    }
                }else Debug.LogError("No SettingsMenu");
            }
    }}
}
