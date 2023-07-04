using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float speed = 5f;

    [SerializeField] private float jumpForce = 350f;
    // private float _vertical;

    private string _currentAnim;

    private float _horizontal;
    private bool _isAttack;
    private bool _isGrounded = true;
    private bool _isJumping;

    // Start is called before the first frame update

    // Update is called once per frame
    private void FixedUpdate()
    {
        _isGrounded = CheckGrounded();
        _horizontal = Input.GetAxisRaw("Horizontal");

        if (_isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (_isGrounded)
        {
            if (_isJumping) return;
            // jump
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                Jump();
            }

            // change run anim
            if (Mathf.Abs(_horizontal) > 0.1f && !_isJumping)
            {
                ChangeAnim("run");
            }

            // attack
            if (Input.GetKeyDown(KeyCode.C) && _isGrounded) Attack();
            
            // throw
            if (Input.GetKeyDown(KeyCode.V) && _isGrounded) Throw();
        }
        // check falling
        if (!_isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            _isJumping = false;
        }
        
        
        
        if (Mathf.Abs(_horizontal) != 0f)
        {
            rb.velocity = new Vector2(_horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, _horizontal > 0 ? 0f : 180f, 0));
        }

        else if (_isGrounded)
        {
            rb.velocity = Vector2.zero;
            ChangeAnim("idle");
        }
    }

    private bool CheckGrounded()
    {
        var position = transform.position;
        Debug.DrawLine(position, position + Vector3.down * 1.25f, Color.red);
        var hit = Physics2D.Raycast(position, Vector2.down, 1.25f, groundLayer);
        return hit.collider != null;
    }

    private void ChangeAnim(string animName)
    {
        if (_currentAnim == animName) return;
        animator.ResetTrigger(animName);
        _currentAnim = animName;
        animator.SetTrigger(_currentAnim);
    }

    private void Attack()
    {
        _isAttack = true;
        ChangeAnim("attack");
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void ResetAttack()
    {
        ChangeAnim("ilde");
        _isAttack = false;
    }
    
    private void Throw()
    {
        _isAttack = true;
        ChangeAnim("throw");
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Jump()
    {
        _isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }
}