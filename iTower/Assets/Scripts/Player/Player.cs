using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    public static Player instance;
    [Header("Properties")]
    public float speed=6;
    public float jumpForce=4;
    public float defaultGravity=2;
    public float groundFriction=0.075f;
    [SerializeField] Transform feetPos;
    [SerializeField] float checkRadius=0.3f;
    [SerializeField] LayerMask whatIsGround;
    [Header("Variables")]
    [HideInInspector]public bool damaged;
    [HideInInspector]public bool healed;
    [SerializeField]bool isGrounded;
    [SerializeField]float jumpTimer;
    public float jumpTime;
    [SerializeField]bool isJumping;
    public int wallBounces;
    public float wallBounceTimer;
    public float wallBounceTimerStop;


    Animator anim;
    Rigidbody2D rb;
    float moveInput;
    int faceDir=-1;

    void Awake(){instance=this;}
    void Start(){
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
    }
    void Update(){
        if(rb.velocity.x>0){GetComponent<SpriteRenderer>().flipX=true;faceDir=1;}else if(rb.velocity.x<0){GetComponent<SpriteRenderer>().flipX=false;faceDir=-1;}//Flip the sprite
        MovePlayerJump();
        if(moveInput==0){//Slide
            if(rb.velocity.x>0.5f||rb.velocity.x<-0.5f){
                var t=1;if(rb.velocity.x<0){t=-1;}
                rb.velocity=new Vector2(rb.velocity.x-(groundFriction*t),rb.velocity.y);
            }else{if(wallBounces==0){rb.velocity=new Vector2(0,rb.velocity.y);}}
        }
        if(wallBounceTimer>0)wallBounceTimer-=Time.deltaTime;
        if(wallBounceTimerStop>0)wallBounceTimerStop-=Time.deltaTime;
        if(wallBounceTimerStop<=0)wallBounces=0;
    }
    void FixedUpdate(){
        MovePlayerHorizontal();
    }
    void MovePlayerHorizontal(){
        moveInput=Input.GetAxisRaw("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        if(moveInput!=0&&wallBounceTimer<=0)rb.velocity=new Vector2(moveInput*speed,rb.velocity.y);
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
        if(Input.GetKeyUp(KeyCode.Space)){isJumping=false;jumpTimer=0;}
        if(!isGrounded&&!isJumping){jumpTimer=0;}
    }

    void BounceWall(){
        Debug.Log("Bounced");
        wallBounces++;
        wallBounceTimer=0.12f*(1+(speed/10));//~0.2 on speed=6
        wallBounceTimerStop=0.12f*speed;//~0.7 on speed=6
        rb.velocity=new Vector2((speed*faceDir)*-1,rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag.Contains("Wall")){
            if(wallBounces<2)BounceWall();
        }
    }
    void OnCollisionExit2D(Collision2D other){
        if(other.gameObject.tag.Contains("Wall")){
            //wallBounceTimer=0.2f;
            //wallBounces=0;
        }
    }
}