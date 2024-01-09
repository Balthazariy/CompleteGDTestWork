using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Base
{
    public sealed class Health
    {
        private float _currentHealth;
        private float _maxHealth;

        public Health(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
        }

        public bool LessOrEqualZero() => _currentHealth <= 0;

        public void RestoreFull() => _currentHealth = _maxHealth;

        public float GetCurrent() => _currentHealth;

        public float GetMax() => _maxHealth;

        public void IncreaseMax(float amount) => _maxHealth += amount;

        public void UpdateMax(float amount) => _maxHealth = amount;

        public void Heal(float amount)
        {
            _currentHealth += amount;
            EqualMax();
        }

        public void Subtract(float amount)
        {
            _currentHealth -= amount;
            EqualMin();
        }

        private void EqualMax()
        {
            if (_currentHealth >= _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        }

        private void EqualMin()
        {
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
            }
        }
    }
}