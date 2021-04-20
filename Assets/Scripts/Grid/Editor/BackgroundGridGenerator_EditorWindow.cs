using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class GridGenerator_EditorWindow : OdinEditorWindow
{
    public enum GridType
    {
        Background,
        ForegroundFX,
        ParticleFX
    }
    
    [SerializeField] private GameObject HexPrefab;
    [SerializeField] private Transform BackgroundGridTransformParent;

    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private FXGrid fxGrid;

    [EnumToggleButtons] 
    [SerializeField] private GridType gridType;
    
    private float hexSpriteWidth;
    private float hexSpriteHeight;

    private bool isCachedTopLeftCorner;
    private Vector3 topLeftCorner;
    
    [MenuItem("GridGenerator/BackgroundGridGenerator")]
    private static void OpenWindow()
    {
        GetWindow<GridGenerator_EditorWindow>().Show();
    }
    
    [HorizontalGroup()]
    [Button(ButtonSizes.Large)]
    private void LoadHexGrid()
    {
        Debug.Log("HexLoad");

        isCachedTopLeftCorner = false;

        if (BackgroundGridTransformParent == null)
            BackgroundGridTransformParent = ((GameObject)(Selection.activeObject)).GetComponent<RectTransform>();

        SetHexSize();
        CreateGrid();
        
        PrefabUtility.RecordPrefabInstancePropertyModifications(BackgroundGridTransformParent.gameObject);
        
        EditorUtility.SetDirty(hexGrid);
        AssetDatabase.SaveAssets();
    }

    [HorizontalGroup()]
    [Button(ButtonSizes.Large)]
    private void DeleteHexGrid()
    {
        if (BackgroundGridTransformParent == null)
            BackgroundGridTransformParent = ((GameObject)(Selection.activeObject)).GetComponent<RectTransform>();
        
        while (BackgroundGridTransformParent.childCount != 0)
        {
            Transform child = BackgroundGridTransformParent.GetChild(0);
            DestroyImmediate(child.gameObject);
        }
    }
    
    void SetHexSize()
    {
        var hexSprite = HexPrefab.GetComponent<RectTransform>().rect;
        
        hexSpriteWidth = hexSprite.width;
        hexSpriteHeight = hexSprite.height;
    }
    
    void CreateGrid() //left to right, top to bottom 
    {
        for (int x = 0; x < HexGrid.Width; x++)
        {
            for (int y = 0; y < HexGrid.Height; y++)
            {
                GameObject hex = (GameObject)PrefabUtility.InstantiatePrefab(HexPrefab, BackgroundGridTransformParent);
                hex.ChangeName(x, y);          
                hex.transform.localPosition = HexGridToLocalPosition(x, y);
                hex.transform.localScale = Vector3.one;

                switch (gridType)
                {
                    case GridType.Background:
                        hexGrid.BlockPositions[x, y] = hex.transform.localPosition;
                        break;
                }

                hex.SetActive(true);
            }
        }
    }
    
    private Vector3 HexGridToLocalPosition(int GridX, int GridY)
    {
        Vector3 initPos = TopLeftCorner();
 
        float x = initPos.x + (GridX * hexSpriteWidth * .75f);
        
        float yOffset = 0;
        if (GridX.IsOdd())
            yOffset = hexSpriteHeight * .5f;
        
        float y = (initPos.y + yOffset) - (GridY * hexSpriteHeight);
        return new Vector3(x, y, 0);
    }
    
    Vector3 TopLeftCorner()
    {
        if (isCachedTopLeftCorner)
            return topLeftCorner;
            
        topLeftCorner = new Vector3(
            (-hexSpriteWidth * .75f) * ((HexGrid.Width / 2f) - .5f),
            (HexGrid.Height / 2f * hexSpriteHeight) - (hexSpriteHeight / 2) + 20, 
            0);
        
        //Debug.Log(topLeftCorner);

        isCachedTopLeftCorner = true;

        return topLeftCorner;
    }
}