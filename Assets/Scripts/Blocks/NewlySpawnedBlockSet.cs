using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collection/NewlySpawnedBlockSet", fileName = "NewlySpawnedBlockSet")]
public class NewlySpawnedBlockSet : ScriptableObject
{
    public List<Block> NewBlockList = new List<Block>();
}