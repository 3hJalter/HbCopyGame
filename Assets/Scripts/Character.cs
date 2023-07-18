using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected HealthBar healthBar;
    [FormerlySerializedAs("_hp")] [SerializeField] private float hp;
    [SerializeField] private CombatText combatTextPrefab;
    private string _currentAnim;
    protected bool IsDead => hp <= 0;

    private void Start()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, transform);
    }

    protected virtual void OnDeSpawn()
    {
        
    }

    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDeSpawn), 1.5f);
    }
    
    protected void ChangeAnim(string animName)
    {
        if (_currentAnim == animName) return;
        animator.ResetTrigger(animName);
        _currentAnim = animName;
        animator.SetTrigger(_currentAnim);
    }

    public void OnHit(float damage) 
    {
        if (!(hp >= 0)) return;
        hp -= damage;
        healthBar.SetNewHp(hp);
        Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        if (!(hp <= 0)) return;
        healthBar.SetNewHp(0);
        OnDeath();
    }

    
}
