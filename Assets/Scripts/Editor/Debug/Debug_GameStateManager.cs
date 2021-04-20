using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public sealed class Debug_GameStateManager : Debug_Base
{
    [AssetsOnly]
    [SerializeField] private GameStateManager gameStateManager;
    private SerializedObject serializedGameStateManager;

    private const string PrefKey = "GameStateManager";
    
    PropertyTree myObjectTree;

    public Debug_GameStateManager()
    {
        LoadReferences();
        InitAssets();
    }

    public override void SaveReferences()
    {
        EditorPrefs.SetString(PrefKey, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(gameStateManager)));
    }

    public override void InitAssets()
    {
        if (gameStateManager != null)
            serializedGameStateManager = new SerializedObject(gameStateManager);
        
        if (myObjectTree == null)
        {
            myObjectTree = PropertyTree.Create(gameStateManager);
        }
    }
    
    public override void Layout()
    {
        serializedGameStateManager.Update();
        
        using (var verticalGroup = new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            EditorGUILayout.LabelField(PrefKey);
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                PropertyField();
                serializedGameStateManager.ApplyModifiedProperties();
                
                if (changeScope.changed)
                    OnDataChange();
            }
        }
    }

    public override bool ValidateObject()
    {
        if (gameStateManager == null)
        {
            Debug.LogError("gameStateManager Null");
            return false;
        }

        return true;
    }
    
    protected override void LoadReferences()
    {
        gameStateManager = 
            AssetDatabase.LoadAssetAtPath<GameStateManager>(AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(PrefKey)));
    }

    //Using Odin Enum Drawer (Toggle) instead of Unity Dropdown
    //http://www.sirenix.net/odininspector/faq/48/can-i-write-my-own-custom-editor-code-using-odin
    //https://odininspector.com/tutorials/how-to-create-custom-drawers-using-odin/how-to-use-the-propertytree
    protected override void PropertyField()
    {
        myObjectTree.BeginDraw(true);
        var someProp1 = myObjectTree.GetPropertyAtPath("defaultStateType");
        someProp1.Draw();
        myObjectTree.EndDraw();
    }

    protected override void OnDataChange()
    {
        Undo.RecordObject(gameStateManager, "modified gameStateManager");
        Debug.Log("modified gameStateManager");
    }
}