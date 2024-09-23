namespace Voyage.Common
{
    using System;

    [Serializable]
    public abstract class AbstractComponent : IAbstractComponent
    {
        public IComponentCollection Owner { get; private set; }

        public virtual void Register(IComponentCollection owner)
        {
            Owner = owner;
            Owner.Register(this);
        }

        public virtual void Initialize()
        {
        }

        public virtual void Dispose()
        {
            Owner.Dispose();
        }
    }
}