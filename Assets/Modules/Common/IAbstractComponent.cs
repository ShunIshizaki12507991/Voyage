namespace Voyage.Common
{
    using System;

    public interface IAbstractComponent : IDisposable
    {
        IComponentCollection Owner { get; }

        void Register(IComponentCollection owner);

        void Initialize();
    }
}