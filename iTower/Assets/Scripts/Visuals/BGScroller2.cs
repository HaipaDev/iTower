using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller2 : MonoBehaviour{
    //Have 1 child with a SpriteRenderer, the parent contains just this script
    [SerializeField]public dir dir;
    [SerializeField]public float strength;
    public float currentSpeed;
    float length;
    float[] startpos=new float[2];
    int dirM;
    void Start(){
        if(dir==dir.up||dir==dir.right){dirM=1;}else{dirM=-1;}//Set directions for calculations
        startpos[0]=transform.GetChild(0).position.y;
        length=transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y;
        var go1=Instantiate(transform.GetChild(0).gameObject,transform);//Create 2nd bg
        if(dir==dir.left||dir==dir.right){//Do the same for X
            startpos[0]=transform.GetChild(0).position.x;
            length=transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
            go1.transform.position=new Vector2(go1.transform.position.x+length*-dirM,go1.transform.position.y);startpos[1]=go1.transform.position.x;}//Spawn reverse to where they'll go
        else{go1.transform.position=new Vector2(go1.transform.position.x,go1.transform.position.y+length*-dirM);startpos[1]=go1.transform.position.y;}
    }
    void FixedUpdate(){
        for(var i=0;i<transform.childCount;i++){
            var pos=transform.GetChild(i).position.y;
            if(dir==dir.up||dir==dir.down){
                transform.GetChild(i).position=new Vector2(transform.GetChild(i).position.x,pos+currentSpeed*dirM);//Move
                if(pos>startpos[i]+length||pos<startpos[i]-length)transform.GetChild(i).position=new Vector2(transform.GetChild(i).position.x,startpos[i]);//Bring back to startpos
            }
            else{//Do the same for X
                pos=transform.GetChild(i).position.x;
                transform.GetChild(i).position=new Vector2(pos+currentSpeed*dirM,transform.GetChild(i).position.y);
                if(pos>startpos[i]+length||pos<startpos[i]-length)transform.GetChild(i).position=new Vector2(startpos[i],transform.GetChild(i).position.y);
            }
        }
    }
}