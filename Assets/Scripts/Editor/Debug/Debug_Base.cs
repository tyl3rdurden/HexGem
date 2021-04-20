using UnityEngine;

public interface Debuggable
{
    void SaveReferences();

    void InitAssets();
    
    void Layout();

    bool ValidateObject();
}

[System.Serializable]
public abstract class Debug_Base : Debuggable
{
    public abstract void SaveReferences();

    public abstract void Layout();

    public abstract bool ValidateObject();

    public abstract void InitAssets();
    
    protected abstract void LoadReferences();

    protected abstract void PropertyField();

    protected abstract void OnDataChange();
}
