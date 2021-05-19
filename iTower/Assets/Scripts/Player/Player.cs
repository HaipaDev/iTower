using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    [Header("Variables")]
    public float speed=4;
    public float jumpForce=4;
    [SerializeField] Transform feetPos;
    [SerializeField] float checkRadius=0.3f;
    [SerializeField] LayerMask whatIsGround;
    [Header("Other")]
    [HideInInspector]public bool damaged;
    [HideInInspector]public bool healed;
    [SerializeField]bool isGrounded;
    [SerializeField]float jumpTimer;
    public float jumpTime;
    [SerializeField]bool isJumping;

    Rigidbody2D rb;
    float moveInput;
    void Start(){
        rb=GetComponent<Rigidbody2D>();
    }
    void Update(){
        if(moveInput>0){GetComponent<SpriteRenderer>().flipX=true;}else if(moveInput<0){GetComponent<SpriteRenderer>().flipX=false;}//Flip the sprite
        MovePlayerJump();
    }
    void FixedUpdate(){
        MovePlayerHorizontal();
    }
    void MovePlayerHorizontal(){
        moveInput=Input.GetAxisRaw("Horizontal");
        rb.velocity=new Vector2(moveInput*speed,rb.velocity.y);
    }
    void MovePlayerJump(){
        isGrounded=Physics2D.OverlapCircle(feetPos.position,checkRadius,whatIsGround);//Check if grounded
        if(Input.GetKey(KeyCode.Space)){isGrounded=false;}//Only isGrounded when not holding space
        if(isGrounded){jumpTimer=jumpTime;}
        if(jumpTimer==jumpTime&&Input.GetKeyDown(KeyCode.Space)){
            isJumping=true;
            rb.velocity=Vector2.up*jumpForce;
        }
        
        if(isJumping&&Input.GetKey(KeyCode.Space)){
            if(jumpTimer>0){
                rb.velocity=Vector2.up*jumpForce;
                jumpTimer-=Time.deltaTime;
            }else{isJumping=false;}
        }
        if(Input.GetKeyUp(KeyCode.Space)){isJumping=false;}
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.tag=="Ground"){
            GetComponent<Collider>().material.dynamicFriction=0;
        }
    }
 
    /*void OnTriggerExit(Collider other){
        if(other.gameObject.tag=="Ground"){
            GetComponent<Collider>().material.dynamicFriction=1;
        }
    }*/
}
