using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Sirenix.OdinInspector;

public class PlatformSpawner : MonoBehaviour{
    [SerializeField]GameObject platformPrefab;
    [SerializeField]int startPlatformsCount=8;
    [SerializeField]int maxPlatformCount=13;
    public Vector2 spawnDistances=new Vector2(1f,2f);//X is min, Y is max
    public Vector2 spawnX=new Vector2(-3f,3f);//X is left max, Y is right max
    public bool spawnBasedOnLastPos=true;
    [EnableIf("spawnBasedOnLastPos")]public float spawnBasedOnLastPosChance=70f;
    public Vector2 spawnXnext=new Vector2(-1.5f,1.5f);//X is left max, Y is right max
    public Vector2 sizes=new Vector2(1f,3f);//X is min, Y is max
    public float fallSpeed=1.4f;
    [HideInInspector]public GameObject highestPlatform;
    int platformCount;
    bool fallingPlatforms;
    public float speedTime=0.5f;
    [SerializeField]float speedTimer=-4;
    [SerializeField]float fallSpeedC;
    Vector2 lastPlatformPos=Vector2.zero;
    void Start(){
        //Spawn first static plaforms
        float yy=-4.6f;//First platform Y
        for(var i=0;i<startPlatformsCount;i++){
            SpawnPlatform(yy);
            yy+=Random.Range(spawnDistances.x,spawnDistances.y);//Set new Y position for next
        }
    }
    void Update(){
        float pyPos=Player.instance.transform.position.y;
        if(pyPos>0&&fallingPlatforms!=true){fallingPlatforms=true;}//If player is higher than middle trigger falling platforms
        if(fallingPlatforms){
            if(FindObjectsOfType<Platform>().Length<maxPlatformCount){SpawnPlatform(highestPlatform.transform.position.y+Random.Range(spawnDistances.x,spawnDistances.y));}//Spawn new platforms
            SetPlatformSpeed(fallSpeedC);//Set speed
        }
        var pSpeed=Player.instance.accumulatedSpeed;
        if(pSpeed>1.15f){//Speed up when Player accumulated speed is higher
            speedTimer=speedTime*(1+(pSpeed/10));
        }
        if(speedTimer>0){
            speedTimer-=Time.deltaTime;
            fallSpeedC=fallSpeed*(1.5f*Mathf.Abs(pSpeed));
        }
        if(speedTimer<=0){//Bring back normal speed
            fallSpeedC=fallSpeed;
        }
        if(pyPos>3){fallSpeedC=fallSpeed*(Mathf.Clamp(pyPos/2,1.66f,6)*(1+(pyPos/10)))*Mathf.Abs(pSpeed);}
    }
    void SetPlatformSpeed(float speed){
        foreach(Platform p in FindObjectsOfType<Platform>()){p.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-speed);}
        FindObjectOfType<BGScroller2>().currentSpeed=speed*FindObjectOfType<BGScroller2>().strength;
    }
    void SpawnPlatform(float yy){
        var posX=0f;
        var _doSpawnBasedOnLast=AssetsManager.CheckChance(spawnBasedOnLastPosChance);
        while(posX>3.5f||posX<-3.5f||posX==0f){
            if((lastPlatformPos==Vector2.zero||(spawnBasedOnLastPos&&_doSpawnBasedOnLast))||!spawnBasedOnLastPos){posX=(float)System.Math.Round(Random.Range(spawnX.x,spawnX.y),2);}
            else{if(spawnBasedOnLastPos)posX=lastPlatformPos.x+(float)System.Math.Round(Random.Range(spawnXnext.x,spawnXnext.y),2);}
        }
        var pos=new Vector2(posX,yy);
        lastPlatformPos=pos;
        GameObject go=Instantiate(platformPrefab,pos,Quaternion.identity);//Spawn platform
        SetPlatformScale(go,(float)System.Math.Round(Random.Range(sizes.x,sizes.y),2));//Set size
        highestPlatform=go;//On each spawn set the highest platform
        platformCount++;
        go.GetComponent<Platform>().ID=platformCount;
        //if(fallingPlatforms&&speedTimer<=0)SetPlatformSpeed(fallSpeed);
    }

    void SetPlatformScale(GameObject go,float size){
        go.GetComponent<SpriteShapeController>().spline.SetPosition(0,new Vector3(-size,0,0));
        go.GetComponent<SpriteShapeController>().spline.SetPosition(1,new Vector3(size,0,0));
    }
}
