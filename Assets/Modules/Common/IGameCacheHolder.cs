namespace Voyage.Common
{
    using UnityEngine;

    public interface IGameCacheHolder : IComponent
    {
        GameObject GameObjectCache { get; }
    }
}