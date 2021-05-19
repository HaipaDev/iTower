using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour{
    void Update(){
        if(transform.position.y<-8){Destroy(gameObject);}
    }
}
