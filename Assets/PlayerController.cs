using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    void Start()
    {

    }

    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }
}
