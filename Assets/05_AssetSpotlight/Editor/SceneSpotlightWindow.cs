namespace UP05
{
#if UNITY_EDITOR
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEditor.SceneManagement;

    public class SceneSpotlightWindow : AssetSpotlightWindow<SceneSpotlightWindow>
    {
        protected override string[] options => new string[] {
            "--s",
            "--h",
            "--c"
        };

        [MenuItem("Tools/Organizer/Scene/Spotlight %&SPACE", false, 0)]
        private static void Spotlight()
        {
            ShowWindow();
        }

        protected override void InitializeAssetList()
        {
            var scenes = AssetDatabase.FindAssets("t:Scene");
            assetList = new (string, string)[scenes.Length];
            for (int i = 0; i < scenes.Length; ++i)
            {
                assetList[i].path = AssetDatabase.GUIDToAssetPath(scenes[i]);
                assetList[i].name = Path.GetFileNameWithoutExtension(assetList[i].path);
            }
        }

        protected override void SelectAsset(string path, params string[] options)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            bool show = options.Contains(this.options[0]);
            bool hide = options.Contains(this.options[1]);
            bool close = options.Contains(this.options[2]);

            if ((hide || show) && close)
            {
                EditorUtility.DisplayDialog("Exception thrown", "Scene spotlight has wrong option(s)", "Ok");
                return;
            }

            window.Close();

            if (!show && (close || hide))
            {
                var scene = EditorSceneManager.GetSceneByPath(path);
                if (scene == null) return;
                EditorSceneManager.CloseScene(scene, close);
                return;
            }
            else if (show)
            {
                EditorSceneManager.OpenScene(path, hide ? OpenSceneMode.AdditiveWithoutLoading : OpenSceneMode.Additive);
                return;
            }

            EditorSceneManager.OpenScene(path);
        }
    }
#endif
}