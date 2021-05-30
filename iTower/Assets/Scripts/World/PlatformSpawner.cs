using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlatformSpawner : MonoBehaviour{
    [SerializeField]GameObject platformPrefab;
    [SerializeField]int startPlatformsCount=8;
    [SerializeField]int maxPlatformCount=13;
    public Vector2 spawnDistances=new Vector2(1f,2f);//X is min, Y is max
    public Vector2 spawnX=new Vector2(-3f,3f);//X is left max, Y is right max
    public Vector2 sizes=new Vector2(1f,3f);//X is min, Y is max
    public float fallSpeed=1.4f;
    [HideInInspector]public GameObject highestPlatform;
    int platformCount;
    bool fallingPlatforms;
    public float speedTime=0.5f;
    [SerializeField]float speedTimer=-4;
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
        }
        var pSpeed=Player.instance.accumulatedSpeed;
        if(pSpeed>1.15f){//Speed up when Player accumulated speed is higher
            speedTimer=speedTime*(1+(pSpeed/10));
        }
        if(pyPos<3&&speedTimer>0){
            speedTimer-=Time.deltaTime;
            if(fallingPlatforms){SetPlatformSpeed(fallSpeed*1.25f*Mathf.Abs(pSpeed));}
        }
        if(pyPos>3&&pyPos<8&&fallingPlatforms){SetPlatformSpeed(fallSpeed*1.25f*Mathf.Abs(8-pyPos));}
        if(pyPos>8&&fallingPlatforms){SetPlatformSpeed(fallSpeed*1.25f*Mathf.Abs(12+8-pyPos));}
        if(speedTimer<=0){//Bring back normal speed
            if(fallingPlatforms){SetPlatformSpeed(fallSpeed);}
        }
    }
    void SetPlatformSpeed(float speed){
        foreach(Platform p in FindObjectsOfType<Platform>()){p.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-speed);}
        FindObjectOfType<BGScroller2>().currentSpeed=speed*FindObjectOfType<BGScroller2>().strength;
    }
    void SpawnPlatform(float yy){
        GameObject go=Instantiate(platformPrefab,new Vector2(Random.Range(spawnX.x,spawnX.y),yy),Quaternion.identity);//Spawn platform
        SetPlatformScale(go,Random.Range(sizes.x,sizes.y));//Set size
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
