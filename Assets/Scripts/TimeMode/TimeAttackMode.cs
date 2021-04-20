using UnityEngine;

[CreateAssetMenu(menuName = "TimeMode/TimeAttack", fileName =  "TimeAttackMode")]
public class TimeAttackMode : TimeMode
{
    public override void OnUpdate()
    {
        CurrentTime -= Time.deltaTime;
                                 
        if (CurrentTime < 0)
        {
            gameStateManager.ChangeState(GameStateType.Bonus);
        }

        SetCurrentTimeInt();
    }

    public override void SetCurrentTimeInt()
    {
        CurrentTimeInt = Mathf.CeilToInt(CurrentTime);
    }

    protected override void ResetTime()
    {
        CurrentTime = DefaultTime;
    }
}
