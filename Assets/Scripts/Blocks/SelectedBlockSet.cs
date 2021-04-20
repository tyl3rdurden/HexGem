using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Collection/SelectedBlockSet", fileName = "SelectedBlockSet")]
public class SelectedBlockSet : ScriptableObject
{
    public List<BlockCoord> CoordList = new List<BlockCoord>();
    public HashSet<Block> HashSet = new HashSet<Block>();

    public void Clear()
    {
        CoordList.Clear();
        HashSet.Clear();
    }
    
    public bool IsFirstSelection()
    {
        return CoordList.Count == 0;
    }
}