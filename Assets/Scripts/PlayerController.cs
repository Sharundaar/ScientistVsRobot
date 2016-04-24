using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DEngine
{
    public class PlayerController : MonoBehaviour
    {

        public enum PlayerState
        {
            FALLING,
            IDLE,
            WALKING,
            JUMPING,
            DEAD
        }

        public delegate void PlayerStateChanged(object _sender, PlayerState _old, PlayerState _new);
        public event PlayerStateChanged StateChanged;

        public PlayerState State
        {
            get { return m_state; }
            private set
            {
                m_oldState = m_state;
                m_state = value;

                OnStateChanged();
            }
        }

        public Vector3 Speed
        {
            get { return m_speed; }
        }

        public int Health
        {
            get { return m_health; }
        }

        private PlayerState m_oldState = PlayerState.IDLE;
        private PlayerState m_state = PlayerState.IDLE;

        [SerializeField]
        private float DamageCooldown = 1.0f;
        private float m_damageTimer = 0;

        [SerializeField]
        private int MaxHealth = 4;

        [SerializeField]
        public float MaxSpeed = 20.0f;

        [SerializeField]
        public float MaxAcceleration = 1000.0f;

        [SerializeField]
        public float MaxRotationSpeed = 10.0f;

        [SerializeField]
        private float JumpForce = 5.0f;

        [SerializeField]
        [Range(0, 1)]
        private float AirControl = 0.1f;

        private Vector3 m_speed = Vector3.zero;
        private Vector3 m_targetSpeed = Vector3.zero;

        [SerializeField]
        private Vector3 m_targetUp = Vector3.up;

        [SerializeField]
        private Vector3 m_targetForward = Vector3.forward;
        private float m_targetRotationSpeed = 0;

        private int m_health;

        private CapsuleCollider m_capsule;
        private Rigidbody m_rigidbody;

        private bool m_grounded = false;
        private bool m_groundedDirty = false;
        private List<Collider> m_GroundColliders = new List<Collider>();

        private bool m_jump = false;

        private CameraControl m_camera;

        void Start()
        {
            m_capsule = GetComponent<CapsuleCollider>();
            m_rigidbody = GetComponent<Rigidbody>();
            m_targetForward = transform.forward;
            m_targetSpeed = Vector3.zero;
            m_camera = GetComponent<CameraControl>();

            Reset();
        }

        // Update is called once per frame
        void Update()
        {
            m_damageTimer += Time.deltaTime;
        }

        void FixedUpdate()
        {
            m_groundedDirty = true;

            switch (State)
            {
                case PlayerState.FALLING:
                    UpdateFallingState();
                    break;
                case PlayerState.IDLE:
                    UpdateIdleState();
                    break;
                case PlayerState.JUMPING:
                    UpdateJumpingState();
                    break;
                case PlayerState.WALKING:
                    UpdateWalkingState();
                    break;

                default:
                    break;
            }

            UpdateAnyState();
        }

        void UpdateJumpingState()
        {
            if (m_jump)
            {
                m_rigidbody.AddForce(transform.up * JumpForce, ForceMode.VelocityChange);
                m_jump = false;
            }
            else
            {
                UpdateSpeed(m_targetSpeed, AirControl * MaxAcceleration);
                ApplySpeed();

                if (Vector3.Dot(m_rigidbody.velocity, transform.up) <= 0)
                {
                    State = PlayerState.FALLING;
                }

                // Handle rotation
                {
                    Quaternion rotation = Quaternion.LookRotation(m_targetForward.normalized, m_targetUp.normalized);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, MaxRotationSpeed);
                }
            }

        }

        void UpdateWalkingState()
        {
            // Update position
            {
                UpdateSpeed(m_targetSpeed, MaxAcceleration);
                ApplySpeed();

            }

            // Handle rotation
            {
                Quaternion rotation = Quaternion.LookRotation(m_targetForward.normalized, m_targetUp.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, MaxRotationSpeed);
            }

            // grounded ?
            {
                if (!IsGrounded())
                    State = PlayerState.FALLING;

            }

            // Idling ?
            {
                if (m_speed.sqrMagnitude < 1e-10f)
                    State = PlayerState.IDLE;
            }

            // Jumping ?
            {
                if (m_jump)
                    State = PlayerState.JUMPING;
            }
        }

        void UpdateFallingState()
        {
            // grounded ?
            if (IsGrounded())
            {
                State = PlayerState.IDLE;
            }

            // Update position
            {
                UpdateSpeed(m_targetSpeed, AirControl * MaxAcceleration);
                ApplySpeed();
            }

            // Handle rotation
            {
                Quaternion rotation = Quaternion.LookRotation(m_targetForward.normalized, m_targetUp.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, MaxRotationSpeed);
            }
        }

        void UpdateIdleState()
        {
            // grounded ?
            if (IsGrounded())
            {
                if (m_targetSpeed.sqrMagnitude > 0)
                    State = PlayerState.WALKING;

                if (m_jump)
                    State = PlayerState.JUMPING;
            }
            else
            {
                State = PlayerState.FALLING;
            }

            // Handle rotation
            Quaternion rotation = Quaternion.LookRotation(m_targetForward.normalized, m_targetUp.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, MaxRotationSpeed);
        }

        void UpdateAnyState()
        {
            if (Health <= 0 && State != PlayerState.DEAD)
            {
                State = PlayerState.DEAD;

            }
        }

        private void UpdateSpeed(Vector3 _target, float _acceleration)
        {
            m_speed += (_target - m_speed).normalized * Mathf.Min((_target - m_speed).magnitude, _acceleration);
        }

        private void ApplySpeed()
        {
            Vector3 newPosition = transform.position + m_speed * Time.fixedDeltaTime;
            Vector3 direction = newPosition - transform.position;
            /*
            RaycastHit hit;
            if (Physics.CapsuleCast(transform.position + transform.up * (m_capsule.height / 2.0f - 0.1f), transform.position - transform.up * (m_capsule.height / 2.0f - 0.1f), m_capsule.radius - 0.1f, direction.normalized, out hit, direction.magnitude))
            {
                if (hit.collider.GetComponent<Rigidbody>() == null)
                {
                    m_speed -= hit.normal * Vector3.Dot(m_speed, hit.normal);
                    newPosition = transform.position + direction.normalized * hit.distance;
                }
            }
            */
            transform.position = newPosition;
        }

        private void OnStateChanged()
        {
            if (StateChanged != null)
                StateChanged(this, m_oldState, m_state);
        }

        public void Reset()
        {
            m_health = MaxHealth;
            State = PlayerState.IDLE;
            m_targetUp = Vector3.up;
        }

        public void Damage()
        {
            if (m_damageTimer >= DamageCooldown)
            {
                m_health--;
                if (m_health <= 0)
                    m_health = 0;
                m_damageTimer = 0;
            }
        }

        public void Jump()
        {
            if (State == PlayerState.IDLE || State == PlayerState.WALKING)
                m_jump = true;
        }

        public void MoveTo(Vector3 _direction, float _speed)
        {
            m_targetSpeed = _direction * _speed * MaxSpeed;
        }

        public void RotateToward(Vector3 _forward, float _speed)
        {
            m_targetForward = Vector3.ProjectOnPlane(_forward, m_targetUp).normalized;
            m_targetRotationSpeed = _speed * MaxRotationSpeed;
        }

        public bool IsGrounded()
        {
            if (!m_groundedDirty)
                return m_grounded;

            m_groundedDirty = false;
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 0.5f, -transform.up, out hit, 1.01f))
            {
                m_grounded = true;
            }
            else
            {
                m_grounded = false;
            }

            return m_grounded; // Mathf.Approximately(Vector3.Dot(m_rigidbody.velocity, transform.up), 0);
        }

        public void SetUpDirection(Vector3 _up)
        {
            m_targetUp = _up.normalized;
            m_targetForward = Vector3.ProjectOnPlane(m_targetForward, m_targetUp).normalized;
            if (m_targetForward.magnitude <= 0.5f) // we tried to put the up vector colinear to the forward vector
                m_targetForward = Vector3.Cross(transform.right, transform.up);
        }
    }
}