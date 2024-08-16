using UnityEditor;
using UnityEditor.SceneManagement;

namespace Jamcelin.Editor
{
    public class EditorStarter
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            var state = EditorPrefs.GetBool("auto_boot_enabled", true);
            SetAutoBoot(state);
        }

        [MenuItem("Jamcelin/Auto Boot")]
        private static void AutoBoot()
        {
            var state = EditorPrefs.GetBool("auto_boot_enabled", true);
            EditorPrefs.SetBool("auto_boot_enabled", !state);
            SetAutoBoot(!state);
        }
        
        [MenuItem("Jamcelin/Auto Boot", true)]
        private static bool AutoBootValidate()
        {
            var state = EditorPrefs.GetBool("auto_boot_enabled", true);
            Menu.SetChecked("Jamcelin/Auto Boot", state);
            return true;
        }

        private static void SetAutoBoot(bool isAutoBoot)
        {
            if (isAutoBoot)
            {
                var scenePath = EditorBuildSettings.scenes[0].path;
                var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                EditorSceneManager.playModeStartScene = asset;
            }
            else
            {
                EditorSceneManager.playModeStartScene = null;
            }
        }
    }
}