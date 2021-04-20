using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/ScoreManager", fileName =  "ScoreManager")]
public class ScoreManager : InitializableSO
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private ScoreSaveLoad scoreSaveLoad;
    
    [SerializeField] private BlockSelectionProcessor blockSelectionProcessor;

    [SerializeField] private SelectedBlockSet selectedBlockSet;

    [SerializeField] private int perBlockScore = 100;
    
    public event Action<int> UpdateScore;
    public event Action<int> UpdateHighScore;
    
    [ShowInInspector, ReadOnly]
    public int CurrentScore { private set; get; }
    
    [ShowInInspector, ReadOnly]
    public int HighScore { private set; get; }

    protected override void Init()
    {
        CurrentScore = 0;
        HighScore = 0;
        
        gameStateManager.OnChangeState += OnChangeState;
        blockSelectionProcessor.OnMatchBlocks += OnMatchBlock;

        HighScore = scoreSaveLoad.GetSavedHighScore();
    }

    protected override void DeInit()
    {
        UpdateScore = null;
        UpdateHighScore = null;
    }

    private void OnChangeState(GameStateType gameStateType)
    {
        switch (gameStateType)
        {
            case GameStateType.Prepare:
                OnPrepareGame();
                break;
            case GameStateType.Finish:
                OnFinish();
                break;
        }
    }

    private void OnPrepareGame()
    {
        ResetScore();
    }

    private void OnFinish()
    {
        CheckHighScore();
    }

    private void ResetScore()
    {
        CurrentScore = 0;
        UpdateScore?.Invoke(CurrentScore);
    }
    
    private void CheckHighScore()
    {
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            scoreSaveLoad.SaveNewHighScore(HighScore);
            
            UpdateHighScore?.Invoke(HighScore);
        }
    }
    
    private void OnMatchBlock()
    {
        int matchCount = selectedBlockSet.CoordList.Count;
        
        int multiplier = GetMultiplier(); //match count multiplier
        
        //combo multiplier
        
        //item multiplier

        CurrentScore += (matchCount * perBlockScore) * multiplier;

        UpdateScore?.Invoke(CurrentScore);

        #region Local Functions
        
        int GetMultiplier()
        {
            if (matchCount >= 10) 
                return 10;
            if (matchCount >=  8)
                return 5;
            if (matchCount >=  6)
                return 2;

            return 1;
        }
        
        #endregion
    }
}
