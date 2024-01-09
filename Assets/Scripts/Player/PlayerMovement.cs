using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Test.Player
{
    public sealed class PlayerMovement
    {
        private Rigidbody _rigidbody;

        private float _speed;

        private float _horizantalInput;
        private float _verticalInput;

        private Vector3 _direction;

        public PlayerMovement(Rigidbody rigidbody, float speed)
        {
            _rigidbody = rigidbody;
            _speed = speed;
        }

        public float GetVelocity() => _rigidbody.velocity.magnitude;

        public void Update()
        {
            _horizantalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");

            _direction = new Vector3(_horizantalInput, 0, _verticalInput);
            _direction.Normalize();
        }

        public void FixedUpdate()
        {
            _rigidbody.velocity = _direction * _speed;

            if (_direction.normalized == Vector3.zero)
            {
                _rigidbody.velocity = Vector3.zero;
            }
        }
    }
}