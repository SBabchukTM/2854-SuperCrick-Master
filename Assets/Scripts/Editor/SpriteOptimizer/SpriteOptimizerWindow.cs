using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.SpriteOptimizer
{
    public class SpriteOptimizerWindow : EditorWindow
    {
        private const string BackgroundCategory = "Background";
        private const string UICategory = "UI";
        private const string CharacterCategory = "Character";
        private const string PropsCategory = "Props";
        private const string ButtonsCategory = "Buttons";
        private const string LogoCategory = "Logo";
        private const string PlayerCategory = "Player";
        private const string SpriteOptimizer = "Sprite Optimizer";
        private const int MinTextureSize = 32;
        private const int MaxTextureSize = 16384;

        private static readonly List<string> Categories = new();

        private static readonly Dictionary<string, int> MaxSizes = new()
        {
            {
                BackgroundCategory, 2048
            },
            {
                CharacterCategory, 512
            },
            {
                UICategory, 256
            },
            {
                PropsCategory, 256
            },
            {
                ButtonsCategory, 512
            },
            {
                PlayerCategory, 512
            },
            {
                LogoCategory, 1024
            }
        };

        private readonly List<string> _selectedCategories = new();

        private static SpriteOptimizerWindow _spriteOptimizerWindow;

        private Vector2 _scrollPosition;
        private string _newCategoryName = "";
        private int _newCategorySize = 256;
        private GUIStyle _labelStyle;
        private GUIStyle _buttonStyle;

        private void OnEnable() =>
                AddCategories();

        private void OnGUI()
        {
            InitializeStyles();

            EditorGUILayout.BeginVertical();

            {
                CreateSpriteOptimizerLabel();

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));

                {
                    DrawCategories();
                    DrawNewCategorySection();
                    DrawCategorySelection();
                }

                EditorGUILayout.EndScrollView();

                DrawBottomButtons();
            }

            EditorGUILayout.EndVertical();
        }

        private void InitializeStyles()
        {
            _buttonStyle ??= new(GUI.skin.button)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                padding = new(20, 20, 10, 10),
                margin = new(10, 10, 10, 10),
                fixedHeight = 40,
                fixedWidth = 250,
                normal =
                {
                    textColor = new(0.9f, 0.9f, 0.9f)
                },
                hover =
                {
                    textColor = new(0.9f, 0.9f, 0.9f)
                },
                active =
                {
                    textColor = new(0.9f, 0.9f, 0.9f)
                }
            };

            _labelStyle ??= new(EditorStyles.label)
            {
                fontSize = 50,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                richText = true,
                wordWrap = true,
                normal =
                {
                    textColor = Color.white
                }
            };
        }

        private void DrawCategories()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Default Categories", EditorStyles.boldLabel);

            foreach (var category in Categories.ToList())
            {
                EditorGUILayout.BeginHorizontal();

                {
                    EditorGUILayout.LabelField(category, GUILayout.Width(150));

                    var newSize = EditorGUILayout.IntField(MaxSizes[category], GUILayout.Width(100));

                    if(MaxSizes != null && newSize != MaxSizes[category])
                        MaxSizes[category] = newSize;

                    if(GUILayout.Button("Remove", GUILayout.Width(100)))
                        RemoveCategory(category);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawNewCategorySection()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add New Category", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            {
                _newCategoryName = EditorGUILayout.TextField("Category Name", _newCategoryName);
                _newCategorySize = EditorGUILayout.IntField("Max Size", _newCategorySize);

                if(GUILayout.Button("Add Category", GUILayout.Width(150)))
                {
                    AddNewCategory(_newCategoryName, _newCategorySize);
                    _newCategoryName = "";
                    _newCategorySize = 256;
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawCategorySelection()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Optimize by Category", EditorStyles.boldLabel);

            foreach (var category in Categories)
            {
                var isSelected = _selectedCategories.Contains(category);
                var newSelection = EditorGUILayout.ToggleLeft(category, isSelected);

                if(newSelection == isSelected)
                    continue;

                if(newSelection)
                    _selectedCategories.Add(category);
                else
                    _selectedCategories.Remove(category);
            }

            if(GUILayout.Button("Optimize Selected Categories", _buttonStyle))
                OptimizeSelectedCategories();
        }

        private void DrawBottomButtons()
        {
            EditorGUILayout.BeginHorizontal();

            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical();

                {
                    if(GUILayout.Button("Optimize Selected Sprites", _buttonStyle))
                        OptimizeSelectedSprites();

                    if(GUILayout.Button("Optimize All Project Sprites", _buttonStyle))
                        OptimizeAllSprites();
                }

                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }

            EditorGUILayout.EndHorizontal();
        }

        public static void InitWindow()
        {
            _spriteOptimizerWindow = GetWindow<SpriteOptimizerWindow>(SpriteOptimizer);
            _spriteOptimizerWindow.minSize = new(400, 500);
            _spriteOptimizerWindow.Show();
        }

        private void CreateSpriteOptimizerLabel()
        {
            EditorGUILayout.BeginHorizontal();

            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(SpriteOptimizer, _labelStyle);
                GUILayout.FlexibleSpace();
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void AddCategories()
        {
            Categories.Clear();

            foreach (var item in MaxSizes)
                Categories.Add(item.Key);
        }

        private static void AddNewCategory(string categoryName, int maxSize)
        {
            if(!string.IsNullOrEmpty(categoryName) && MaxSizes.TryAdd(categoryName, maxSize))
            {
                Categories.Add(categoryName);
            }
            else
                Debug.LogWarning("Category name is empty or already exists.");
        }

        private void RemoveCategory(string category)
        {
            if(MaxSizes.ContainsKey(category))
            {
                MaxSizes.Remove(category);
                Categories.Remove(category);
                Debug.Log($"Category '{category}' removed.");
            }
            else
                Debug.LogWarning($"Category '{category}' not found.");
        }

        private static void OptimizeSelectedSprites()
        {
            var selectedAssets = Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets);

            OptimizeSprites(selectedAssets.Cast<Texture2D>()
                    .ToArray());
        }

        private static void OptimizeAllSprites()
        {
            var guids = AssetDatabase.FindAssets("t:texture2D", null);
            var allTextures = new List<Texture2D>();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                if(texture != null)
                    allTextures.Add(texture);
            }

            OptimizeSprites(allTextures.ToArray());
        }

        private void OptimizeSelectedCategories()
        {
            if(_selectedCategories.Count == 0)
            {
                Debug.LogWarning("No categories selected.");

                return;
            }

            var guids = AssetDatabase.FindAssets("t:texture2D", null);
            var texturesInCategories = new List<Texture2D>();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                foreach (var category in _selectedCategories)
                    if(path.ToLower()
                       .Contains(category.ToLower()))
                    {
                        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                        if(texture != null)
                            texturesInCategories.Add(texture);
                    }
            }

            if(texturesInCategories.Count > 0)
            {
                OptimizeSprites(texturesInCategories.ToArray());
                Debug.Log($"Optimized {texturesInCategories.Count} sprites in selected categories.");
            }
            else
                Debug.LogWarning("No textures found for selected categories.");
        }

        private static void OptimizeSprites(IEnumerable<Texture2D> textures)
        {
            foreach (var texture in textures)
            {
                var importer = SetTextureData(texture);

                if(importer == null)
                    continue;

                var maxTextureSize = SetMaxSize(texture);

                SetTextureSettings(importer, maxTextureSize);

                EditorUtility.SetDirty(texture);
                importer.SaveAndReimport();
            }

            AssetDatabase.Refresh();
            Debug.Log("Sprite optimization completed!");
        }

        private static void SetTextureSettings(TextureImporter importer, int maxTextureSize)
        {
            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);

            var platformSettings = importer.GetDefaultPlatformTextureSettings();
            platformSettings.maxTextureSize = maxTextureSize;

            importer.SetPlatformTextureSettings(platformSettings);
        }

        private static int SetMaxSize(Texture2D texture)
        {
            var determineMaxSize = DetermineMaxSize(texture);
            determineMaxSize = Mathf.Clamp(determineMaxSize, MinTextureSize, MaxTextureSize);
            determineMaxSize = Mathf.ClosestPowerOfTwo(determineMaxSize);

            return determineMaxSize;
        }

        private static TextureImporter SetTextureData(Texture2D texture)
        {
            var path = AssetDatabase.GetAssetPath(texture);
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;

            return importer;
        }

        private static int DetermineMaxSize(Object texture)
        {
            var path = AssetDatabase.GetAssetPath(texture)
                    .ToLower();

            var category = GetCategory(path);
            var maxAllowedSize = MaxSizes[category];

            return maxAllowedSize;
        }

        private static string GetCategory(string path)
        {
            var isHasCategory = Categories.Count > 0 && Categories != null;

            if(isHasCategory)
            {
                foreach (var category in Categories)
                {
                    if(category.Contains(UICategory))
                        continue;

                    if(path.Contains(category.ToLower()))
                        return category;
                }
            }
            else
            {
                throw new($"Category is null or empty. Categories count: {Categories.Count}, check for null {Categories}");
            }

            Debug.LogWarning($"{path} does not contains any category.\n So it was set {UICategory}");

            return UICategory;
        }
    }
}