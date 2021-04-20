using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GeneralUtils
{
    public static bool IsItem(this Block block)
    {
        return block.Type > BlockType.Item_Start;
    }
    
    public static void ChangeName(this Block go, int x, int y)
    {
        go.name = $"W {x}, H {y}";
    }
    
    public static void ChangeName(this GameObject go, int x, int y)
    {
        go.name = $"W {x}, H {y}";
    }
}