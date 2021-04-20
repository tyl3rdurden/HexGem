using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private List<SOManager> managers;
    void Update()
    {
        if (gameStateManager.currentStateType != GameStateType.InGame) return;
        
        for (var index = 0; index < managers.Count; index++)
        {
            managers[index].OnUpdate();
        }
    }
}