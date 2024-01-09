using System;
using System.Collections;
using System.Collections.Generic;
using Test.Base;
using Test.Managers;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Test.Enemies.Base
{
    public class EnemyBase : MonoBehaviour
    {
        public event Action<EnemyBase> OnEnemyDeathEvent;

        public Action<Vector3> OnDuplicatorDeathEvent;

        [Header("Health")]
        [SerializeField] private float _maxHealth;

        [Space(4), Header("PlayerStats")]
        [SerializeField] private float _attackRange = 2;

        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;

        private Animator _animatorController;
        private NavMeshAgent _navMeshAgent;

        private float _lastAttackTime = 0;

        private bool _isDead;

        private Health _health;
        private Player.Player _player;

        public void Initialize(Player.Player player)
        {
            _player = player;

            _health = new Health(_maxHealth);

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animatorController = GetComponent<Animator>();

            _navMeshAgent.SetDestination(_player.transform.position);
        }

        public void SubstractHealth(float amount) => _health.Subtract(amount);

        public void Move()
        {
            if (_player == null)
            {
                return;
            }

            if (_isDead)
            {
                return;
            }

            if (_health.LessOrEqualZero())
            {
                Die();
                _navMeshAgent.isStopped = true;
                return;
            }

            var distance = Vector3.Distance(transform.position, _player.transform.position);

            if (distance <= _attackRange)
            {
                _navMeshAgent.isStopped = true;
                if (Time.time - _lastAttackTime > _attackSpeed)
                {
                    _lastAttackTime = Time.time;
                    _player.SubtractHealth(_damage);
                    _animatorController.SetTrigger("Attack");
                }
            }
            else
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_player.transform.position);
            }
            _animatorController.SetFloat("Speed", _navMeshAgent.speed);
        }

        protected virtual void Die()
        {
            OnEnemyDeathEvent?.Invoke(this);
            _isDead = true;
            _animatorController.SetTrigger("Die");
        }
    }
}