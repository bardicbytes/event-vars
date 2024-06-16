// alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.EventVars.BigDemo
{
    [RequireComponent(typeof(Rigidbody))]
    public class FirstPersonMovement : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; } = default;
        [field: SerializeField] public Transform Camera { get; private set; } = default;

        [Space]
        [SerializeField] private FloatEventVar.Field _moveSpeed;
        [SerializeField] private FloatEventVar.Field _jumpPower;
        [SerializeField] private FloatEventVar.Field _mouseLookSpeed;

        [Space]
        [SerializeField] private float _groundStep = .1f;
        [SerializeField] private float _groundMaxAngle = 45f;

        [SerializeField] private float _minPitch = -60f;
        [SerializeField] private float _maxPitch = 60f;

        private Vector3 _startingPosition;
        private Quaternion _startingRotation;

        private bool _jump = false;
        private bool _interact = false;
        private Vector2 _inputDirection = Vector2.zero;
        private Vector2 _mouseInput = Vector2.zero;

        private bool _isGrounded = false;

        private List<Collider> _groundColliders = new List<Collider>();

        private bool _fpsModeDisabled = true;

        private float _currentPitch = 0f;

        private void Reset()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            _startingPosition = transform.position;
            _startingRotation = transform.rotation;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) SetCursorEnabled(true);
            if (Input.GetKeyDown(KeyCode.Tab)) ToggleCursor();

            RecordInput();
            TurnAndPitchCamera();

            if (transform.position.y < -5) Respawn();
        }

        private void FixedUpdate()
        {
            DoMovement();
            if (_fpsModeDisabled)
            {
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.angularVelocity = Vector3.zero;
            }
        }

        private void OnCollisionEnter(Collision collision) => HandleCollision(collision);

        private void OnCollisionStay(Collision collision) => HandleCollision(collision);

        private void OnCollisionExit(Collision collision)
        {
            if (!_isGrounded) return;
            RemoveGroundCollider(collision.collider);
        }

        public void ToggleCursor() => SetCursorEnabled(!_fpsModeDisabled);

        public void SetCursorEnabled(bool isActive)
        {
            _fpsModeDisabled = isActive;
            RefreshCursor();
        }

        public void RefreshCursor()
        {
            if (!_fpsModeDisabled)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }

            if (_fpsModeDisabled)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        private void TurnAndPitchCamera()
        {
            if (_fpsModeDisabled) return;

            transform.Rotate(new Vector3(0f, _mouseInput.x * _mouseLookSpeed * Time.deltaTime, 0f), Space.Self);

            _currentPitch -= _mouseInput.y * _mouseLookSpeed * Time.deltaTime;
            _currentPitch = Mathf.Clamp(_currentPitch, _minPitch, _maxPitch);
            Camera.transform.localEulerAngles = new Vector3(_currentPitch, 0, 0);
        }

        private void HandleCollision(Collision collision)
        {
            if (collision.contactCount < 0) return;

            // too high
            if (collision.contacts[0].point.y > transform.position.y + _groundStep) return;
            Collider other = collision.contacts[0].otherCollider;

            //already a tracked ground collider
            if (_groundColliders.Contains(other)) return;
            var angle = Vector3.Angle(collision.contacts[0].normal, Vector3.up);

            // too steep
            if (angle > _groundMaxAngle) return;

            _groundColliders.Add(other);

            _isGrounded = _groundColliders.Count > 0;
        }

        private void RemoveGroundCollider(Collider other)
        {
            if (!_groundColliders.Contains(other)) return;
            _groundColliders.Remove(other);
            _isGrounded = _groundColliders.Count > 0;
        }

        private void DoMovement()
        {
            if (_fpsModeDisabled) return;
            if (!_isGrounded) return;

            var force = _moveSpeed * new Vector3(_inputDirection.x, 0, _inputDirection.y);

            force = transform.TransformVector(force);
            force.y = Rigidbody.velocity.y;

            Rigidbody.velocity = force;
            if (_jump && _isGrounded) Rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
        }

        private void RecordInput()
        {
            // get WASD + Mouse input and save to instance variables
            _inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            _jump = Input.GetButton("Jump");
            _interact = Input.GetButton("Fire1");
        }
        public void Respawn()
        {
            transform.position = _startingPosition;
            transform.rotation = _startingRotation;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}