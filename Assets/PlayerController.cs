using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D rb;

    private float movingInput;

    public LayerMask whatIsGround;
    public float groundCheckDistance;
    private bool isGrounded;

    void Update()
    {
        CollisionCheck();
        
        movingInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
        }
        Move();
    }
    private void Move()         => rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);

    private void Jump()         => rb.velocity = new Vector2(rb.velocity.x, jumpForce);

    private bool CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        return isGrounded;
    }


    private void OnDrawGizmos() => Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
}
