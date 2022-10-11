using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GSceneManager : MonoBehaviour{ public static GSceneManager instance;
    /*ParticleSystem transition;
    Animator transitioner;
    float transitionTime=0.35f;*/
    //float prevGameSpeed;
    void OnDisable(){Debug.LogWarning("GSceneManager disabled?");}
    void Awake(){if(GSceneManager.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
    void Start(){
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //prevGameSpeed = GameManager.instance.gameSpeed;
    }
    void Update(){
        CheckESC();
        if(SceneManager.GetActiveScene().name=="Game"&&Input.GetKeyDown(KeyCode.R)){ReloadScene();}
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //transitioner=FindObjectOfType<Tag_Transition>().GetComponent<Animator>();
    }
    public void LoadStartMenuLoader(){SceneManager.LoadScene("Menu");Instantiate(CoreSetup.instance.GetJukeboxPrefab());}
    public void LoadStartMenu(){
        SaveSerial.instance.Save();
        /*SaveSerial.instance.SaveLogin();
        if(StatsAchievsManager.instance!=null){
            StatsAchievsManager.instance.SaveStats();
            SaveSerial.instance.SaveStats();
        }*/
        SceneManager.LoadScene("Menu");
        GameManager.instance.ResetMusicPitch();if(SceneManager.GetActiveScene().name=="Menu")GameManager.instance.speedChanged=false;GameManager.instance.gameSpeed=1f;
        Resources.UnloadUnusedAssets();
    }
    public void LoadStartMenuGame(){GSceneManager.instance.StartCoroutine(LoadStartMenuGameI());}
    IEnumerator LoadStartMenuGameI(){
        if(SceneManager.GetActiveScene().name=="Game"){
            //GameManager.instance.SaveHighscore();
            yield return new WaitForSecondsRealtime(0.01f);
            //GameManager.instance.ResetScore();
        }
        yield return new WaitForSecondsRealtime(0.05f);
        SaveSerial.instance.Save();
        //SaveSerial.instance.SaveLogin();
        //StatsAchievsManager.instance.SaveStats();
        //SaveSerial.instance.SaveStats();
        GameManager.instance.ResetMusicPitch();
        yield return new WaitForSecondsRealtime(0.01f);
        GameManager.instance.speedChanged=false;GameManager.instance.defaultGameSpeed=1f;GameManager.instance.gameSpeed=1f;
        Resources.UnloadUnusedAssets();
        /*GameManager.instance.SetGamemodeSelected(0);*/
    }
    public void RestartGame(){GSceneManager.instance.StartCoroutine(GSceneManager.instance.RestartGameI());}
    IEnumerator RestartGameI(){
        //GameManager.instance.SaveHighscore();
        //if(GameManager.instance.CheckGamemodeSelected("Adventure"))GameManager.instance.SaveAdventure();//not sure if Restart should save or not
        yield return new WaitForSecondsRealtime(0.01f);
        //spawnReqsMono.RestartAllValues();
        //spawnReqsMono.ResetSpawnReqsList();
        //GameManager.instance.ResetScore();
        GameManager.instance.ResetMusicPitch();
        yield return new WaitForSecondsRealtime(0.05f);
        ReloadScene();
        //GameManager.instance.EnterGameScene();
        //GameRules.instance.EnterGameScene();
    }
    public void LoadGameScene(){
        SceneManager.LoadScene("Game");//GameManager.instance.ResetScore();
        GameManager.instance.gameSpeed=1f;
        //GameManager.instance.EnterGameScene();
        //GameRules.instance.EnterGameScene();
    }
    public void LoadOptionsScene(){SceneManager.LoadScene("Options");}
    /*public void LoadSocialsScene(){SceneManager.LoadScene("Socials");}
    public void LoadLoginScene(){SceneManager.LoadScene("Login");}
    public void LoadLeaderboardsScene(){SceneManager.LoadScene("Leaderboards");}
    public void LoadScoreUsersDataScene(){SceneManager.LoadScene("ScoreUsersData");}
    public void LoadAchievementsScene(){SceneManager.LoadScene("Achievements");}
    public void LoadStatsSocialScene(){SceneManager.LoadScene("StatsSocial");}
    public void LoadScoreSubmitScene(){SceneManager.LoadScene("ScoreSubmit");}*/
    public void LoadCreditsScene(){SceneManager.LoadScene("Credits");}
    public void LoadWebsite(string url){Application.OpenURL(url);}
    public void ReloadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.speedChanged=false;
        GameManager.instance.gameSpeed=1f;
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void RestartApp(){
        if(Jukebox.instance!=null)Destroy(Jukebox.instance.gameObject);
        SceneManager.LoadScene("Loading");
        GameManager.instance.speedChanged=false;
        GameManager.instance.gameSpeed=1f;
    }
    public static bool EscPressed(){return Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Joystick1Button1);}
    void CheckESC(){    if(EscPressed()){
            var scene=SceneManager.GetActiveScene().name;
            if(scene=="Credits"/*||scene=="Socials"*/){LoadStartMenu();}
            //else if(scene=="Achievements"||scene=="StatsSocial"){LoadSocialsScene();}
    }}

    /*void LoadLevel(string sceneName){
        //StartCoroutine(LoadTransition(sceneName));
        LoadTransition(sceneName);
    }
    void LoadTransition(string sceneName){
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        transitioner=FindObjectOfType<Tag_Transition>().GetComponent<Animator>();
        
        //transition.Play();
        transitioner.SetTrigger("Start");

        //yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }*/
}
