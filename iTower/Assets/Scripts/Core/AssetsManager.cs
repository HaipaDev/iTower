using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AssetsManager : MonoBehaviour{	public static AssetsManager instance;
	[Header("Main")]
	[AssetsOnly,Searchable]public List<GObject> objects;
	[AssetsOnly,Searchable]public List<GObject> vfx;
	[AssetsOnly,Searchable]public List<GSprite> sprites;
	[AssetsOnly,Searchable]public List<GMaterial> materials;
    
    void Awake(){if(instance!=null){Destroy(gameObject);}else{DontDestroyOnLoad(gameObject);instance=this;}}
	void Start(){}

#region//Main
    public GameObject Make(string str, Vector2 pos){
		GObject o=objects.Find(x=>x.name==str);
		if(o==null){
			Debug.LogWarning("Object: " + str + " not found!");
			return null;
		}
        GameObject objref=Instantiate(o.gobj,pos,Quaternion.identity);
        return objref;
	}
    public GameObject MakeSpread(string str, Vector2 pos, int amnt, float rangeX=0.5f, float rangeY=0.5f){
		GObject o=objects.Find(x=>x.name==str);
		if(o==null){
			Debug.LogWarning("Object: " + str + " not found!");
			return null;
		}
		GameObject objref=Instantiate(o.gobj,pos,Quaternion.identity);
		for(var i=1;i<amnt-1;i++){
		var rndX=UnityEngine.Random.Range(-rangeX,rangeX);
		var rndY=UnityEngine.Random.Range(-rangeY,rangeY);
		var poss=pos+new Vector2(rndX,rndY);
        Instantiate(o.gobj,poss,Quaternion.identity);
		}
        return objref;
	}
    public GameObject VFX(string str, Vector2 pos, float duration=0){
		GObject o=vfx.Find(x=>x.name==str);
		if(o==null){
			Debug.LogWarning("Object: " + str + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
        GameObject objref;
		if(SaveSerial.instance.settingsData.particles){
			objref=Instantiate(gobj,pos,Quaternion.identity);
			if(duration!=0)Destroy(objref,duration);
			return objref;
		}else return null;
	}
    public GameObject Get(string str,bool ignoreWarning=false){
		GObject o=objects.Find(x=>x.name==str);
		if(o==null){
			if(!String.IsNullOrEmpty(str)&&!ignoreWarning)Debug.LogWarning("Object: " + str + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
        return gobj;
	}
	public Sprite GetObjSpr(string str,bool ignoreWarning=false){
		GObject o=objects.Find(x=>x.name==str);
		if(o==null){
			if(!String.IsNullOrEmpty(str)&&!ignoreWarning)Debug.LogWarning("Object: " + str + " not found!");
			return null;
		}
		Sprite spr=null;
		if(o.gobj.GetComponent<SpriteRenderer>()!=null)spr=o.gobj.GetComponent<SpriteRenderer>().sprite;
		if(o.gobj.GetComponent<UnityEngine.UI.Image>()!=null)spr=o.gobj.GetComponent<UnityEngine.UI.Image>().sprite;
        return spr;
	}
	public GameObject GetVFX(string str){
		GObject o=vfx.Find(x=>x.name==str);
		if(o==null){
			if(!String.IsNullOrEmpty(str))Debug.LogWarning("VFX: " + str + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
		return gobj;
        //if(SaveSerial.instance.settingsData.particles)return gobj; else return null;
	}
    public Sprite Spr(string str,bool ignoreWarning=false){
		GSprite gs=sprites.Find(x=>x.name==str);
		if(gs==null){
			if(!String.IsNullOrEmpty(name)&&!ignoreWarning)Debug.LogWarning("Sprite: " + str + " not found!");
			return null;
		}
		Sprite s=gs.spr;
        return s;
	}
    public Sprite SprAny(string str){Sprite _spr;
		_spr=Spr(str,true);
		if(_spr==null)_spr=GetObjSpr(str,true);
		if(_spr==null)Debug.LogWarning("Sprite not found in the library of sprites nor for the object by name: "+str);
        return _spr;
	}

	public Material GetMat(string mat,bool instantiate=false){
		GMaterial gm=materials.Find(x=>x.name==mat);
		if(gm==null){
			if(!String.IsNullOrEmpty(mat))Debug.LogWarning("Material: " + mat + " not found!");
			return null;
		}
		Material m=gm.mat;
		if(instantiate){m=Instantiate(m);}
        return m;
	}
	public Material UpdateShaderMatProps(Material mat,ShaderMatProps shaderMatProps,bool isUI=false){	Material _mat=mat;
		if(_mat!=null&&!_mat.shader.name.Contains("AllIn1SpriteShader")){
        	if(!isUI){if(AssetsManager.instance.GetMat("AIOShaderMat")!=null)_mat=Instantiate(AssetsManager.instance.GetMat("AIOShaderMat"));}
			else{if(AssetsManager.instance.GetMat("AIOShaderMat_UI")!=null)_mat=Instantiate(AssetsManager.instance.GetMat("AIOShaderMat_UI"));}
		}
		_mat.SetTexture("_MainTex",shaderMatProps.text);
		_mat.SetFloat("_HsvShift",shaderMatProps.hue*360);
        _mat.SetFloat("_HsvSaturation",shaderMatProps.saturation*2);
        _mat.SetFloat("_HsvBright",shaderMatProps.value*2);
        _mat.SetFloat("_NegativeAmount",shaderMatProps.negative);
        _mat.SetFloat("_PixelateSize",Mathf.Clamp(shaderMatProps.pixelate*512,4,512));
        _mat.SetFloat("_BlurIntensity",shaderMatProps.blur*100);
        _mat.SetFloat("_BlurHD",Convert.ToSingle(shaderMatProps.lowResBlur));
		return _mat;
	}
#endregion

#region//Public functions
	public static bool CaseInsStrCmpr(string str,string toComp){return str.IndexOf(toComp, StringComparison.OrdinalIgnoreCase) >= 0;}
	public static int BoolToInt(bool b){if(b){return 1;}else{return 0;}}
	public void TransformIntoUIParticle(GameObject go,float mult=0,float dur=-4,bool multShape=false,int type=0){
		if(go.GetComponent<UnityEngine.UI.Extensions.UIParticleSystem>()==null){
			var ps=go.GetComponent<ParticleSystem>();var psMain=ps.main;
			if(mult==0){
				if(ps.startSize<=1){mult=100;}
				if(ps.startSize<=10&&ps.startSize>1){mult=10;}
			}
			if(dur>0){Destroy(go,dur);}
			else if(dur==0){Destroy(go,psMain.startLifetime.constantMax+psMain.duration);}
			else if(dur==-1){Destroy(go,psMain.startLifetime.constantMax+psMain.duration*2);}
			var startSize=psMain.startSize;
			var sizeMin=startSize.constantMin;var sizeMax=startSize.constantMax;if(sizeMin==0){sizeMin=sizeMax;}
			var startSpeed=psMain.startSpeed;
			var speedMin=startSpeed.constantMin;var speedMax=startSpeed.constantMax;if(speedMin==0){speedMin=speedMax;}
			var startColor=new ParticleSystem.MinMaxGradient(psMain.startColor.colorMin,psMain.startColor.colorMax);
			var _color=startColor.colorMin;if(startColor.colorMin.r<0.15f&&startColor.colorMin.g<0.15f&&startColor.colorMin.b<0.15f){_color=startColor.colorMax;}
			if(_color==Color.clear){_color=Color.white;}
			//psMain.startColor=new ParticleSystem.MinMaxGradient(_color,psMain.startColor.colorMax);
			var colorBySpeed=ps.colorBySpeed;
			var colorMin=colorBySpeed.range.x;var colorMax=colorBySpeed.range.y;
			psMain.startSize=new ParticleSystem.MinMaxCurve(sizeMin*mult, sizeMax*mult);
			psMain.startSpeed=new ParticleSystem.MinMaxCurve(speedMin*mult, speedMax*mult);
			//colorBySpeed.range=new Vector2(colorMin*mult, colorMax*mult);
			//colorBySpeed.range=new Vector2(colorMin*30, colorMax*30);
			var psShape=ps.shape;if(multShape){psShape.scale*=mult;}

			var psUI=go.AddComponent<UnityEngine.UI.Extensions.UIParticleSystem>();
			psUI.raycastTarget=false;
			var _tex=ps.GetComponent<Renderer>().material.GetTexture("_MainTex");
			Material mat=new Material(Shader.Find("UI Extensions/Particles/Additive"));
			/*Debug.Log(go.name+" | ColorMin: "+startColor.colorMin);
			Debug.Log(go.name+" | ColorMax: "+startColor.colorMax);
			float H,S,V;Color.RGBToHSV(_color,out H,out S,out V);
			Debug.Log(go.name+" | _color: "+_color + " | HSV("+H+", "+S+", "+V+")");*/
			if(_isColorDark(_color)||type==1){
				//Debug.Log(go.name+" - IsDark");
				mat=new Material(Shader.Find("UI Extensions/Particles/Alpha Blended"));
			}
			mat.SetTexture("_MainTex",_tex);
			psUI.material=mat;
		}
	}
	public static Quaternion QuatRotateTowards(Vector3 target, Vector3 curPos, float rotModif=90){
		Vector3 vectorToTarget = target - curPos;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotModif;
		return Quaternion.AngleAxis(angle, Vector3.forward);
	}
	public static bool _isColorDark(Color color){bool b=false;float H,S,V;Color.RGBToHSV(color,out H,out S,out V);if(V<=0.3f){b=true;}return b;}
	public static void MakeParticleLooping(ParticleSystem ps){var psMain=ps.main;psMain.loop=true;psMain.stopAction=ParticleSystemStopAction.None;}

	public class CoroutineWithData {
		public Coroutine coroutine { get; private set; }
		public object result;
		private IEnumerator target;
		public CoroutineWithData(MonoBehaviour owner, IEnumerator target){
			this.target=target;
			this.coroutine=owner.StartCoroutine(Run());
		}
	
		private IEnumerator Run(){
			while(target.MoveNext()){
				result=target.Current;
				yield return result;
			}
		}
 	}
	//public static float DeltaPercentMinMax(float cur,float max){return (cur-max)/100;}
	public static float Normalize(float val,float min,float max){return (val-min)/(max-min);}
	public static float InvertNormalized(float val){return 1-val;}
	public static float InvertNormalizedAbs(float val){return Mathf.Abs(InvertNormalized(val));}
	public static float InvertNormalizedMin(float val,float min){return InvertNormalized(val)*min;}
	public static float RandomFloat(float min,float max){return (float)System.Math.Round(UnityEngine.Random.Range(min, max), 2);}
	public static bool CheckChance(float chance){return chance>=RandomFloat(0f,100f);}

	

    public static void SetActiveAllChildren(Transform transform, bool value){
        foreach (Transform child in transform){
            child.gameObject.SetActive(value);
 
            SetActiveAllChildren(child, value);
        }
    }
#endregion
}

[System.Serializable]
public class GObject{
	public string name;
	[AssetsOnly]public GameObject gobj;
}
[System.Serializable]
public class GSprite{
	public string name;
	public Sprite spr;
}
[System.Serializable]
public class GTextSprite{
	public string name;
	public Texture2D text;
	public Rect rect;
	public Vector2 pivot;
}
[System.Serializable]
public class GMaterial{
	public string name;
	public Material mat;
}
[System.Serializable]
public class ShaderMatProps{
	//public string name;
	public Texture2D text;
    [Range(0,1)]public float hue=0;
    [Range(0,1)]public float saturation=0.5f;
    [Range(0,1)]public float value=0.5f;
    [Range(0,1)]public float negative=0;
    [Range((4/512),1)]public float pixelate=1;
    [Range(0,1)]public float blur=0;
    public bool lowResBlur=true;
}
[System.Serializable]
public class SimpleAnim{
	public Sprite spr;
	public float delay=0.02f;
}
[System.Serializable]
public class ListOfSimpleAnims{
	public string name;
	public List<SimpleAnim> anim;
}
[System.Serializable]
public class TransformAndPos{
	public Transform trans;
	public Vector2 pos;
}
[System.Serializable]
public class RectTransformAndPos{
	public RectTransform trans;
	public Vector2 pos;
}
[System.Serializable]
public class RectTransformAlign{
	public RectTransform trans;
	public TextAnchor align;
}