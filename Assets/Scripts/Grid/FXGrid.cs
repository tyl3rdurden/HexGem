using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/FXGrid", fileName = "FXGrid")]
public class FXGrid : ScriptableObject
{
    [TableMatrix(HorizontalTitle = "Width", VerticalTitle = "Height", Transpose = false), ShowInInspector, ReadOnly]
    public RectTransform[,] ForegroundFXs = new RectTransform[HexGrid.Width, HexGrid.Height];
    
    [TableMatrix(HorizontalTitle = "Width", VerticalTitle = "Height", Transpose = false), ShowInInspector, ReadOnly]
    public ParticleSystem[,] ParticleFXs = new ParticleSystem[HexGrid.Width, HexGrid.Height];
}