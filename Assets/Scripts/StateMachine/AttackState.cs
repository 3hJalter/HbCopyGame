using UnityEngine;

namespace StateMachine
{
    public class AttackState : IState
    {
        private float _timer;
        public void OnEnter(Enemy enemy)
        {
            if (enemy.Target == null) return;
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
            enemy.StopMoving();
            enemy.Attack();
        }

        public void OnExecute(Enemy enemy)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1.5f)
            {
                enemy.ChangeState(new PatrolState());
            }
        }
        
        public void OnExit(Enemy enemy)
        {
            
        }

        
    }
}