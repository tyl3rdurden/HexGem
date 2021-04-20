using UnityEngine;

[CreateAssetMenu(menuName = "TimeMode/TimeFree", fileName =  "TimeFreeMode")]
public class TimeFreeMode : TimeMode
{
    public override void OnUpdate()
    {
        CurrentTime += Time.deltaTime;

        SetCurrentTimeInt();
    }

    public override void SetCurrentTimeInt()
    {
        CurrentTimeInt = Mathf.FloorToInt(CurrentTime);
    }

    protected override void ResetTime()
    {
        CurrentTime = DefaultTime;
    }
}