using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;
    
    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;
    
    private bool _canDoubleJump = true;
    
    private float _movingInput;
    
    [Header("Collision info")]
    public LayerMask whatIsGround;
    public float groundCheckDistance;
  
    private bool _isGrounded;

    private void Start()
    {
        _rb   = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimationController();

        CollisionCheck();
        
        InputChecks();

        if (_isGrounded) _canDoubleJump = true;
        Move();
    }

    private void AnimationController()
    {
        bool isMoving = _rb.velocity.x != 0;
        _anim.SetBool("isMoving", isMoving);
        _anim.SetBool("isGrounded", _isGrounded);
        _anim.SetFloat("yVelocity", _rb.velocity.y);
    }

    private void InputChecks()
    {
        _movingInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
    }

    private void JumpButton()
    {
        if (_isGrounded)
        {
            Jump();
        } else if (_canDoubleJump)
        {
            _canDoubleJump = false;
            Jump();
        }
    }

    private void Move()         => _rb.velocity = new Vector2(moveSpeed * _movingInput, _rb.velocity.y);

    private void Jump()         => _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);

    private bool CollisionCheck()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        return _isGrounded;
    }


    private void OnDrawGizmos() => Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
}
