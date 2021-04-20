using System;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/ScoreLabelAnimator", fileName =  "ScoreLabelAnimator")]
public class ScoreLabelAnimator : ScriptableObject
{
    public Action<int> UpdateAnimatedScore;
    
    [SerializeField] private DOTweenPlayerSO scoreLabelAnimation;
    [SerializeField] private float animationTime = .2f;
    
    private int addedScore = 0;
    private int currentScore = 0;
    private int animatedScore = 0;
    private int newTotalScore = 0;

    private bool isCountingUpScore = false;

    public void SetInstances(TextMeshProUGUI scoreLabel)
    {
        scoreLabelAnimation.SetTargetTransform((RectTransform)scoreLabel.transform);
    }
    
    public void OnUpdate()
    {
        CountUpScoreAnimation(Time.deltaTime);
    }
    
    public void OnUpdateScore(int newCurrentScore)
    {
        if (newCurrentScore == 0)
            ResetScore();
        else
            AnimateScore(newCurrentScore); 
    }

    private void CountUpScoreAnimation(float timeScale)
    {
        if (isCountingUpScore == false) return;
        
        animatedScore += (int)(addedScore * (timeScale/animationTime));
        if (animatedScore >= newTotalScore)
        {
            currentScore = newTotalScore;
            animatedScore = newTotalScore;
            
            isCountingUpScore = false;
        }
        
        UpdateAnimatedScore?.Invoke(animatedScore);
    }
    
    private void ResetScore()
    {
        newTotalScore = 0;
        currentScore = 0;
        animatedScore = 0;
        addedScore = 0;
        
        isCountingUpScore = false;
            
        scoreLabelAnimation.ResetState();
        
        UpdateAnimatedScore?.Invoke(currentScore);
    }
    
    private void AnimateScore(int newCurrentScore)
    {
        addedScore = newCurrentScore - currentScore;
        newTotalScore = newCurrentScore;
        
        isCountingUpScore = true;
        
        scoreLabelAnimation.Play();
    }
}