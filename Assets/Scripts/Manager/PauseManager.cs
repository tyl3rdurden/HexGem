using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/PauseManager", fileName =  "PauseManager")]
public class PauseManager : InitializableSO
{
    public Action<bool> OnPauseState;

    [ShowInInspector]
    public bool isGamePaused { get; private set; }

    protected override void Init()
    {
        isGamePaused = false;
    }

    protected override void DeInit()
    {
        OnPauseState = null;
    }

    public void OnPauseGame(bool pauseGame)
    {
        isGamePaused = pauseGame;

        if (pauseGame)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}