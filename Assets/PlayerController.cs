using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D rb;

    private bool _canDoubleJump = true;
    
    private float _movingInput;

    public LayerMask whatIsGround;
    public float groundCheckDistance;
    private bool _isGrounded;

    void Update()
    {
        CollisionCheck();
        
        InputChecks();

        if (_isGrounded) _canDoubleJump = true;
        Move();
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

    private void Move()         => rb.velocity = new Vector2(moveSpeed * _movingInput, rb.velocity.y);

    private void Jump()         => rb.velocity = new Vector2(rb.velocity.x, jumpForce);

    private bool CollisionCheck()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        return _isGrounded;
    }


    private void OnDrawGizmos() => Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
}
