using UnityEditor;
namespace KingdomOfNight
{
    public partial class SceneLoader
    {
#if UNITY_EDITOR
        [MenuItem("Scenes/Test")]
        public static void LoadTest() { OpenScene("Assets/scenes/Test.unity"); }
        [MenuItem("Scenes/_MainScene")]
        public static void Load_MainScene() { OpenScene("Assets/scenes/_MainScene.unity"); }
#endif
    }
}