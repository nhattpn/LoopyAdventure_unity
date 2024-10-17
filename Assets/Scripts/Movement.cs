using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{   
    [SerializeField] private float speed;
    [SerializeField] private float hjump; //cho phep chinh gia tri trong unity
    [SerializeField] private float ljump;
    private bool move_left;
    private bool move_right;
    private Rigidbody2D character;
    private Animator anim;
    private bool Grounded;
    private void Awake (){
        character= GetComponent <Rigidbody2D>();
        anim= GetComponent <Animator>();
    }
    private void Update(){
        //velocity chinh vi tri cua component theo thoi gian, velocity=new vector2(2,0); tuc la di chuyen x qua phai 2 unit tren giay
        float horizontalInput=Input.GetAxis("Horizontal");
        if(Grounded) {character.velocity = new Vector2(horizontalInput*speed, character.velocity.y);} // cai horizontal handle cac truong hop left/right arrow va a/d 
        if(Input.GetKey(KeyCode.Space) && Grounded){
            Jump();
        }
        if(horizontalInput > 0.1f){
            move_left=false;
            transform.localScale= Vector3.one;
        }
        else if(horizontalInput < -0.1f){
            move_left=true;
            transform.localScale= new Vector3(-1,1,1);
        }
        anim.SetBool("IsRunning", horizontalInput!=0);
        anim.SetBool("IsGrounded",Grounded);
    }
    private void Jump(){
        if(move_left) {character.velocity = new Vector2(character.velocity.x - ljump, hjump);}
        else character.velocity = new Vector2(character.velocity.x + ljump, hjump);
        anim.SetTrigger("Jump");
        Grounded=false;
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag=="Ground"){
            Grounded=true;
        }
    }
}
