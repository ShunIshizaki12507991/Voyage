namespace Voyage.Common
{
    using UnityEngine;

    public interface IMovementComponent : IComponent
    {
    }

    public sealed class MovementComponent : AbstractComponentBehaviour, IMovementComponent
    {
        [SerializeField]
        private float m_WalkSpeed;

        public Vector3 Postion
        {
            get => m_Transform.position;
            private set => m_Transform.position = value;
        }

        public Quaternion Rotation
        {
            get => m_Transform.rotation;
            private set => m_Transform.rotation = value;
        }

        private Transform m_Transform;
    }
}