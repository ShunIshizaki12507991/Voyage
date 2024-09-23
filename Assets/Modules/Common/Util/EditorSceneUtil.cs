#if UNITY_EDITOR
namespace Voyage.Common.Util
{
    using UnityEngine;

    public static class EditorSceneUtil
    {
        private static string ms_FirstSceneName;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            ms_FirstSceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
        }

        public static bool IsFirstScene(GameObject go) => ms_FirstSceneName == go.scene.name;
    }
}
#endif