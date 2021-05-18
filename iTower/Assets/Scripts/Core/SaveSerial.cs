using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Encoders;
using BayatGames.SaveGameFree.Serializers;

public class SaveSerial : MonoBehaviour{
	public static SaveSerial instance;
	[SerializeField] string filename = "playerData";
	bool dataEncode=true;
	[SerializeField] string filenameSettings = "gameSettings.cfg";
	bool settingsEncode=false;
#region//Player Data
	public PlayerData playerData=new PlayerData();
	[System.Serializable]public class PlayerData{
		
	}
	public void Save(){
		SaveGame.Encode = dataEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filename, playerData);
		Debug.Log("Game Data saved");
	}
	public void Load(){
		if (File.Exists(Application.persistentDataPath + "/"+filename)){
			SaveGame.Encode = dataEncode;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			playerData = SaveGame.Load<PlayerData>(filename);

			Debug.Log("Game Data loaded");
		}else Debug.Log("Game Data file not found in "+Application.persistentDataPath+"/"+filename);
	}
	public void Delete(){
		playerData=new PlayerData();
		GC.Collect();
		if (File.Exists(Application.persistentDataPath + "/"+filename)){
			File.Delete(Application.persistentDataPath + "/"+filename);
			Debug.Log("Game Data deleted");
		}else Debug.Log("Game Data file not found in "+Application.persistentDataPath+"/"+filename);
	}
#endregion
#region//Settings Data
	public SettingsData settingsData=new SettingsData();
	[System.Serializable]public class SettingsData{
		public string gameVersion="0.0.1";
		public bool fullscreen=true;
		public bool pprocessing;
		public bool scbuttons;
		public int quality=4;
		public float masterVolume=0;
		public float soundVolume=0;
		public float musicVolume=-25;
	}
	
	public void SaveSettings(){
		SaveGame.Encode = settingsEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filenameSettings, settingsData);
		Debug.Log("Settings saved");
	}
	public void LoadSettings(){
		if (File.Exists(Application.persistentDataPath + "/"+filenameSettings)){
			SettingsData data = new SettingsData();
			SaveGame.Encode = settingsEncode;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			settingsData = SaveGame.Load<SettingsData>(filenameSettings);
			Debug.Log("Settings loaded");
		}
		else Debug.Log("Settings file not found in " + Application.persistentDataPath + "/" + filenameSettings);
	}
	public void ResetSettings(){
		settingsData=new SettingsData();
		GC.Collect();
		if (File.Exists(Application.persistentDataPath + "/"+filenameSettings)){
			File.Delete(Application.persistentDataPath + "/"+filenameSettings);
			Debug.Log("Settings Data deleted");
		}else Debug.Log("Settings file not found in "+Application.persistentDataPath+"/"+filenameSettings);
	}
#endregion
	private void Awake(){
		SetUpSingleton();
		instance=this;
	}
	private void SetUpSingleton(){
		int numberOfObj=FindObjectsOfType<SaveSerial>().Length;
		if(numberOfObj>1){Destroy(gameObject);}else{DontDestroyOnLoad(gameObject);}
	}
}