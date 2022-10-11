using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour{    public static Player instance;
    [DisableInPlayMode][SerializeField] Transform feetPos;
    [DisableInPlayMode][SerializeField] float checkRadius=0.3f;
    [DisableInPlayMode][SerializeField] LayerMask whatIsGround;
    public float speedDef=6f;
    [DisableInEditorMode]public float speed;
    //public float accelerationSpeed=1.5f;
    public float jumpForceDef=5.75f;
    [DisableInEditorMode]public float jumpForce;
    public float jumpTimeDef=0.35f;
    //[DisableInEditorMode]public float jumpTimeC;
    public float jumpDelayDef=0.5f;
    [DisableInEditorMode]public float jumpDelay;
    public float defaultGravity=2;
    public float groundFriction=0.075f;
    [DisableInEditorMode]public float accumulatedSpeed=1;
    [DisableInEditorMode]public float accumulatedSpeedTimer;
    [DisableInEditorMode]public int wallBounces;
    [DisableInEditorMode]public float wallBounceTimer;
    [DisableInEditorMode]public float wallBounceTimerStop;
    [DisableInEditorMode][SerializeField]bool isGrounded;
    [DisableInEditorMode][SerializeField]float jumpTimer;
    [DisableInEditorMode][SerializeField]bool isJumping;
    
    [HideInInspector]public bool damaged;
    [HideInInspector]public bool healed;


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
        MovePlayerHorizontal();
        if(wallBounceTimer>0)wallBounceTimer-=Time.deltaTime;
        if(wallBounceTimerStop>0)wallBounceTimerStop-=Time.deltaTime;
        if(wallBounceTimerStop<=0)wallBounces=0;

        if(accumulatedSpeedTimer>0)accumulatedSpeedTimer-=Time.deltaTime;
        if(accumulatedSpeedTimer<=0){accumulatedSpeed-=0.04f;accumulatedSpeedTimer=0.075f;}
        if(accumulatedSpeed>1){
            var speedMax=1.5f;
            if(accumulatedSpeed>speedMax){accumulatedSpeed=speedMax;}
            speed=speedDef*accumulatedSpeed*1.5f;//;if(accumulatedSpeed==speedMax){speed=speedDef*2;}
            jumpForce=jumpForceDef*accumulatedSpeed;if(accumulatedSpeed>=1.3f){jumpForce=jumpForceDef*1.1f;}
            //jumpTimeC=jumpTimeDef*accumulatedSpeed*0.9f;
        }else{
            if(accumulatedSpeed<1){accumulatedSpeed=1;}
            speed=speedDef;
            jumpForce=jumpForceDef;
            //jumpTimeC=jumpTimeDef;
        }
    }
    void MovePlayerHorizontal(){
        moveInput=Input.GetAxisRaw("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        if(moveInput!=0&&wallBounceTimer<=0){
            rb.velocity=new Vector2(speed*moveInput,rb.velocity.y);
            AddSpeed(0.0025f,1f);
            //if(rb.velocity.x<speedDef&&rb.velocity.x>-speedDef){rb.velocity=new Vector2(Mathf.Clamp(rb.velocity.x+(accelerationSpeed*moveInput),-speedDef,speedDef),rb.velocity.y);}//Acelerate
        }
        if(moveInput==0){//Slide
            if(rb.velocity.x>0.5f||rb.velocity.x<-0.5f){
                var t=1;if(rb.velocity.x<0){t=-1;}
                rb.velocity=new Vector2(rb.velocity.x-(groundFriction*t),rb.velocity.y);
            }else{if(wallBounces==0){rb.velocity=new Vector2(0,rb.velocity.y);}}
            //accumulatedSpeed-=0.025f;//Substract accumSpeed when not moving
        }
    }
    void MovePlayerJump(){
        isGrounded=(Physics2D.OverlapCircle(feetPos.position,checkRadius,whatIsGround)&&!Input.GetKey(KeyCode.Space));//Check if grounded
        if(isGrounded){jumpTimer=jumpTimeDef;}
        if(jumpTimer==jumpTimeDef&&jumpDelay<=0&&Input.GetKeyDown(KeyCode.Space)){
            isJumping=true;
            AddSpeed(0.03f,1.5f);
            rb.velocity=Vector2.up*jumpForce;
            jumpDelay=jumpDelayDef;
        }
        
        if(jumpDelay>0){jumpDelay-=Time.deltaTime;}
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
        wallBounces++;
        wallBounceTimer=0.2f;
        wallBounceTimerStop=0.7f;
        if(isGrounded){rb.velocity=new Vector2((speed*faceDir)*-1,rb.velocity.y);AddSpeed(0.25f,3f);}
        else{if(rb.velocity.y>-1f||rb.velocity.x>speedDef*2f){rb.velocity=new Vector2((speed*faceDir)*-1,jumpForce*1.3f);AddSpeed(0.125f,3f);}}//Bounce upwards when not grounded and already jumping up
    }
    void AddSpeed(float amnt,float timerAddMult=1){
        accumulatedSpeed+=amnt;
        if(accumulatedSpeedTimer<0.5f)accumulatedSpeedTimer+=amnt*timerAddMult;
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag.Contains("Wall")){
            if(!isJumping&&wallBounces<1)BounceWall();
            else if(isJumping&&!isGrounded&&wallBounces<1)BounceWall();
        }
    }
}