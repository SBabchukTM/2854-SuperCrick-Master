﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;

namespace Editor.AddressablesTools.CustomAnalyzeRules
{
    public class SceneToAssetNoUnityBuildInAnalyzeRule : CheckSceneDupeDependencies
    {
        public override string ruleName => "Scene to Addressable No Unity BuildIn";

        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings) =>
                base.RefreshAnalysis(settings)
                        .Where(x => !x.resultName.Contains("unity_builtin_extra"))
                        .ToList();
    }

    [InitializeOnLoad]
    internal class RegisterSceneToAssetNoUnityBuildInAnalyzeRule
    {
        static RegisterSceneToAssetNoUnityBuildInAnalyzeRule() =>
                AnalyzeSystem.RegisterNewRule<SceneToAssetNoUnityBuildInAnalyzeRule>();
    }
}