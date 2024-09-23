namespace Voyage.Common
{
    using UnityEngine;

    public interface ITransformAccessor : IComponent
    {
        public Transform Transform { get; }
    }
}