using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour{
    [SerializeField] GameObject[] panels;
    [SerializeField]GameObject qualityDropdopwn;
    [SerializeField]GameObject fullscreenToggle;
    [SerializeField]GameObject pprocessingToggle;
    [SerializeField]GameObject cheatToggle;
    [SerializeField]GameObject masterSlider;
    [SerializeField]GameObject soundSlider;
    [SerializeField]GameObject musicSlider;
    [SerializeField]AudioSource audioSource;
    public AudioMixer audioMixer;
    [SerializeField]GameObject pprocessingPrefab;
    public PostProcessVolume postProcessVolume;
    private void Start(){
        if(audioSource==null)audioSource=GetComponent<AudioSource>();

        if(SaveSerial.instance!=null){
            qualityDropdopwn.GetComponent<Dropdown>().value = SaveSerial.instance.settingsData.quality;
            fullscreenToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.settingsData.fullscreen;
            pprocessingToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.settingsData.pprocessing;
            cheatToggle.GetComponent<Toggle>().isOn = GameSession.instance.cheatmode;

            masterSlider.GetComponent<Slider>().value = SaveSerial.instance.settingsData.masterVolume;
            soundSlider.GetComponent<Slider>().value = SaveSerial.instance.settingsData.soundVolume;
            musicSlider.GetComponent<Slider>().value = SaveSerial.instance.settingsData.musicVolume;
        }
        if(SceneManager.GetActiveScene().name=="Options")OpenSettings();
    }
    private void Update(){
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance!=null)if(SaveSerial.instance.settingsData.pprocessing==true&&postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
        if(SaveSerial.instance!=null)if(SaveSerial.instance.settingsData.pprocessing==true&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume.enabled=true;}
        if(SaveSerial.instance!=null)if(SaveSerial.instance.settingsData.pprocessing==false&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
        if(SaveSerial.instance.settingsData.masterVolume<=-40){SaveSerial.instance.settingsData.masterVolume=-80;}
        if(SaveSerial.instance.settingsData.soundVolume<=-40){SaveSerial.instance.settingsData.soundVolume=-80;}
        if(SaveSerial.instance.settingsData.musicVolume<=-40){SaveSerial.instance.settingsData.musicVolume=-80;}
    }
    public void SetPanelActive(int i){
        foreach(GameObject p in panels){p.SetActive(false);}panels[i].SetActive(true);
    }
    public void OpenSettings(){transform.GetChild(0).gameObject.SetActive(true);transform.GetChild(1).gameObject.SetActive(false);}
    public void OpenDeleteAll(){transform.GetChild(1).gameObject.SetActive(true);transform.GetChild(0).gameObject.SetActive(false);}
    public void Close(){transform.GetChild(0).gameObject.SetActive(false);transform.GetChild(1).gameObject.SetActive(false);}
    public void SetMasterVolume(float volume){
    if(GameSession.instance!=null){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.masterVolume = volume;
    }}public void SetSoundVolume(float volume){
    if(GameSession.instance!=null){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.soundVolume = volume;
    }}
    public void SetMusicVolume(float volume){
    if(GameSession.instance!=null){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.musicVolume = volume;
    }}
    public void SetQuality(int qualityIndex){
    if(GameSession.instance!=null){
        QualitySettings.SetQualityLevel(qualityIndex);
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.quality = qualityIndex;
    }}
    public void SetFullscreen (bool isFullscreen){
    if(GameSession.instance!=null){
        Screen.fullScreen = isFullscreen;
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.fullscreen = isFullscreen;
        Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,isFullscreen,60);
    }}
    public void SetPostProcessing (bool isPostprocessed){
    if(GameSession.instance!=null){
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance!=null)if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.pprocessing = isPostprocessed;
        if(isPostprocessed==true && postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}//FindObjectOfType<Level>().RestartScene();}
        if(isPostprocessed==true && postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(isPostprocessed==false && FindObjectOfType<PostProcessVolume>()!=null){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }}
    public void SetCheatmode(bool isCheatmode){
        if(GameSession.instance!=null)GameSession.instance.cheatmode=isCheatmode;
    }
}
