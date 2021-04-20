using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockRaycaster : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster raycaster;
    
    PointerEventData pointerEventData = new PointerEventData(null);
    List<RaycastResult> results = new List<RaycastResult>(2);
    
    private GameObject currentBlock;
    
    public bool RaycastHitBlock(Vector2 position)
    {
        results.Clear();
        
        pointerEventData.position = position;
        raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            currentBlock = results[0].gameObject;
            return true;
        }

        return false;
    }

    public GameObject CurrentBlock()
    {
        return currentBlock;
    }
}