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
    public float wallCheckDistance;
    private bool _isGrounded;
    private bool _isWallDetected;
    private bool _canWallSlide;
    private bool _isWallSliding;
    
    
    private bool _facingRight = true;
    private float facingDirection = 1;
    
    private void Start()
    {
        _rb   = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimationController();
        FlipController();
        CollisionCheck();
        
        InputChecks();

        if (_isGrounded) _canDoubleJump = true;

        if (_canWallSlide)
        {
            _isWallSliding = true;
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.1f);
        }
        
        if (!_isWallDetected)
        {
            _isWallSliding = false;
            Move();
        }
    }

    private void AnimationController()
    {
        bool isMoving = _rb.velocity.x != 0;
        _anim.SetBool("isMoving", isMoving);
        _anim.SetBool("isGrounded", _isGrounded);
        _anim.SetBool("isWallSliding", _isWallSliding);
        _anim.SetFloat("yVelocity", _rb.velocity.y);
    }

    private void InputChecks()
    {
        _movingInput = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Vertical") < 0) _canWallSlide = false;
        
        if (Input.GetKeyDown(KeyCode.Space)) JumpButton();
    }

    private void JumpButton()
    {
        if (_isGrounded) Jump();
        else if (_canDoubleJump) {
            _canDoubleJump = false;
            Jump();
        }
    }

    private void Move() => _rb.velocity = new Vector2(moveSpeed * _movingInput, _rb.velocity.y);

    private void Jump() => _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);

    private void FlipController()
    {
        if (_facingRight && _movingInput < 0)
        {
            Flip();
        } else if (!_facingRight && _movingInput > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0);
    }
    private void CollisionCheck()
    {
        _isGrounded     = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        _isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

        if (_isWallDetected && _rb.velocity.y < 0) _canWallSlide = true;
        if (!_isWallDetected) _canWallSlide = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
    }
}
