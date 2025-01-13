using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl2 : MonoBehaviour
{
    public int playerIndex;
    public float speed = 5f;
    public float jump;
    public KeyCode punch;
    public Transform groundCollider;
    public LayerMask groundLayer;
    public bool faceRight = true;
    private bool isGrounded;
    private bool isJump;
    public int maxJumpCount = 2;
    int jumpCount;
    
    public float Move;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        switch (playerIndex){
            case 1:
                Move = Input.GetAxis("Horizontal1");
                isJump = Input.GetButtonDown("Vertical1");
                break;
            case 2:
                Move = Input.GetAxis("Horizontal2");
                isJump = Input.GetButtonDown("Vertical2");
                break;
        }
        
        flipSprite();

        rb.velocity = new Vector2(speed * Move, rb.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCollider.position, 0.2f, groundLayer);
        
        if(isGrounded && jumpCount < maxJumpCount && !Input.GetButtonDown("Jump")){
            jumpCount = maxJumpCount;
        }
        if(isJump){
            if(isGrounded || (!isGrounded && jumpCount-1 > 0)){
                jumpCount--;
                //rb.AddForce(new Vector2(rb.velocity.x, jump));
                rb.velocity = new Vector2(rb.velocity.x, jump);
            }
        }
        if(UnityEngine.Input.GetKeyDown(punch)){
            Debug.Log($"{gameObject.name} punched");
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
}