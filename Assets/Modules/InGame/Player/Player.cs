namespace Voyage.InGame.Player
{
    using Common;
    using UniRx;
    using UnityEngine;

    public sealed class Player : MonoBehaviour, IGameCacheHolder, ITransformAccessor, IPlayerCameraAccessor
    {
        private GameObject m_GameObjectCache;
        private Transform m_TransformCache;
        private Camera m_MainCamera;
        private IComponentCollection m_ComponentCollection;

        public GameObject GameObjectCache => m_GameObjectCache;
        public Transform Transform => m_TransformCache;
        public Camera PlayerCamera => m_MainCamera;

        private void Awake()
        {
            m_GameObjectCache = gameObject;
            m_TransformCache = transform;
            m_MainCamera = Camera.main;

            m_ComponentCollection = new ComponentCollection();
            foreach (var component in GetComponentsInChildren<IAbstractComponent>())
                component.Register(m_ComponentCollection);
            m_ComponentCollection.RegisterInterface<IGameCacheHolder>(this).AddTo(this);
            m_ComponentCollection.RegisterInterface<ITransformAccessor>(this).AddTo(this);
            m_ComponentCollection.RegisterInterface<IPlayerCameraAccessor>(this).AddTo(this);
            m_ComponentCollection.Register(new MovementComponent(m_TransformCache));
            m_ComponentCollection.Initialize();
        }
    }
}