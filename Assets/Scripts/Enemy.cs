using System;
using StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Character target;
    
    public Character Target
    {
        get => target;
        set => target = value;
    }


    private IState _currentState;
    // Start is called before the first frame update

    private void Update()
    {   if (_currentState != null && !IsDead)
            _currentState?.OnExecute(this);
    }

    protected override void OnInit()
    {
        base.OnInit();
        DeActiveAttack();
        ChangeState(new IdleState());
    }

    protected override void OnDeSpawn()
    {
        base.OnDeSpawn();
        Destroy(gameObject);
        Destroy(healthBar);
    }

    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }

    public void ChangeState(IState newState)
    {
        _currentState?.OnExit(this);

        _currentState = newState;
        _currentState?.OnEnter(this);
    }

    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }

    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.2f);
    }

    public bool IsTargetInRange()
    {
        return target != null && 
               Vector2.Distance(target.transform.position, transform.position) < attackRange;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemyWall"))
        {
            ChangeDirection(!isRight);
        }
    }

    [SerializeField] private bool isRight = true;
    [SerializeField] private EdgeCollider2D attackArea;

    public void ChangeDirection(bool isEnemyRight)
    {
        isRight = isEnemyRight;
        transform.rotation = isEnemyRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

    public void SetTarget(Character getComponent)
    {
        if (IsDead) return;
        target = getComponent;
        if (IsTargetInRange()) ChangeState(new AttackState());
        else if (target) ChangeState(new PatrolState());
        else ChangeState(new IdleState());
    }
    
    private void ActiveAttack()
    {
        attackArea.gameObject.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.gameObject.SetActive(false);
    }
}