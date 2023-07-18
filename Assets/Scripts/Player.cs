using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;

    // [SerializeField] private Animator animator;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject predictPoint;
    [SerializeField] private float speed = 5f;

    [SerializeField] private float jumpForce = 350f;

    [SerializeField] private int coin;
    [SerializeField] private bool isAttack;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isGliding;
    [SerializeField] private bool isGlideDone;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Kunai bombPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject predictPoints;
    [SerializeField] private Vector3 savePoint;
    // private float _vertical;

    [SerializeField] private EdgeCollider2D attackArea;
    // private string _currentAnim;


    private float _horizontal;

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
        predictPoints = Instantiate(predictPoints, transform.position, Quaternion.Euler(Vector3.zero));
    }

    // Start is called before the first frame update
    
    // Update is called once per frame
    private void Update()
    {
        var numPoint = predictPoints.transform.childCount;
        for (var i = 0; i < numPoint; i++)
        {
            predictPoints.transform.GetChild(i).transform.position = PointPositions(i * 0.1f);
        }
        if (IsDead) return;
        isGrounded = CheckGrounded();
        isGliding = !isGrounded && !isJumping;
        if (isGliding && Input.GetKeyDown(KeyCode.Space) && !isGlideDone)
        {
            Glide();
        }
        _horizontal = Input.GetAxisRaw("Horizontal");
        if (!isGrounded || isJumping || isAttack) return;
        // jump
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        // change run anim
        if (Mathf.Abs(_horizontal) > 0.1f && !isJumping)
        {
            ChangeAnim("run");
        }
        // attack
        if (Input.GetKeyDown(KeyCode.C)) Attack();
        // throw
        if (Input.GetKeyDown(KeyCode.V)) Throw();
        if (Input.GetMouseButtonDown(0)) Shoot();
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
        if (isGliding && isGlideDone)
        {
            rb.gravityScale = 0.0f;
            rb.velocity = new Vector2(rb.velocity.x, -1);
        }
        else
        {
            rb.gravityScale = 1f;
        }
        if (!isGrounded && !isGlideDone && rb.velocity.y <= 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }


        if (Mathf.Abs(_horizontal) != 0f)
        {
            isLeft = _horizontal > 0;
            rb.velocity = new Vector2(_horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, isLeft ? 0f : 180f, 0));
            if (isGrounded && !isJumping)
            {
                isGlideDone = false;
            }
        }

        else if (isGrounded && !isJumping)
        {
            isGlideDone = false;
            rb.velocity = Vector2.zero;
            ChangeAnim("idle");
        }
    }

    [SerializeField] private bool isLeft;

    public void OnSavePoint()
    {
        savePoint = transform.position;
    }

    protected override void OnInit()
    {
        base.OnInit();
        isAttack = false;
        transform.position = savePoint;
        isGlideDone = false;
        isGliding = false;
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
        Debug.DrawLine(position, position + Vector3.down * 1.3f, Color.red);
        var hit = Physics2D.Raycast(position, Vector2.down, 1.3f, groundLayer);
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
        var kunai = Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
        kunai.rb.velocity = transform.right * 5f;
    }

    public Vector3 mousePos;
    private void Shoot()
    {
        if (IsDead || !isGrounded || isJumping || isAttack) return;
        isAttack = true;
        ChangeAnim("throw");
        Invoke(nameof(ResetAttack), 0.25f);
        var bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
        bomb.rb.gravityScale = 1;
        var position = transform.TransformPoint(Vector3.zero);
        Debug.Log("Player" + position);
        var direction = new Vector3((mousePos.x - position.x)*-1,
            (mousePos.y - position.y)*-1,
            mousePos.z - position.z);
        direction = direction.normalized;
        Debug.Log("Dir" + direction);
        bomb.rb.velocity = direction * 20f / bomb.rb.mass;
        bomb.posList = new List<Vector3>();
        for (var i = 0; i < predictPoints.transform.childCount; i++)
        {
            bomb.posList.Add(predictPoints.transform.GetChild(i).transform.TransformPoint(Vector3.zero));
        }
    }

    public void Glide()
    {
        if (!isGliding || isGlideDone) return;
        isGlideDone = true;
        ChangeAnim("glide");
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
        if (isGrounded)
        {
            isGlideDone = false;
            ChangeAnim("Idle");
        }
    }

    public void SetMove(float horizontal)
    {
        _horizontal = horizontal;
    }
    
    private Vector2 PointPositions(float t )
    {
        var position = transform.TransformPoint(Vector3.zero);
        var direction = new Vector3((mousePos.x - position.x)*-1,
            (mousePos.y - position.y)*-1).normalized;
        var xPos = position.x + direction.x * 20f * t;
        var yPos = position.y + direction.y * 20f * t - 0.5f * 9.8f * t * t;
        return new Vector2(xPos, yPos);
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