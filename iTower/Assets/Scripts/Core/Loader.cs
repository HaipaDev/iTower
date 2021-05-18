using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Loader : MonoBehaviour{
    public float timer=1f;
    public AudioMixer audioMixer;
    bool loaded;
    private void Load(){
        if(!loaded){
        SaveSerial.instance.Load();
        SaveSerial.instance.LoadSettings();
        loaded=true;
        }
        Screen.fullScreen = SaveSerial.instance.settingsData.fullscreen;if(SaveSerial.instance.settingsData.fullscreen)Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,true,60);
        QualitySettings.SetQualityLevel(SaveSerial.instance.settingsData.quality);
        audioMixer.SetFloat("MasterVolume", SaveSerial.instance.settingsData.masterVolume);
        audioMixer.SetFloat("SoundVolume", SaveSerial.instance.settingsData.soundVolume);
        audioMixer.SetFloat("MusicVolume", SaveSerial.instance.settingsData.musicVolume);
    }
    void Update(){
        Load();
        timer-=Time.deltaTime;
        if(timer<=0){if(SceneManager.GetActiveScene().name=="Loading"){SceneManager.LoadScene("Menu");}Destroy(gameObject);}
    }
}
