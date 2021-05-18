using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screenflash : MonoBehaviour{
    [SerializeField] Color damageFlashColor;
    [SerializeField] float damageFlashSpeed;
    [SerializeField] Color healFlashColor;
    [SerializeField] float healedFlashSpeed;
    Player player;
    Image image;
    // Start is called before the first frame update
    void Start(){
        player=FindObjectOfType<Player>();
        image=GetComponent<Image>();
    }

    // Update is called once per frame
    void Update(){
    if(player!=null){
        if(player.damaged==true){image.color=damageFlashColor;player.damaged=false;}
        else{image.color=Color.Lerp(image.color, Color.clear, damageFlashSpeed*Time.deltaTime);}
        if(player.healed==true){image.color=healFlashColor;player.healed=false;}
        else{image.color=Color.Lerp(image.color, Color.clear, healedFlashSpeed*Time.deltaTime);}
    }}
}
