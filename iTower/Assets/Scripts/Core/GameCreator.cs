using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;

public class GameCreator : MonoBehaviour{
    public static GameCreator instance;
    [SerializeField] GameObject gameSessionPrefab;
    [SerializeField] GameObject loaderPrefab;
    [SerializeField] GameObject saveSerialPrefab;
    [SerializeField] GameObject gameAssetsPrefab;
    [SerializeField] GameObject levelPrefab;
    [SerializeField] GameObject audioManagerPrefab;
    private void Awake(){
        instance=this;
        Load();
    }
    void Load(){
        if(FindObjectOfType<GameSession>()==null){Instantiate(gameSessionPrefab);}
        if(FindObjectOfType<SaveSerial>()==null){Instantiate(saveSerialPrefab);}
        if(FindObjectOfType<GameAssets>()==null){Instantiate(gameAssetsPrefab);}
        if(FindObjectOfType<Level>()==null){Instantiate(levelPrefab);}
        if(FindObjectOfType<AudioManager>()==null){Instantiate(audioManagerPrefab);}
        if(FindObjectOfType<PostProcessVolume>()!=null&& FindObjectOfType<SaveSerial>().settingsData.pprocessing!=true){FindObjectOfType<PostProcessVolume>().enabled=false;}

    }
}
