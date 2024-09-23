namespace Voyage.Common
{
    public interface IUpdateTaskComponent : IComponent
    {
        bool IsEnable { get; }
        void SetActivate(bool active);
        void OnUpdate();
    }

    // public sealed class UpdateTaskComponent : AbstractComponent, IUpdateTaskComponent
    // {
    //     private bool m_IsActive;
    //
    //     public override void Register(IComponentCollection owner)
    //     {
    //         base.Register(owner);
    //         owner.RegisterInterface<IUpdateTaskComponent>(this);
    //     }
    //
    //     bool IUpdateTaskComponent.IsEnable => m_IsActive;
    //
    //     public void SetActivate(bool active)
    //     {
    //         m_IsActive = active;
    //     }
    //
    //     public void OnUpdate()
    //     {
    //     }
    // }
}