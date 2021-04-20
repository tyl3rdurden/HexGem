// #if UNITY_EDITOR // not included in Editor folder due to being called from 
//
// using System;
// using System.IO;
// using Sirenix.OdinInspector;
// using Sirenix.OdinInspector.Editor;
// using UnityEditor;
// using UnityEngine;
//
// public class TweenCreator_EditorWindow : OdinEditorWindow
// {
//     public enum TweenType
//     {
//         Move,
//         Punch,
//         Color,
//     }
//
//     [EnumToggleButtons, OnValueChanged(nameof(UpdateTweenType))]
//     [SerializeField] private TweenType createTweenType;
//     
//     [SerializeField] private bool debugWindow;
//
//     [ShowIf("debugWindow"), InlineEditor(InlineEditorObjectFieldModes.Hidden)]
//     [SerializeField] private TweenCreator_TemplateDictionary templateContainer;
//
//     [SerializeField] private ITweenDataProcessor currentTweenData;
//
//     private static TweenCreator_EditorWindow instance;
//
//     private DOTweenPlayerSO so;
//     
//     string path = "Assets";
//
//     [MenuItem("Tweens/TweenCreator")]
//     public static void OpenWindow()
//     {
//         instance = GetWindow<TweenCreator_EditorWindow>();
//         instance.Show();
//         instance.UpdateTweenType();
//         
//         instance.path = "Assets";
//
//         if (Selection.activeObject != null && Selection.activeObject is DOTweenPlayerSO) //when opened via DoTweenPlayerSO's CreateTween
//         {
//             instance.so = Selection.activeObject as DOTweenPlayerSO;
//             instance.path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(instance.so));
//         }
//     }
//
//     private void UpdateTweenType()
//     {
//         currentTweenData = templateContainer.TweenDataTemplates[createTweenType];
//     }
//
//     [Title("Create")]
//     [Button(ButtonSizes.Large)]
//     private void CreateTween()
//     {
//         string typeString = currentTweenData.GetType().ToString();
//         TweenDataSO asset = Instantiate(currentTweenData);
//
//         string assetPath = $"{path}/{typeString}.asset";
//
//         int counter = 1;
//         while (AssetDatabase.LoadAssetAtPath<TweenDataSO>(assetPath) != null)
//         {
//             assetPath = $"{path}/{typeString}{counter++}.asset";
//         }
//             
//         AssetDatabase.CreateAsset(asset, assetPath);
//
//         if (instance.so != null)
//         {
//             int newLength = instance.so.TweenDatas.Length + 1;
//
//             Array.Resize(ref instance.so.TweenDatas, newLength);
//             instance.so.TweenDatas[instance.so.TweenDatas.Length - 1] = asset;
//
//             if (currentTweenData.GetType() == typeof(DOLocalRectMoveSO))
//             {
//                 AssetDatabase.RenameAsset(assetPath, instance.so.name.Replace("Anim", "_Move"));
//             }
//             else if (currentTweenData.GetType() == typeof(DOPunchSO))
//             {
//                 AssetDatabase.RenameAsset(assetPath, instance.so.name.Replace("Anim", "_Punch"));
//             }
//
//             Debug.Log($"Added new tween to {instance.so.name}");
//         }
//         else
//         {
//             Debug.Log($"Created new tween at {path}/{typeString}.asset");
//         }
//         
//         AssetDatabase.SaveAssets();
//     }
// }
//
// #endif