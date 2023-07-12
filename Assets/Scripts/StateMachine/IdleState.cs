using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StateMachine
{
    public class IdleState : IState
    {
        private float _randomTime;
        private float _timer;

        public void OnEnter(Enemy enemy)
        {
            enemy.StopMoving();
            _timer = 0;
            _randomTime = Random.Range(2f, 4f);
        }

        public void OnExecute(Enemy enemy)
        {
            _timer += Time.deltaTime;
            if (_timer > _randomTime) enemy.ChangeState(new PatrolState());
        }

        public void OnExit(Enemy enemy)
        {
            // throw new NotImplementedException();
        }
    }
}