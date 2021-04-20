using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/ScoreSaveLoad", fileName =  "ScoreSaveLoad")]
public class ScoreSaveLoad : InitializableSO
{
    private const string HighScoreKey = "HG_HighScoreKey";

    public int GetSavedHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public void SaveNewHighScore(int newHighScore)
    {
        PlayerPrefs.SetInt(HighScoreKey, newHighScore);
        PlayerPrefs.Save();
    }
}