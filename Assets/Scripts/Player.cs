using System;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;

    // [SerializeField] private Animator animator;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5f;

    [SerializeField] private float jumpForce = 350f;

    [SerializeField] private int coin;
    [SerializeField] private bool isAttack;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isJumping;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;

    [SerializeField] private Vector3 savePoint;
    // private float _vertical;

    [SerializeField] private EdgeCollider2D attackArea;
    // private string _currentAnim;


    private float _horizontal;

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }

    // Start is called before the first frame update
    
    // Update is called once per frame
    private void Update()
    {
        if (IsDead) return;
        isGrounded = CheckGrounded();
        // _horizontal = Input.GetAxisRaw("Horizontal");
        if (!isGrounded || isJumping || isAttack) return;
        // jump
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        // change run anim
        if (Mathf.Abs(_horizontal) > 0.1f && !isJumping) ChangeAnim("run");
        // attack
        if (Input.GetKeyDown(KeyCode.C)) Attack();
        // throw
        if (Input.GetKeyDown(KeyCode.V)) Throw();
    }

    private void FixedUpdate()
    {
        if (IsDead) return;
        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // check falling
        if (!isGrounded && rb.velocity.y <= 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }


        if (Mathf.Abs(_horizontal) != 0f)
        {
            rb.velocity = new Vector2(_horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, _horizontal > 0 ? 0f : 180f, 0));
        }

        else if (isGrounded && !isJumping)
        {
            rb.velocity = Vector2.zero;
            ChangeAnim("idle");
        }
    }

    

    public void OnSavePoint()
    {
        savePoint = transform.position;
    }

    protected override void OnInit()
    {
        base.OnInit();
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("Idle");
        DeActiveAttack();
        OnSavePoint();
        UIManager.instance.SetCoin(0);
    }

    protected override void OnDeSpawn()
    {
        base.OnDeSpawn();
        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    private bool CheckGrounded()
    {
        var position = transform.position;
        Debug.DrawLine(position, position + Vector3.down * 1.2f, Color.red);
        var hit = Physics2D.Raycast(position, Vector2.down, 1.2f, groundLayer);
        return hit.collider != null;
    }

    // private void ChangeAnim(string animName)
    // {
    //     if (_currentAnim == animName) return;
    //     animator.ResetTrigger(animName);
    //     _currentAnim = animName;
    //     animator.SetTrigger(_currentAnim);
    // }

    public void Attack()
    {
        if (IsDead || !isGrounded || isJumping || isAttack) return;
        isAttack = true;
        ChangeAnim("attack");
        Invoke(nameof(ResetAttack), 0.25f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.2f);
    }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
    }

    public void Throw()
    {
        if (IsDead || !isGrounded || isJumping || isAttack) return;
        isAttack = true;
        ChangeAnim("throw");
        Invoke(nameof(ResetAttack), 0.25f);
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    public void Jump()
    {
        if (IsDead || !isGrounded || isJumping) return;
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void ActiveAttack()
    {
        attackArea.enabled = true;
    }

    private void DeActiveAttack()
    {
        attackArea.enabled = false;
    }
    
    public void OnMovingPlatform()
    {
        // isJumping = false;
        // isGrounded = true;
        // rb.velocity = Vector2.zero;
        ChangeAnim("Idle");
    }

    public void SetMove(float horizontal)
    {
        _horizontal = horizontal;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Coin"))
        {
            if (!col.CompareTag("DeathZone")) return;
            Debug.Log("Die");
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1f);
            // transform.gameObject.SetActive(false);
            return;
        }

        Debug.Log("Trigger Coin");
        coin++;
        PlayerPrefs.SetInt("coin", coin);
        UIManager.instance.SetCoin(coin);
        Destroy(col.gameObject);
    }
}