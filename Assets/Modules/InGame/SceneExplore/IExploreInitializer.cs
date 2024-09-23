namespace Voyage.InGame.SceneExplore
{
    using System.Threading;
    using Cysharp.Threading.Tasks;

    internal interface IExploreInitializer
    {
        UniTask InitializeAsync(CancellationToken cancellationToken);
    }
}