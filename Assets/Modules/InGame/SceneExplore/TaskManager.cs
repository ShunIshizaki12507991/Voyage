namespace Voyage.InGame.SceneExplore
{
    using System.Threading;
    using Common;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public sealed class TaskManager : MonoBehaviour, IExploreInitializer
    {
        private bool m_HasInitialized;

        UniTask IExploreInitializer.InitializeAsync(CancellationToken cancellationToken)
        {
            m_HasInitialized = true;
            return default;
        }

        private void Update()
        {
            if (!m_HasInitialized)
                return;
            UpdateTaskRegistry.Run();
        }
    }
}