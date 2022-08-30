using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;

public class SaveSerial : MonoBehaviour{	public static SaveSerial instance;
	void Awake(){if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
	//[SerializeField] string filenameLogin = "hyperGamerLogin";
	[SerializeField] string filename = "playerData";
	//[SerializeField] string filenameStats = "statsData";
	[SerializeField] string filenameSettings = "gameSettings";
	public static int maxRegisteredHyperGamers=3;

/*#region//HyperGamerLogin
	public HyperGamerLoginData hyperGamerLoginData=new HyperGamerLoginData();
	[System.Serializable]public class HyperGamerLoginData{
		public int registeredCount;
		public bool loggedIn;
		public string username;
		public string password;
		public DateTime lastLoggedIn;
	}
	public string _loginDataPath(){return Application.persistentDataPath+"/"+filenameLogin+".hyper";}
	public void SetLogin(string username, string password){
		hyperGamerLoginData.loggedIn=true;
		hyperGamerLoginData.username=username;
		hyperGamerLoginData.password=password;
		hyperGamerLoginData.lastLoggedIn=DateTime.Now;
		Debug.Log("Login data set");
	}
	public void LogOut(){
		hyperGamerLoginData.loggedIn=false;
		hyperGamerLoginData.username="";
		hyperGamerLoginData.password="";
		Debug.Log("Logged out");
	}
	public void SaveLogin(){
		var settings=new ES3Settings(_loginDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
		ES3.Save("hyperGamerLoginData",hyperGamerLoginData,settings);
		Debug.Log("Login saved");
	}
	public void LoadLogin(){
		if(ES3.FileExists(_loginDataPath())){
			var settings=new ES3Settings(_loginDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
			if(ES3.KeyExists("hyperGamerLogin",settings))ES3.LoadInto<HyperGamerLoginData>("hyperGamerLoginData",settings);
		}else Debug.LogWarning("Login Data file not found in "+Application.persistentDataPath+"/"+filenameLogin);
	}
	void AutoLogin(){
		TryLogin(hyperGamerLoginData.username,hyperGamerLoginData.password);
	}
	public void TryLogin(string username, string password){
		//try{
			if(DBAccess.instance!=null){if(hyperGamerLoginData.username!="")DBAccess.instance.LoginHyperGamer(hyperGamerLoginData.username,hyperGamerLoginData.password);}
			else{Debug.Log("No DBAccess, cant try to login");}
		//}catch{}
	}
#endregion*/
#region//Player Data
	public PlayerData playerData=new PlayerData();
	public float buildFirstLoaded;
	public float buildLastLoaded;
	[System.Serializable]public class PlayerData{
		//public AchievData[] achievsCompleted=new AchievData[0];
	}

	public string _playerDataPath(){return Application.persistentDataPath+"/"+filename+".hyper";}
	public void Save(){
        var settings=new ES3Settings(_playerDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
		if(!ES3.KeyExists("buildFirstLoaded",settings))ES3.Save("buildFirstLoaded",GameSession.instance.buildVersion,settings);
		ES3.Save("buildLastLoaded",GameSession.instance.buildVersion,settings);
		ES3.Save("playerData",playerData,settings);
		Debug.Log("Game Data saved");
	}
	public void Load(){
		if(ES3.FileExists(_playerDataPath())){
			var settings=new ES3Settings(_playerDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
			if(ES3.KeyExists("buildFirstLoaded",settings))buildFirstLoaded=ES3.Load<float>("buildFirstLoaded",settings);
			else Debug.LogWarning("Key for buildFirstLoaded not found in: "+_playerDataPath());
			if(ES3.KeyExists("buildLastLoaded",settings))buildLastLoaded=ES3.Load<float>("buildLastLoaded",settings);
			else Debug.LogWarning("Key for buildLastLoaded not found in: "+_playerDataPath());
			if(ES3.KeyExists("playerData",settings))ES3.LoadInto<PlayerData>("playerData",playerData,settings);
			else Debug.LogWarning("Key for playerData not found in: "+_playerDataPath());
			//var hi=-1;foreach(int h in playerData.highscore){hi++;if(h!=0)playerData.highscore[hi]=h;}
			Debug.Log("Game Data loaded");
		}else Debug.LogWarning("Game Data file not found in: "+_playerDataPath());
	}
	public void Delete(){
		//playerData=new PlayerData(){highscore=new Highscore[GameCreator.GetGamerulesetsPrefabsLength()]/*,achievsCompleted=new AchievData[StatsAchievsManager._AchievsListCount()]*/};
		GC.Collect();
		if(ES3.FileExists(_playerDataPath())){
			ES3.DeleteFile(_playerDataPath());
			Debug.Log("Game Data deleted");
		}
	}
#endregion
/*#region//Stats Data
	public StatsData statsData=new StatsData(){statsGamemodesList=new StatsGamemode[StatsAchievsManager.GetStatsGMListCount()]};
	[System.Serializable]public class StatsData{
		public StatsGamemode[] statsGamemodesList=new StatsGamemode[0];
		public float sandboxTime=0;
		public List<string> uniquePowerups;
	}

	public string _statsDataPath(){return Application.persistentDataPath+"/"+filenameStats+".hyper";}
	public void SaveStats(){
		var settings=new ES3Settings(_statsDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
		ES3.Save("statsData",statsData,settings);
		Debug.Log("Stats Data saved");
	}
	public void LoadStats(){
		if(ES3.FileExists(_statsDataPath())){
			var settings=new ES3Settings(_statsDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
			if(ES3.KeyExists("statsData",settings))ES3.LoadInto<StatsData>("statsData",statsData,settings);
			else Debug.LogWarning("Key for statsData not found in: "+_statsDataPath());
		}else Debug.LogWarning("Stats Data file not found in: "+_statsDataPath());
	}
	public void DeleteStats(){
		statsData=new StatsData(){statsGamemodesList=new StatsGamemode[StatsAchievsManager.GetStatsGMListCount()]};
		GC.Collect();
		if(ES3.FileExists(_statsDataPath())){
			ES3.DeleteFile(_statsDataPath());
			Debug.Log("Stats Data deleted");
		}
	}
#endregion*/
#region//Settings Data
	public SettingsData settingsData=new SettingsData();
	[System.Serializable]public class SettingsData{
		//public InputType inputType;
		//public JoystickType joystickType;
		//public float joystickSize=1;
		public bool lefthand;
		public bool dtapMouseShoot;
		public bool scbuttons;
		public bool vibrations=true;
		public bool discordRPC=true;
		public bool autosubmitScores=true;
		
		public float masterVolume=0.95f;
		public float soundVolume=0.95f;
		public float ambienceVolume=-0.55f;
		public float musicVolume=0.66f;
		public bool windDownMusic=true;
		
		public int quality=4;
		public bool fullscreen=true;
		public bool pprocessing;
		public bool screenshake=true;
		public bool dmgPopups=true;
		public bool particles=true;		
		public bool screenflash=true;
	}
	
	public string _settingsDataPath(){return Application.persistentDataPath+"/"+filenameSettings+".json";}
	public void SaveSettings(){
		var settings=new ES3Settings(_settingsDataPath());
		ES3.Save("settingsData",settingsData,settings);
		Debug.Log("Settings saved");
	}
	public void LoadSettings(){
		if(ES3.FileExists(_settingsDataPath())){
		var settings=new ES3Settings(_settingsDataPath());
			if(ES3.KeyExists("settingsData",settings))ES3.LoadInto<SettingsData>("settingsData",settingsData,settings);
			else Debug.LogWarning("Key for settingsData not found in: "+_settingsDataPath());
		}else Debug.LogWarning("Settings file not found in: "+_settingsDataPath());
	}
	public void ResetSettings(){
		settingsData=new SettingsData();
		GC.Collect();
		if(ES3.FileExists(_settingsDataPath())){
			ES3.DeleteFile(_settingsDataPath());
		}
	}
#endregion
}

[System.Serializable]
public class Highscore{
	public int score;
	public float playtime;
	public string version;
	public float build;
	public DateTime date;
}
[System.Serializable]
public class LockboxCount{public string name;public int count;}