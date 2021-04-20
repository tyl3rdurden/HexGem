using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public enum GameStateType
{
    MainMenu = 0,
    Load, //Only happens once
    Prepare,
    Countdown,
    InGame,
    Bonus,
    Finish,
    DEBUG
}

[CreateAssetMenu(menuName = "Manager/GameStateManager", fileName =  "GameStateManager")]
public partial class GameStateManager : InitializableSerializedSO
{
    [EnumToggleButtons]
    [SerializeField] private GameStateType defaultStateType;
    
    [OnValueChanged("UpdateGameStateDic")]
    [SerializeField] private List<GameState> gameStateLists;
    
    [ReadOnly]
    public Dictionary<GameStateType, GameState> GameStateDic = new Dictionary<GameStateType, GameState>();
        
    [ShowInInspector, ReadOnly]
    public GameStateType currentStateType { private set; get; }

    [NonSerialized] public Action<GameStateType> OnChangeState;
    
    private GameState currentState;
    
    private bool isAlreadyLoaded = false;

    protected override void Init()
    {
        isAlreadyLoaded = false;
        currentStateType = defaultStateType;
        currentState = GameStateDic[currentStateType];
    }

    protected override void DeInit()
    {
        OnChangeState = null;
    }

    protected override void PostAwake()
    {
        ChangeState(currentStateType);
    }

    public void ChangeState(GameStateType newGameStateType)
    {
        ValidStateCheck(ref newGameStateType);
        
        currentState.ExitState();
        
        currentStateType = newGameStateType;
        currentState = GameStateDic[currentStateType];
        
        currentState.PreEnterState();

        OnChangeState?.Invoke(currentStateType);
        
        currentState.PostEnterState();
    }

    private void ValidStateCheck(ref GameStateType newGameStateType)
    {
        switch (newGameStateType)
        {
            case GameStateType.MainMenu:
                break;
            case GameStateType.Load:
                if (HasGameAlreadyBeenLoaded())
                    newGameStateType = GameStateType.Prepare;
                isAlreadyLoaded = true;
                break;
            case GameStateType.Prepare:
                if (GameNeedsToLoad())
                    newGameStateType = GameStateType.Load;
                break;
            case GameStateType.Countdown:
                break;
            case GameStateType.InGame:
                break;
            case GameStateType.Finish:
                break;
        }
        
        #region Local Functions
        
        bool HasGameAlreadyBeenLoaded()
        {
            return isAlreadyLoaded;
        }
        
        bool GameNeedsToLoad()
        {
            return isAlreadyLoaded == false;
        }
        
        #endregion
    }
}

#if UNITY_EDITOR
public partial class GameStateManager
{
    [UsedImplicitly] //used in updating gameStateLists through Editor when new gameState added
    private void UpdateGameStateDic()
    {
        foreach (var gameState in gameStateLists)
        {
            if (GameStateDic.ContainsKey(gameState.ThisState) == false)
                GameStateDic.Add(gameState.ThisState, gameState);
        }
    }
}
#endif