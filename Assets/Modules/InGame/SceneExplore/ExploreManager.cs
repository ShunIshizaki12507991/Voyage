namespace Voyage.InGame.SceneExplore
{
    using System.Collections.Generic;
    using System.Threading;
    using Common.InGame;
    using Common.Scene;
    using Common.Util;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public sealed class ExploreManager : MonoBehaviour, ISceneEntry<ExploreContext, ExploreResult>
    {
        [SerializeField]
        private GameObject[] m_ManagerPrefabs = System.Array.Empty<GameObject>();

        private GameObject[] m_Managers = System.Array.Empty<GameObject>();

#if UNITY_EDITOR
        private async UniTaskVoid Start()
        {
            if (EditorSceneUtil.IsFirstScene(this.gameObject))
                await StartExploreForEditor(this.GetCancellationTokenOnDestroy());
        }

        private async UniTask StartExploreForEditor(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ISceneEntry<ExploreContext, ExploreResult> entry = this;
            await entry.RunAsync(default, cancellationToken);
        }
#endif
        async UniTask<ExploreResult> ISceneEntry<ExploreContext, ExploreResult>.RunAsync(ExploreContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var managers = new List<GameObject>();
            foreach (var prefab in m_ManagerPrefabs)
            {
                var manager = Instantiate(prefab, null);
                managers.Add(manager);
                await manager.GetComponent<IExploreInitializer>().InitializeAsync(cancellationToken);
            }

            m_Managers = managers.ToArray();

            return default;
        } 
    }
}