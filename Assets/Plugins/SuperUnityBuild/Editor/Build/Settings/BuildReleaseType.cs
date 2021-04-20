using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SuperSystems.UnityBuild
{
    public enum CPPCompilerConfigurationType
    {
        Debug,
        Release,
        Master
    }

    [System.Serializable]
    public class BuildReleaseType
    {
        public string typeName = string.Empty;
        public string bundleIdentifier = string.Empty;
        public string companyName = string.Empty;
        public string productName = string.Empty;

        //Project Settings
        public bool incrementalGC;
        public CPPCompilerConfigurationType cppCompilerConfiguration;

        //Debug Settings
        public bool developmentBuild = false;
        public bool allowDebugging = false;
        public bool waitForManagedDebugger = false;
        public bool enableHeadlessMode = false;
        public string customDefines = string.Empty;

        public SceneList sceneList = new SceneList();
    }
}