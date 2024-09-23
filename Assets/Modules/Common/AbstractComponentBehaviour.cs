namespace Voyage.Common
{
    using UnityEngine;

    public abstract class AbstractComponentBehaviour : MonoBehaviour, IAbstractComponent
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
        }
    }
}