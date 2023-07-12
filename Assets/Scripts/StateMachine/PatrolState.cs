using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StateMachine
{
    public class PatrolState : IState
    {
        private float _randomTime;
        private float _timer;

        public void OnEnter(Enemy enemy)
        {
            _timer = 0;
            _randomTime = Random.Range(3f, 6f);
        }

        public void OnExecute(Enemy enemy)
        {
            _timer += Time.deltaTime;
            if (enemy.Target != null)
            {
                enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
                if (enemy.IsTargetInRange())
                {
                    enemy.ChangeState(new AttackState());
                } else enemy.Moving();
            }
            else
            {
                if (_timer < _randomTime) enemy.Moving();
                else enemy.ChangeState(new IdleState());
            }
        }

        public void OnExit(Enemy enemy)
        {
            // throw new NotImplementedException();
        }
    }
}