using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [ShowInInspector]
    public BlockCoord Coord
    {
        get => coord;
        set
        {
            coord = value;
            needsToUpdatePosition = true;
        }
    }    
    private BlockCoord coord;

    public bool NeedsToUpdatePosition => needsToUpdatePosition;
    private bool needsToUpdatePosition;
    
    public BlockType Type;

    private IBlockBehaviour blockBehaviour;
    
    [SerializeField] private Image image;
    
    [SerializeField] private BlockData blockData;

    private bool isTransformCached;
    private Transform cachedTransform;
    
    public Transform CachedTransform()
    {
        if (isTransformCached)
            return cachedTransform;

        cachedTransform = transform;
        isTransformCached = true;

        return cachedTransform;
    }
    
    public void SetBlockType(BlockType type)
    {
        Type = type;
        image.sprite = blockData.Sprite(Type);
        blockBehaviour = blockData.SetBehaviour(Type);
    }

    public void UpdatedPosition()
    {
        needsToUpdatePosition = false;
    }

    public bool IsValidSelection() 
    {
        return blockBehaviour.IsValidSelection(this);
    }

    public void SetAffectedBlocks()
    {
        blockBehaviour.SetAffectedBlocks();
    }
}