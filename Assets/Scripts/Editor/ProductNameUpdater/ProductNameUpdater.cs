using TMPro;
using UnityEditor;
using UnityEngine;

namespace Editor.ProductNameUpdater
{
    public class ProductNameUpdater : EditorWindow
    {
        private const string ToolName = nameof(ProductNameUpdater);

        private string _newProductName;
        private GameObject _splashScreenPrefab;

        [MenuItem("Tools/" + ToolName)]
        public static void ShowWindow() =>
                GetWindow<ProductNameUpdater>(ToolName);

        private void OnGUI()
        {
            GUILayout.Label("Update Product Name", EditorStyles.boldLabel);

            _newProductName = EditorGUILayout.TextField("New Product Name", _newProductName);

            if(GUILayout.Button("Update PlayerSettings Product Name"))
                UpdatePlayerSettingsProductName();

            _splashScreenPrefab = (GameObject)EditorGUILayout.ObjectField("SplashScreen Prefab", _splashScreenPrefab, typeof(GameObject), false);

            if(GUILayout.Button("Update SplashScreen Prefab"))
                UpdateSplashScreenPrefab();
        }

        private void UpdatePlayerSettingsProductName()
        {
            if(string.IsNullOrEmpty(_newProductName))
            {
                Debug.LogError("Product Name cannot be empty!");

                return;
            }

            PlayerSettings.productName = _newProductName;
            Debug.Log($"PlayerSettings.productName updated to: {_newProductName}");
        }

        private void UpdateSplashScreenPrefab()
        {
            if(!_splashScreenPrefab)
            {
                Debug.LogError("SplashScreen Prefab is not assigned!");

                return;
            }

            var textMeshPro = _splashScreenPrefab.GetComponentInChildren<TextMeshProUGUI>();

            if(!textMeshPro)
            {
                Debug.LogError("TextMeshProUGUI component not found in the prefab!");

                return;
            }

            textMeshPro.text = _newProductName;
            EditorUtility.SetDirty(_splashScreenPrefab);
            Debug.Log($"SplashScreen prefab updated with new product name: {_newProductName}");
        }
    }
}