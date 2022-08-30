using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;

public class Platform : MonoBehaviour{
    public int ID;
    void Start() {
        if(GetComponentInChildren<TextMeshProUGUI>()!=null){
            if(ID%10==0){GetComponentInChildren<TextMeshProUGUI>().text=ID.ToString();}//Every 10th display number
            else{Destroy(GetComponentInChildren<Canvas>().gameObject);}
        }    
        if(!gameObject.name.Contains("Start")){
            if(ID%50==0){transform.position=new Vector2(0,transform.position.y);SetPlatformScale(8f);}//transform.GetChild(0).localScale=new Vector2(8f,transform.GetChild(0).localScale.y);}//Every 50th long platform
        }
    }
    void Update(){
        if(transform.position.y+0.2f<-7){Destroy(gameObject);}
    }
    void SetPlatformScale(float size){
        GetComponent<SpriteShapeController>().spline.SetPosition(0,new Vector3(-size,0,0));
        GetComponent<SpriteShapeController>().spline.SetPosition(1,new Vector3(size,0,0));
    }
}
