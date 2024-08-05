namespace Voyage.InGame.Player
{
    using Rewired;
    using UnityEngine;

    internal sealed class PlayerComponent : MonoBehaviour
    {
        private Camera m_MainCamera;
        private Animator m_PlayerAnimator;
        private CapsuleCollider m_PlayerCollider;
        private Rigidbody m_PlayerRigidbody;
        private float m_WalkSpeed = 8f;
        private float m_RunSpeed = 12f;
        private Player m_Player;
        private float m_GroundDistance = 0.01f;
        private MotionState m_MotionState = MotionState.IDLE;
        private JumpState m_JumpState = JumpState.NONE;

        [SerializeField]
        private Transform m_ColliderTransform;
        [SerializeField]
        private LayerMask m_GroundMask;
        [SerializeField]
        private float m_JumpForce = 3f;

        private void Awake()
        {
            m_MainCamera = Camera.main;
            m_PlayerAnimator = GetComponentInChildren<Animator>();
            m_PlayerCollider = GetComponentInChildren<CapsuleCollider>();
            m_PlayerRigidbody = GetComponent<Rigidbody>();
            m_Player = ReInput.players.GetPlayer(0);
        }

        private void FixedUpdate()
        {
            var isGrounded = CheckOnGround();
            var input = Vector3.zero;
            if (m_Player.GetButton("Forward"))
            {
                input += Vector3.forward;
                m_MotionState = MotionState.WALK;
            }
            if (m_Player.GetButton("Left"))
            {
                input += Vector3.left;
                m_MotionState = MotionState.WALK;
            }
            if (m_Player.GetButton("Back"))
            {
                input += Vector3.back;
                m_MotionState = MotionState.WALK;
            }
            if (m_Player.GetButton("Right"))
            {
                input += Vector3.right;
                m_MotionState = MotionState.WALK;
            }
            if (m_Player.GetButtonDown("Jump"))
                if (isGrounded)
                    m_MotionState = MotionState.JUMP;
            if (m_Player.GetButton("Shift"))
            {
                input = input * 2;
                m_MotionState = MotionState.RUN;
            }

            switch (m_MotionState)
            {
                case MotionState.IDLE:
                    break;
                case MotionState.WALK:
                case MotionState.RUN:
                    ApplyMotion(input);
                    break;
                case MotionState.JUMP:
                    UpdateJumpMotion(isGrounded);
                    break;
                case MotionState.FALLING:
                    break;
            }
        }

        private void ApplyMotion(Vector3 input)
        {
            if (input.magnitude <= 0f)
            {
                if (m_PlayerRigidbody.velocity.magnitude == 0)
                {
                    m_MotionState = MotionState.IDLE;
                    m_PlayerAnimator.SetBool("IsWalking", false);
                }
                else if (m_PlayerRigidbody.velocity.magnitude < m_WalkSpeed)
                {
                    m_MotionState = MotionState.WALK;
                    m_PlayerAnimator.SetBool("IsRunning", false);
                }
                return;
            }

            var movementRight = Vector3.right;
            var movementForward = Vector3.forward;
            var groundNormal = Vector3.up;

            if (m_MainCamera != null)
            {
                // カメラに対して前と右の方向を取得
                var cameraRight = m_MainCamera.transform.right;
                var cameraForward = m_MainCamera.transform.forward;
                
                movementRight = ProjectOnPlane(cameraRight, groundNormal).normalized;
                movementForward = ProjectOnPlane(cameraForward, groundNormal).normalized;
            }

            var movement = movementRight * input.x + movementForward * input.z;

            Vector3 rotateTarget = new Vector3(movement.x, 0, movement.z);
            if (rotateTarget.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(rotateTarget);
                transform.rotation = Quaternion.Lerp(lookRotation, transform.rotation, 0.8f);
            }

            switch (m_MotionState)
            {
                case MotionState.WALK:
                    {
                        var currentSpeed = m_WalkSpeed - m_PlayerRigidbody.velocity.magnitude;
                        m_PlayerAnimator.SetBool("IsWalking", true);
                        m_PlayerAnimator.SetBool("IsRunning", false);
                        m_PlayerRigidbody.AddForce(movement * currentSpeed, ForceMode.Acceleration);
                    } 
                    break;
                case MotionState.RUN:
                    {
                        var currentSpeed = m_RunSpeed - m_PlayerRigidbody.velocity.magnitude;
                        m_PlayerAnimator.SetBool("IsRunning", true);
                        m_PlayerRigidbody.AddForce(movement * currentSpeed, ForceMode.Acceleration);
                    }
                    break;
            }
        }

        private Vector3 ProjectOnPlane(Vector3 vector, Vector3 normal)
        {
            return Vector3.Cross(normal, Vector3.Cross(vector, normal));
        }

        private void UpdateJumpMotion(bool isGrounded)
        {
            if (m_MotionState is not MotionState.JUMP)
                return;

            switch (m_JumpState)
            {
                case JumpState.NONE:
                    m_JumpState = JumpState.WAITING;
                    break;
                case JumpState.WAITING:
                    m_PlayerRigidbody.velocity += Vector3.up * 3.0f;
                    m_JumpState = JumpState.RISING;
                    break;
                case JumpState.RISING:
                    m_JumpState = JumpState.FALL;
                    break;
                case JumpState.FALL:
                    m_JumpState = JumpState.LANDING;
                    break;
                case JumpState.LANDING:
                    m_JumpState = JumpState.NONE;
                    m_MotionState = MotionState.IDLE;
                    break;
            }
            
            if (isGrounded)
                m_PlayerRigidbody.AddForce(new Vector3(0, m_JumpForce, 0));
        }

        private bool CheckOnGround()
        {
            var extent = Mathf.Max(0, m_PlayerCollider.height * 0.5f - m_PlayerCollider.radius);
            var origin = m_ColliderTransform.TransformPoint(m_PlayerCollider.center + Vector3.down * extent) + Vector3.up * m_GroundDistance;
            
            var sphereCastRay = new Ray(origin, Vector3.down);
            var raycastHit = Physics.SphereCast(sphereCastRay, m_PlayerCollider.radius, m_GroundDistance * 2f, m_GroundMask);

            return raycastHit;
        }
    }
}