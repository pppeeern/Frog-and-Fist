using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Transform groundCollider;
    public LayerMask groundLayer;
    private bool isGrounded;

    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode down;
    public KeyCode punch;
    
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCollider.position, 0.5f, groundLayer);
        Debug.Log(isGrounded);
        
        if (UnityEngine.Input.GetKey(left))
        {
            rb.velocity = new UnityEngine.Vector2(-moveSpeed, 0);
        }
        else if (UnityEngine.Input.GetKey(right))
        {
            rb.velocity = new UnityEngine.Vector2(moveSpeed, 0);
        }
        else
        {
            rb.velocity = new UnityEngine.Vector2(0, rb.velocity.y);
        }

        if (UnityEngine.Input.GetButtonDown("jump") && isGrounded)
        {
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpForce);
        }
    }
}
