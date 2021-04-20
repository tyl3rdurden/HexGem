using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Set<T> : SerializedScriptableObject
{
    public List<T> Items = new List<T>();
}
