namespace Voyage.Common.Scene
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine.AddressableAssets;
    using UnityEngine.SceneManagement;

    public interface ISceneContext
    {
    }

    public interface ISceneResult<TContext>
        where TContext : ISceneContext
    {
    }

    public interface ISceneEntry<TContext, TResult>
        where TContext : ISceneContext
        where TResult : ISceneResult<TContext>
    {
        UniTask<TResult> RunAsync(TContext context, CancellationToken cancellationToken);
    }
    
    public static class SceneEntry
    {
        public static async UniTask<TResult> RunAsync<TContext, TResult>(string address, TContext context, CancellationToken cancellationToken)
            where TContext : ISceneContext
            where TResult : ISceneResult<TContext>
        {
            var handle = Addressables.LoadSceneAsync(address, LoadSceneMode.Additive, true);
            try
            {
                var scene = await handle.WithCancellation(cancellationToken);
                var entry = scene.Scene.ExtractSceneEntry<TContext, TResult>();
                return await entry.RunAsync(context, cancellationToken);
            }
            finally
            {
                await Addressables.UnloadSceneAsync(handle);
            }
        }

        private static ISceneEntry<TContext, TResult> ExtractSceneEntry<TContext, TResult>(this Scene scene)
            where TContext : ISceneContext
            where TResult : ISceneResult<TContext>
        {
            var entries = new List<ISceneEntry<TContext, TResult>>();
            foreach (var go in scene.GetRootGameObjects())
                entries.AddRange(go.GetComponents<ISceneEntry<TContext, TResult>>());
            if (entries.Count > 1)
                throw new InvalidOperationException("This scene has multiple ISceneEntry's.");
            if (entries.Count == 0)
                throw new InvalidOperationException("This scene has no ISceneEntry's.");
            return entries[0];
        }
    }
}