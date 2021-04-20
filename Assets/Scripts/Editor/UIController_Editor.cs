using DG.DOTweenEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(UIController), true)]
public class UIController_Editor : OdinEditor
{
    private bool DebugAnimation;
    public override void OnInspectorGUI()
    {
        UIController uiController = (UIController)target;

        DebugAnimation = EditorGUILayout.Toggle("DebugAnimation", DebugAnimation);

        if (DebugAnimation)
        {
            if (GUILayout.Button("ShowView()"))
            {
                Debug.Log($"{this.name} ShowView()");

                uiController.Editor_ShowView();

                PrepareAndPlayTweens(uiController.Editor_AnimIn());
            }

            if (GUILayout.Button("HideView()"))
            {
                Debug.Log($"{this.name} HideView()");

                uiController.Editor_HideView();
            
                PrepareAndPlayTweens(uiController.Editor_AnimOut());
            }
        }

        DrawDefaultInspector();
    }

    private void PrepareAndPlayTweens(DOTweenPlayerSO player)
    {
        UIController uiController = (UIController)target;

        foreach (var tween in player.Editor_ProcessedTweens())
        {
            DOTweenEditorPreview.PrepareTweenForPreview(tween.GetTween());
        }
            
        foreach (var tween in player.Editor_ProcessedTweens())
        {
            DOTweenEditorPreview.Start();
        }
    }
}
#endif