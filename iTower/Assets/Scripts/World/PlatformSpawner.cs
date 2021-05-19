using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour{
    [SerializeField]GameObject platformPrefab;
    [SerializeField]int startPlatformsCount=8;
    [SerializeField]int maxPlatformCount=13;
    public Vector2 spawnDistances=new Vector2(1f,2f);//X is min, Y is max
    public Vector2 spawnX=new Vector2(-3f,3f);//X is left max, Y is right max
    public Vector2 sizes=new Vector2(1f,3f);//X is min, Y is max
    public float fallSpeed=1.4f;
    GameObject highestPlatform;
    bool fallingPlatforms;
    public float speedTime=0.5f;
    [SerializeField]float speedTimer=-4;
    [SerializeField]float highestPlayerPos=0;
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
            SetPlatformSpeed(fallSpeed);
            if(FindObjectsOfType<Platform>().Length<maxPlatformCount){SpawnPlatform(highestPlatform.transform.position.y+Random.Range(spawnDistances.x,spawnDistances.y));}//Spawn new platforms
        }
        /*if(pyPos>2f){//Speed up when Player is high
            speedTimer=speedTime*(1+(pyPos/10));
        }
        if(speedTimer>0){
            speedTimer-=Time.deltaTime;
            if(pyPos>highestPlayerPos){
                SetPlatformSpeed(fallSpeed*highestPlayerPos);
                Player.instance.GetComponent<Rigidbody2D>().gravityScale=highestPlayerPos;
                highestPlayerPos=pyPos;
            }
        }*/
        //if(speedTimer<=0){Player.instance.GetComponent<Rigidbody2D>().gravityScale=Player.instance.defaultGravity;highestPlayerPos=0;if(fallingPlatforms){SetPlatformSpeed(fallSpeed);}}//Bring back normal speed
    }
    void SetPlatformSpeed(float speed){
        foreach(Platform p in FindObjectsOfType<Platform>()){p.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-speed);}
    }
    void SpawnPlatform(float yy){
        GameObject go=Instantiate(platformPrefab,new Vector2(Random.Range(spawnX.x,spawnX.y),yy),Quaternion.identity);//Spawn platform
        go.transform.localScale=new Vector2(Random.Range(sizes.x,sizes.y),go.transform.localScale.y);//Set size
        highestPlatform=go;//On each spawn set the highest platform
    }
}
