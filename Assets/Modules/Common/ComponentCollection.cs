namespace Voyage.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;

    public interface IComponentCollection : IDisposable
    {
        void Register(IAbstractComponent component);

        void Initialize();

        IDisposable RegisterInterface<T>(T component)
            where T : IComponent;

        bool QueryInterface<T>(out T component)
            where T : IComponent;
    }

    [Serializable]
    public class ComponentCollection : IComponentCollection
    {
        private readonly HashSet<IAbstractComponent> m_AbstractComponents = new(ReferenceEqualityComparer<IAbstractComponent>.Instance);
        private readonly HashSet<IComponent> m_Components = new(ReferenceEqualityComparer<IComponent>.Instance);
        private readonly Dictionary<Type, HashSet<IComponent>> m_ComponentDict = new();

        public void Register(IAbstractComponent component)
        {
            if (m_AbstractComponents.Add(component))
                component.Register(this);
        }

        public void Initialize()
        {
            foreach (var component in m_AbstractComponents)
                component.Initialize();
        }

        public IDisposable RegisterInterface<T>(T component)
            where T : IComponent
        {
            m_Components.Add(component);
            if (!m_ComponentDict.ContainsKey(typeof(T)))
                m_ComponentDict.Add(typeof(T), new HashSet<IComponent>(ReferenceEqualityComparer<IComponent>.Instance));
            m_ComponentDict[typeof(T)].Add(component);
            return Disposable.CreateWithState((this, component), static t => t.Item1.m_ComponentDict[typeof(T)].Remove(t.component));
        }

        public bool QueryInterface<T>(out T component)
            where T : IComponent
        {
            component = default;
            if (m_ComponentDict.ContainsKey(typeof(T)))
            {
                component = (T)m_ComponentDict[typeof(T)].First();
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            foreach (var component in m_AbstractComponents)
                component.Dispose();
            m_Components.Clear();
            m_ComponentDict.Clear();
        }
    }
}