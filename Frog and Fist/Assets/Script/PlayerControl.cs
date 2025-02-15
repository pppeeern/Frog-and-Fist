using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    String Horizontal;
    String Vertical;
    KeyCode Down;

    [Header("Player Properties")]
    public int playerIndex;
    public float moveSpeed = 5f;
    float Move;
    public float jumpPower = 8f;
    public int maxJump = 2;
    int jumpRemain;
    public float damageResist = 10;

    [Header("Ground Check")]
    public bool isGrounded;
    [SerializeField] private Transform groundCollider;
    [SerializeField] private LayerMask groundLayer;

    [Header("Boolean")]
    [SerializeField] private bool faceRight = true;
    public bool canMove = true;
    public bool alive = true;
    bool isJump;
    bool isDown;

    [Header("Reference")]
    [SerializeField] GameObject spawn;
    [SerializeField] Collider2D Collider;
    GameObject curPlatform; 
    Rigidbody2D rb;
    Animator animator;
    PlayerCombat playerCombat;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCombat = GetComponent<PlayerCombat>();

        animator.Play("idle");
        jumpRemain = maxJump;

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
// gravity, press down after jump

    void Update()
    {   
        Move = Input.GetAxis(Horizontal);
        isJump = Input.GetButtonDown(Vertical);
        isDown = Input.GetKeyDown(Down);
        
        flipSprite();

        if(canMove) rb.velocity = new Vector2(moveSpeed * Move, rb.velocity.y);
        
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCollider.position, 0.2f, groundLayer);
        
        // Jump Function
        if(isJump && jumpRemain > 0){
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpRemain--;
        }
        if(isGrounded && !wasGrounded) jumpRemain = maxJump;

        // Get Down Platform Function
        if(isDown){
            if(isGrounded && curPlatform != null){
                StartCoroutine(DisableCollision());
            }
            else if(!isGrounded){
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y-6);
            }
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

        alive = true;
        playerCombat.resetOnSpawn();
        rb.simulated = true;
        rb.WakeUp();
        transform.position = spawn.transform.position + new Vector3(0, 0.5f, 0);
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.1f);
    }


    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Finish")){
            Debug.Log($"{transform.name} Out!");
            alive = false;
            rb.velocity = Vector2.zero;
            rb.simulated = false;
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