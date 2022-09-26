using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI txt;
    [SerializeField] public string value="score";
    void Start(){
        txt=GetComponent<TMPro.TextMeshProUGUI>();
    }
    void Update(){ChangeText();}
    void ChangeText(){
        if(GameManager.instance!=null){
            if(value=="gameVersion")txt.text=GameManager.instance.gameVersion;
        }
    }
}
