using System;
using System.Collections.Generic;
using Test.Base;
using Test.Enemies;
using Test.Enemies.Base;
using Test.Managers;
using UnityEngine;
using Zenject;

namespace Test.Player
{
    public class Player : MonoBehaviour
    {
        public event Action OnPlayerDeathEvent;

        public event Action<bool> OnEnemyInAttackRangeEvent;

        public event Action<float> OnSuperAttackCooldownEvent;

        public event Action<bool> OnSuperAttackEvent;

        public event Action<float, float> OnPlayerHealthUpdate;

        [Header("Health")]
        [SerializeField] private float _maxHealth;

        [Space(4), Header("PlayerStats")]
        [SerializeField] private float _attackRange = 2;

        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _speed;
        [SerializeField] private float _healAmount;

        private bool _isDead;
        private bool _isEnemyOnAttackRange;

        private Animator _animatorController;
        private Rigidbody _rigidbody;

        private Health _health;
        private PlayerMovement _movement;

        private EnemySpawnerManager _enemySpawnerManager;

        private EnemyBase _closestEnemy;

        private float _superAttackCooldown = 2.0f;
        private float _currentSuperAttackCooldown;
        private bool _startSuperAttackCooldown;

        [Inject]
        public void Construct(EnemySpawnerManager enemySpawnerManager)
        {
            _enemySpawnerManager = enemySpawnerManager;

            _animatorController = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();

            _health = new Health(_maxHealth);
            _movement = new PlayerMovement(_rigidbody, _speed);

            OnPlayerHealthUpdate?.Invoke(_health.GetCurrent(), _health.GetMax());
        }

        private bool IsDied() => _health.LessOrEqualZero();

        public void SubtractHealth(float amount)
        {
            _health.Subtract(amount);
            OnPlayerHealthUpdate?.Invoke(_health.GetCurrent(), _health.GetMax());
        }

        public void Heal()
        {
            _health.Heal(_healAmount);
            OnPlayerHealthUpdate?.Invoke(_health.GetCurrent(), _health.GetMax());
        }

        private void Update()
        {
            if (_isDead)
            {
                return;
            }

            if (IsDied())
            {
                Die();
                return;
            }

            _animatorController.SetFloat("Speed", _movement.GetVelocity());
            _movement.Update();

            var enemies = _enemySpawnerManager.GetList();

            _closestEnemy = null;
            _isEnemyOnAttackRange = false;

            if (_startSuperAttackCooldown)
            {
                _currentSuperAttackCooldown -= Time.deltaTime;
                OnSuperAttackCooldownEvent?.Invoke(_currentSuperAttackCooldown);

                if (_currentSuperAttackCooldown <= 0)
                {
                    _currentSuperAttackCooldown = _superAttackCooldown;
                    OnSuperAttackCooldownEvent?.Invoke(_currentSuperAttackCooldown);

                    _startSuperAttackCooldown = false;
                }
            }

            SetClosestEnemy(enemies);

            CheckDistanceToEnemy();
        }

        private void CheckDistanceToEnemy()
        {
            if (_closestEnemy != null)
            {
                var distance = Vector3.Distance(transform.position, _closestEnemy.transform.position);
                if (distance <= _attackRange)
                {
                    _isEnemyOnAttackRange = true;
                    OnEnemyInAttackRangeEvent?.Invoke(true);
                }
                else
                {
                    OnEnemyInAttackRangeEvent?.Invoke(false);
                }
            }
        }

        private void SetClosestEnemy(List<EnemyBase> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemie = enemies[i];
                if (enemie == null)
                {
                    continue;
                }

                if (_closestEnemy == null)
                {
                    _closestEnemy = enemie;
                    continue;
                }

                var distance = Vector3.Distance(transform.position, enemie.transform.position);
                var closestDistance = Vector3.Distance(transform.position, _closestEnemy.transform.position);

                if (distance < closestDistance)
                {
                    _closestEnemy = enemie;
                }
            }
        }

        private void FixedUpdate()
        {
            if (_isDead)
            {
                return;
            }

            _movement.FixedUpdate();
        }

        private void Die()
        {
            _isDead = true;
            _animatorController.SetTrigger("Die");
            OnPlayerDeathEvent?.Invoke();
        }

        public void Attack()
        {
            if (AnimatorIsEnd("sword attack"))
            {
                _animatorController.SetTrigger("Attack");

                if (_isEnemyOnAttackRange)
                {
                    transform.transform.rotation = Quaternion.LookRotation(_closestEnemy.transform.position - transform.position);

                    _closestEnemy.SubstractHealth(_damage);
                }
            }
        }

        public void SuperAttack()
        {
            if (!AnimatorIsEnd("sword double attack"))
            {
                return;
            }

            if (!_isEnemyOnAttackRange)
            {
                return;
            }

            if (_startSuperAttackCooldown)
            {
                return;
            }

            _animatorController.SetTrigger("DoubleAttack");

            transform.transform.rotation = Quaternion.LookRotation(_closestEnemy.transform.position - transform.position);

            _closestEnemy.SubstractHealth(_damage * 2);

            _currentSuperAttackCooldown = _superAttackCooldown;

            _startSuperAttackCooldown = true;
        }

        private bool AnimatorIsEnd(string animationName)
        {
            return !_animatorController.GetCurrentAnimatorStateInfo(0).IsName(animationName) && !_animatorController.IsInTransition(0);
        }
    }
}