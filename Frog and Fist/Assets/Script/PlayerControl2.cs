using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl2 : MonoBehaviour
{
    public int playerIndex;
    String Horizontal;
    String Vertical;
    KeyCode Down;

    public bool alive = true;
    public int ultimatePoint = 0;
    bool ultimateStatus;
    [SerializeField] GameObject spawn;
    public float speed = 5f;
    public float jump;
    public KeyCode punch;
    public KeyCode ultimate;
    [SerializeField] private Transform groundCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool faceRight = true;
    public bool isAttack;
    public float attackStrength = 20;
    public float attackSpeed = 1f;
    public float stuntDelay = 0.2f;
    public GameObject atkHitbox;
    public bool isGrounded;
    public bool isJump;
    public int maxjumpRemain = 2;
    public int jumpRemain;
    public bool isDown;
    private GameObject curPlatform;
    [SerializeField] private Collider2D Collider;
    
    private float Move;
    private bool canMove = true;

    private Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.Play("idle");
        atkHitbox.SetActive(false);
        jumpRemain = maxjumpRemain;

        if(!faceRight){
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        switch (playerIndex){
            case 1:
                Horizontal = "Horizontal1";
                Vertical = "Vertical1";
                Down = KeyCode.S;
                break;
            case 2:
                Horizontal = "Horizontal2";
                Vertical = "Vertical2";
                Down = KeyCode.DownArrow;
                break;
        }
    }

// FIX //
// Update() for Input
// FixedUpdate() for operation
// gravity, press down after jump
// organize the script, PlayerControl only for

    void Update()
    {   
        Move = Input.GetAxis(Horizontal);
        isJump = Input.GetButtonDown(Vertical);
        isDown = Input.GetKeyDown(Down);
        
        flipSprite();

        if(canMove) rb.velocity = new Vector2(speed * Move, rb.velocity.y);
        
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCollider.position, 0.2f, groundLayer);

        /*if(!alive){
            StartCoroutine(Respawn());
            if(isGrounded) alive = true;
        }*/
        
        // Jump Function
        if(isJump && jumpRemain > 0){
            rb.velocity = new Vector2(rb.velocity.x, jump);
            jumpRemain--;
        }
        if(isGrounded && !wasGrounded) jumpRemain = maxjumpRemain;

        // Get Down Platform Function
        if(isDown){
            if(isGrounded && curPlatform != null){
                StartCoroutine(DisableCollision());
            }
            else if(!isGrounded){
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y-6);
            }
        }

        // Punch Function
        if(UnityEngine.Input.GetKeyDown(punch) && !isAttack){
            attack();
        }

        // Ultimate Skill Function
        if(ultimatePoint >= 100 && !ultimateStatus){
            ultimatePoint = 100;
            ultimateStatus = true;
            Debug.Log($"{transform.name}'s ultimate is ready!");
        }
        if(UnityEngine.Input.GetKeyDown(ultimate)){
            if(ultimateStatus){
                ultimatePoint = 0;
                ultimateStatus = false;
                Debug.Log($"ULTIMATE {transform.name} ! ! !");
            }
            else Debug.Log($"{transform.name} not ready for ultimate");
        }
    }

    private void flipSprite(){
        if(faceRight && Move < 0f || !faceRight && Move > 0f){
            faceRight = !faceRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Respawn(){
        yield return new WaitForSeconds(1);
        transform.position = spawn.transform.position;
        rb.simulated = true;
    }

    private void attack(){
        isAttack = true;
        animator.Play("punch");
        atkHitbox.SetActive(true);
        Invoke("resetAttack", 0.4f/attackSpeed);
    }
    private void resetAttack() {
        isAttack = false;
        atkHitbox.SetActive(false);
        animator.Play("idle");
    }
    public void hurt(Transform attacker, float strength){
        animator.Play("hurt");
        canMove = false;
        Vector2 direction = (transform.position - attacker.transform.position).normalized;
        rb.AddForce(direction*strength, ForceMode2D.Impulse);
        ultimatePoint += 1;

        Invoke("resetHurt", stuntDelay);
    }

    private void resetHurt() {
        //rb.velocity = Vector3.zero;
        canMove = true;
        animator.Play("idle");
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Finish")){
            Debug.Log($"{transform.name} Out!");
            //alive = false;
            rb.simulated = false;
            rb.velocity = Vector2.zero;
            StartCoroutine(Respawn());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Platforms")){
            curPlatform = collision.gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Platforms")){
            curPlatform = null;
        }
    }
    private IEnumerator DisableCollision(){
        Collider2D platformCollider = curPlatform.GetComponent<EdgeCollider2D>();

        Physics2D.IgnoreCollision(Collider, platformCollider);
        yield return new WaitForSeconds(0.4f);
        Physics2D.IgnoreCollision(Collider, platformCollider, false);
    }
}