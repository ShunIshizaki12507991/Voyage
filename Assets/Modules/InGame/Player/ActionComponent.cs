namespace Voyage.InGame.Player
{
    using Common;
    using UniRx;

    internal interface IActionComponent : IComponent
    {
        void UpdateMove();
    }

    internal sealed class ActionComponent : AbstractComponentBehaviour, IActionComponent, IUpdateTaskComponent
    {
        public bool IsEnable => m_IsEnable;
        private bool m_IsEnable;

        public override void Register(IComponentCollection owner)
        {
            base.Register(owner);
            owner.RegisterInterface<IActionComponent>(this).AddTo(this);
            owner.RegisterInterface<IUpdateTaskComponent>(this).AddTo(this);
        }

        public void SetActivate(bool active)
        {
            m_IsEnable = active;
        }

        void IUpdateTaskComponent.OnUpdate()
        {
        }

        public void UpdateMove()
        {
            if (!Owner.QueryInterface<ITransformAccessor>(out var transformAccessor))
                return;
            var t = transformAccessor.Transform;
        }
    }
}