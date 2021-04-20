///   Simple pooling for Unity.
///   Author: Martin "quill18" Glaude (quill18@quill18.com)
///   Extended: Simon "Draugor" Wagner (https://www.twitter.com/Draugor_/)
///   Latest Version: https://gist.github.com/Draugor/00f2a47e5f649945fe4466dea7697024
///   License: CC0 (http://creativecommons.org/publicdomain/zero/1.0/)
///
///   Additional Changes by Joon Ryul Lee 2021.01.30
///   Changed to 1 instance per prefab and as a Scriptable Object

using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Pool/BlockPool", fileName =  "BlockPool")]
public class SimplePool : ScriptableObject
{
    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private GameObject _prefab;
    
    [ShowInInspector, BoxGroup, ReadOnly] 
    private int UseCount;

    [ShowInInspector, BoxGroup, ReadOnly] 
    private int RemainingCount;

    private Stack<Block> _poolStack;
    private HashSet<int> MemberIDHashSet;
        
    private int _nextId = 1;

    private Vector3 baseScale = Vector3.one;

    public void Preload(int initialQty)
    {
        UseCount = 0;
        RemainingCount = 0;

        _poolStack = new Stack<Block>(initialQty);
        MemberIDHashSet = new HashSet<int>();

        _nextId = 1;
        
        for (int i = 0; i < initialQty; i++)
            _poolStack.Push(CreateNewInstance());

        RemainingCount = initialQty;
    }

    // Spawn an object from our pool
    public Block Spawn(Vector3 pos, bool setActive, Transform parent = null)
    {
        Block obj;
        if (_poolStack.Count == 0)
        {
            obj = CreateNewInstance(parent);
        }
        else
        {
            // Grab the last object in the inactive array
            obj = _poolStack.Pop();
            RemainingCount--;
            UseCount++;
        }
            
        obj.CachedTransform().SetParent(parent, false);
        obj.CachedTransform().localPosition = pos;
        obj.CachedTransform().localScale = baseScale;
        
        if (setActive)
            obj.gameObject.SetActive(true);
        
        return obj;
    }

    // Return an object to the inactive pool.
    public void Despawn(Block obj)
    {
        if (obj.gameObject.activeInHierarchy == false) return;
        
        obj.gameObject.SetActive(false);
        _poolStack.Push(obj);
        RemainingCount++;
        UseCount--;
    }

    private Block CreateNewInstance(Transform parent = null)
    {
        // We don't have an object in our pool, so we
        // instantiate a whole new object.
        var go = Instantiate(_prefab, parent);
        var obj = go.GetComponent<Block>();
        obj.name = _prefab.name + " (" + (_nextId++) + ")";

        // Add the unique GameObject ID to our MemberHashset so we know this GO belongs to us.
        MemberIDHashSet.Add(obj.GetInstanceID());
        hexGrid.GameObjectToBlockDic.Add(go, obj);
        
        return obj;
    }
}