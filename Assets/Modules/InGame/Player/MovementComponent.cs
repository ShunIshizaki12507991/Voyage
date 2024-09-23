namespace Voyage.InGame.Player
{
    using Common;
    using UniRx;
    using UnityEngine;

    internal interface IMovementComponent : ITransformAccessor
    {
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        void UpdateMove(Vector3 input);
    }

    internal sealed class MovementComponent : AbstractComponent, IMovementComponent, IUpdateTaskComponent
    {
        public MovementComponent(Transform transform)
        {
            m_Transform = transform;
        }

        public override void Register(IComponentCollection owner)
        {
            base.Register(owner);
            owner.RegisterInterface<IMovementComponent>(this);
        }

        public Vector3 Position
        {
            get => m_Transform.position;
            private set => m_Transform.position = value;
        }

        public Quaternion Rotation
        {
            get => m_Transform.rotation;
            private set => m_Transform.rotation = value;
        }

        Transform ITransformAccessor.Transform => m_Transform;
        private Transform m_Transform;

        void IMovementComponent.UpdateMove(Vector3 input)
        {
            if (input.magnitude <= 0f)
                return;

            if (!Owner.QueryInterface<IPlayerCameraAccessor>(out var cameraAccessor))
                return;

            var camera = cameraAccessor.PlayerCamera;
            var movementRight = Vector3.right;
            var movementForward = Vector3.forward;
            var groundNormal = Vector3.up;

            if (camera != null)
            {
                // カメラに対して前と右の方向を取得
                var cameraRight = camera.transform.right;
                var cameraForward = camera.transform.forward;

                movementRight = ProjectOnPlane(cameraRight, groundNormal).normalized;
                movementForward = ProjectOnPlane(cameraForward, groundNormal).normalized;
            }

            var movement = movementRight * input.x + movementForward * input.z;

            var rotateTarget = new Vector3(movement.x, 0, movement.z);
            if (rotateTarget.magnitude > 0.1f)
            {
                var lookRotation = Quaternion.LookRotation(rotateTarget);
                Rotation = Quaternion.Lerp(lookRotation, Rotation, 0.8f);
            }

            var speed = 5f;
            Position += input * speed * Time.deltaTime;
        }

        private Vector3 ProjectOnPlane(Vector3 vector, Vector3 normal)
        {
            return Vector3.Cross(normal, Vector3.Cross(vector, normal));
        }

        bool IUpdateTaskComponent.IsEnable => m_IsEnable;
        private bool m_IsEnable;

        void IUpdateTaskComponent.SetActivate(bool active)
        {
            m_IsEnable = active;
        }

        void IUpdateTaskComponent.OnUpdate()
        {
        }
    }
}