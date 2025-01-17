﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class CoreSetup : MonoBehaviour{   public static CoreSetup instance;
    [Header("Main Managers")]
    [AssetsOnly][SerializeField] GameObject gameMangerPrefab;
    [AssetsOnly][SerializeField] GameObject gsceneManagerPrefab;
    [AssetsOnly][SerializeField] GameObject saveSerialPrefab;
    [AssetsOnly][SerializeField] GameObject easySavePrefab;
    
    [Header("Assets Managers")]
    [AssetsOnly][SerializeField] GameObject assetsManagerPrefab;
    [AssetsOnly][SerializeField] GameObject audioManagerPrefab;
    [AssetsOnly][SerializeField] GameObject jukeboxPrefab;
    
    //[Header("Networking, Advancements etc")]
    //[AssetsOnly][SerializeField] GameObject dbaccessPrefab;
    //[AssetsOnly][SerializeField] GameObject discordPresencePrefab;
    //[AssetsOnly][SerializeField] GameObject statsAchievsManagerPrefab;
    void Awake(){
        instance=this;
        if(SceneManager.GetActiveScene().name=="Loading")LoadPre();
        else Load();
    }
    void LoadPre(){
        if(FindObjectOfType<SaveSerial>()==null){Instantiate(saveSerialPrefab);}
        if(FindObjectOfType<ES3ReferenceMgr>()==null){Instantiate(easySavePrefab);}
        if(FindObjectOfType<GSceneManager>()==null){var go=Instantiate(gsceneManagerPrefab);go.GetComponent<GSceneManager>().enabled=true;}
            /*Idk it disables itself so I guess Ill turn it on manually*/
        //if(FindObjectOfType<DBAccess>()==null){Instantiate(dbaccessPrefab);}
    }
    void Load(){
        LoadPre();
        if(FindObjectOfType<GameManager>()==null){Instantiate(gameMangerPrefab);}

        if(FindObjectOfType<AssetsManager>()==null){Instantiate(assetsManagerPrefab);}
        if(FindObjectOfType<AudioManager>()==null){Instantiate(audioManagerPrefab);}

        //if(FindObjectOfType<DBAccess>()==null){Instantiate(dbaccessPrefab);}
        //if(FindObjectOfType<DiscordPresence.PresenceManager>()==null){Instantiate(discordPresencePrefab);}
        //if(FindObjectOfType<StatsAchievsManager>()==null){Instantiate(statsAchievsManagerPrefab);}
        
        if(FindObjectOfType<PostProcessVolume>()!=null&&FindObjectOfType<SaveSerial>().settingsData.pprocessing!=true){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
        //if(FindObjectOfType<EventSystem>()!=null){if(FindObjectOfType<EventSystem>().GetComponent<UIInputSystem>()==null)FindObjectOfType<EventSystem>().gameObject.AddComponent<UIInputSystem>();}
        if(FindObjectOfType<Jukebox>()==null&&SceneManager.GetActiveScene().name=="Menu"){Instantiate(jukeboxPrefab);}
        //yield return new WaitForSeconds(0.5f);
        //Destroy(gameObject);
    }

    public GameObject GetJukeboxPrefab(){return jukeboxPrefab;}
}
