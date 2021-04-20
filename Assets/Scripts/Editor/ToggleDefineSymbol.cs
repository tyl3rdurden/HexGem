using UnityEditor;

public class ToggleDefineSymbol : Editor
{
    private const string Live = "LIVE";
    private const string Test = "TEST";
    
    [MenuItem("Tools/Set Prod")]
    public static void Set_ProductionBuild()
    {
        string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
            BuildTargetGroup.Android);

        if (!symbols.Contains(Live))
        {
            symbols = symbols.Replace(";" + Test, string.Empty);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                BuildTargetGroup.Android, symbols + ";" + Live);
        }
    }

    [MenuItem("Tools/Set Test")]
    public static void Set_Test()
    {
        string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
            BuildTargetGroup.Android);

        if (!symbols.Contains(Test))
        {
            symbols = symbols.Replace(";" + Live, string.Empty);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                BuildTargetGroup.Android, symbols + ";" + Test);
        }
    }
}