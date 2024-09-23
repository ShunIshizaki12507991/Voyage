namespace Voyage.InGame.Player
{
    using Common;
    using Rewired;
    using UnityEngine;

    internal interface IInputComponent : IComponent
    {
        bool HasJump();

        Vector3 GetMoveValue();
    }

    internal sealed class InputComponent : AbstractComponentBehaviour, IInputComponent, IUpdateTaskComponent
    {
        private Rewired.Player m_Player;

        private void Awake()
        {
            m_Player = ReInput.players.GetPlayer(0);
        }

        public override void Initialize()
        {
            base.Initialize();
            UpdateTaskRegistry.Register(this, UpdateGroup.Units);
        }

        public override void Register(IComponentCollection owner)
        {
            base.Register(owner);
            owner.RegisterInterface<IInputComponent>(this);
            owner.RegisterInterface<IUpdateTaskComponent>(this);
        }

        private bool m_IsEnable { get; set; }

        bool IUpdateTaskComponent.IsEnable => m_IsEnable;

        void IUpdateTaskComponent.SetActivate(bool active)
        {
            m_IsEnable = active;
        }

        void IUpdateTaskComponent.OnUpdate()
        {
            if (HasJump())
            {
                return;
            }

            var input = GetMoveValue();
            if (Owner.QueryInterface<IMovementComponent>(out var movementComponent))
                movementComponent.UpdateMove(input);
        }

        public bool HasJump() => m_Player.GetButtonDown("Jump");

        public Vector3 GetMoveValue()
        {
            var input = Vector3.zero;
            if (m_Player.GetButton("Forward"))
                input += Vector3.forward;
            if (m_Player.GetButton("Left"))
                input += Vector3.left;
            if (m_Player.GetButton("Back"))
                input += Vector3.back;
            if (m_Player.GetButton("Right"))
                input += Vector3.right;
            if (m_Player.GetButton("Shift"))
                input *= 2;

            return input;
        }
    }
}