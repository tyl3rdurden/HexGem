using Sirenix.OdinInspector;
using UnityEngine;

public enum BlockType
{
    Red = 0, 
    Green,
    Blue,
    Yellow,
    Pink,
    Item_Start,
    Item_Vertical,
    Item_Diagonal,
    Item_Circular
}

public enum BlockBehaviourType
{
    Default,
    Vertical,
    Diagonal,
    Circular
}

[InlineEditor]
[CreateAssetMenu(menuName = "Data/BlockData", fileName = "BlockData")]
public class BlockData : ScriptableObject
{
    [SerializeField] private SpriteSet spriteSet;
    [SerializeField] private BlockBehaviourSet blockBehaviourSet;
    
    public Sprite Sprite(BlockType type)
    {
        return spriteSet.Items[(int)type];
    }

    public IBlockBehaviour SetBehaviour(BlockType type)
    {
        if (type < BlockType.Item_Start)
            return blockBehaviourSet.Items[(int) BlockBehaviourType.Default];
        
        switch (type)
        {
            case BlockType.Item_Vertical:
                return blockBehaviourSet.Items[(int) BlockBehaviourType.Vertical];
            case BlockType.Item_Diagonal:
                return blockBehaviourSet.Items[(int) BlockBehaviourType.Diagonal];
            case BlockType.Item_Circular:
                return blockBehaviourSet.Items[(int) BlockBehaviourType.Circular];
        }
        
        return blockBehaviourSet.Items[(int) BlockBehaviourType.Default];
    }
}
