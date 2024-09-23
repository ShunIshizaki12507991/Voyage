namespace Voyage.InGame.Player
{
    using Common;
    using UnityEngine;

    internal interface IPlayerCameraAccessor : IComponent
    {
        Camera PlayerCamera { get; }
    }
}