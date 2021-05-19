using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour{
    [SerializeField]GameObject platformPrefab;
    [SerializeField]int startPlatformsCount=8;
    public Vector2 spawnDistances=new Vector2(1f,2f);//X is min, Y is max
    public Vector2 spawnX=new Vector2(-3f,3f);//X is left max, Y is right max
    public Vector2 sizes=new Vector2(1f,3f);//X is min, Y is max
    void Start(){
        //Spawn first static plaforms
        float yy=-4.6f;//First platform Y
        for(var i=0;i<startPlatformsCount;i++){
            GameObject go=Instantiate(platformPrefab,new Vector2(Random.Range(spawnX.x,spawnX.y),yy),Quaternion.identity);//Spawn platform
            go.transform.localScale=new Vector2(Random.Range(sizes.x,sizes.y),go.transform.localScale.y);//Set size
            yy+=Random.Range(spawnDistances.x,spawnDistances.y);//Set new Y position for next
        }
    }
    void Update(){
        
    }
}
