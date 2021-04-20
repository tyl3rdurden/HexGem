using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PreMadeGrid", fileName = "PreMadeGrid")]
public partial class PreMadeGrid : SerializedScriptableObject
{
    [TableMatrix(HorizontalTitle = "Width", VerticalTitle = "Height", Transpose = false, 
         DrawElementMethod = "DrawBlockCell", SquareCells = true, ResizableColumns = false), ShowInInspector]
    public BlockType[,] Blocks = new BlockType[HexGrid.Width, HexGrid.Height];
}

#if UNITY_EDITOR
public partial class PreMadeGrid
{
    [Header("Editor Only")]
    [SerializeField] private BlockData blockData;
    
    private BlockType DrawBlockCell(Rect rect, BlockType block)
    {
        block = (BlockType) Sirenix.Utilities.Editor.SirenixEditorFields.EnumDropdown(rect.Padding(4f), (GUIContent) null, (BlockType) block,
            null);
        UnityEditor.EditorGUI.DrawPreviewTexture(rect.Padding(1), BlockColorTexture(block));
        return block;
    }

    private Texture2D BlockColorTexture(BlockType type)
    {
        string path = $"Assets/Resources/Atlas/Block/{blockData.Sprite(type).name}.png";
        
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }
}
#endif