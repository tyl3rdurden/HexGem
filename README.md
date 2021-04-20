# Hex Gem

Portfolio project of a simple 3 match game 


Features:
* Classic simple brush style 3 match game
* Time based (but also has Unlimited time mode option)
* Score system
  * Saves High score
* Items drops depending on number of connected blocks
* Pause support


Game Play \
![GamePlay](https://i.imgur.com/RHTe9aj.gif) \
(Graphics Credits: [KennyNL](https://www.kenney.nl/assets))

Setup Instructions:
1. Pull repository
2. Import Odin Inspector asset into Plugin folder
3. Enable Simulated Touchscreen support \
  3.1 Open Input Debug Window \
   ![Imgur](https://i.imgur.com/4dARPgo.png) \
  3.2 Enable Simulated Touchscreen support \
   ![Imgur](https://i.imgur.com/6EiHma4.png)
4. Press Play!


## Technical Goals 

### Try out Scriptable Object Architecture
* **Unity Editor is Dependency Injector**  
  * No need for other large libraries ex) Zenject  
* NO `MonoBehaviour` singletons  
  * Bring any prefab into scene and have it work without dragging in millions of connected managers/singletons
* Reduce Scene version control conflicts as most changes happen in smaller chunks of asset data and not scene data
* Able to swap data or algorithms easily by copying the original SO, changing the data, and dragging in the new one
* Based off of the Unite videos
  * [Unite 2017 - Game Architecture with Scriptable Objects](https://www.youtube.com/watch?v=raQ3iHhE_Kk)
  * [Unite 2016 - Overthrowing the MonoBehaviour Tyranny](https://www.youtube.com/watch?v=6vmRwLYWNRo)

### Focus on rapid iteration
* Use of non domain/scene reload play mode enter \
  ![Imgur](https://i.imgur.com/6MLNR4B.png)
  * **Extremely fast** when entering and exiting scene
  * Requires a bit more code maintenance due to persistent variables not resetting - static, scriptable objects etc.
    * Requires manual reset of variables via `InitializableSO.Init()` which uses `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]`
  * Without reloading Domain & Scene
    * Less than 1 second \
    ![NoReload](https://i.imgur.com/PqGTVgc.gif)
  * With reloading Domain & Scene
    * 10 seconds+ \
    ![WithReload](https://i.imgur.com/uQucU8f.gif)
	
#### Editor Tools

* Pre-Made visual Grid system for easy testing  
  * ex) setting up a new grid for item spawn testing \
  ![Pre-Made visual Grid system](https://i.imgur.com/LmHBihw.gif)

* Debug Menu
  * Toggle specific parts of a prefab / scene object from menu without additional clutter of other irrelevant fields
    * Top Inspector: Prefab Instance
    * Bottom Inspector: Prefab Asset
    * Debug Window: Able to control both asset and instance from same menu
  * Saves and loads references to objects via GUID and EditorPref \
  ![Debug Menu](https://i.imgur.com/jzMcoW0.gif)


* Preview and test UI Tweens during Edit Mode 
  * Drag any UI prefab into empty scene (with camera and canvas)
    * Press Show/Hide to preview animation
    * Modify animation inside same inspector menu and preview change immediately \
    ![Editor Tween](https://i.imgur.com/BVBdUfK.gif)

### Learn UGUI
* Easy support for multi resolution mobile devices

### Try out the New Input System
* Central input handler 
* Input without using `Update()`
  * Able to handle input via ScriptableObject only

### Create a simple but robust UI System
* Basic UIController with DOTweenPlayerSOs that handle in and out animations
  * Inherited by HUDController and PopupController
    * PopupController 
      * Adds connection to PopupManager and an exitButton
      * PopupManager handles back button flow and ability to open/close any popup 
      * Sealed `ShowView()` `HideView()` methods after adding PopupManager integration
    * HUDController
      * Sealed `ShowView()` `HideView()` methods to prevent further modification. Any other
        modification can only be done through `OnShow()` `OnHide()` `AfterMoveIn()` `AfterMoveOut()`

### Try out various plugins
* [Odin Inspector/Serializer](https://odininspector.com/) - **Required for project** (Not included in repo - **Paid Asset**)
  * Allows serialization of various data types such as **interfaces**, properties, 2D arrays, dictionaries, etc.
  * Allows extending Unity Editor really easily and build various tools
  

* [DOTween](http://dotween.demigiant.com/)
  * Performant popular tween library
  * Extended to allow easily visualization of tweens and drag and drop


* [Asset Usage Detector](https://github.com/yasirkula/UnityAssetUsageDetector)
  * Allows finding of Scriptable Object references - a common complaint of SO oriented architecture \
    ![Imgur](https://i.imgur.com/WUtMxOx.png)


* [SuperUnityBuild](https://github.com/Chaser324/unity-build)
  * Personal Modifications
    * Fixed errors for Unity 2019.4
      * Added additional options
      * Incremental GC
      * C++ Compiler Configuration
      * Wait For Managed Debugger
  * Allows one click builds for various Platforms and Release types
  * Allows Pre-Build / Post-Build actions
  * Easily accessible GUI within Unity \
  ![SuperUnityBuild](https://i.imgur.com/xFzojIJ.png)
  

* [UniTask](https://github.com/Cysharp/UniTask)
  * PlayerLoop based task(UniTask.Yield, UniTask.Delay, UniTask.DelayFrame, etc..) that enable replacing all coroutine operations
  * Does NOT require `MonoBehaviour` - perfect for Scriptable Objects
  

* [ZString](https://github.com/Cysharp/ZString)
  * Integrated with Unity TextMeshPro to avoid string allocation
  

* [SceneDropdown](https://github.com/markv12/AutomaticSceneDropdownDemo)
  * Searches project for scene files and adds them to a dropdown list that loads scene on click \
  ![SceneDropdown](https://i.imgur.com/X1O9zFN.png) 


* [Unity UI Extensions](https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/wiki/Home)
  * Community contributed Unity UI extensions with various convenient and new features for handling uGUI
    * Use case: Option Popup's iOS style Toggle 
 

* ~~[Particle Effects for UGUI](https://github.com/mob-sakai/ParticleEffectForUGUI)~~
  * Significant Garbage Allocation everytime a single particle was played
  * Using 2 cameras instead - Particle and Main Camera

## Architecture
* Component based
  * Comprised of small components to be able to debug and fix issues easily
  * Compartmentalize code to prevent being overloaded with too much information
* UML
  * Block System -  \
    ![Block](https://i.imgur.com/qeTbhm4.png)
  * UI - UI System with Back Button Handling \
    ![UI](https://i.imgur.com/s1MuwEL.png)
  * Tween Player \
    ![Tween Player](https://i.imgur.com/K7icC6t.png)
  * Time Mode \
    ![Time](https://i.imgur.com/hQm7yiz.png)
* Code Patterns
  * Mixins
    * Reuse common functionality across classes without the rigidness of inheritance
    * Made it a SO for my case so that it can hold common references to assets
      * Dont have to drag & drop identical references for each class that uses the mixin
  * Visitor Patterns
    * Used for different data and process into one common class
    * Ex)
      * `MoveTween` fields - `Duration`, `EndPos`, `MoveType` etc.
      * `ColorTween` fields - `Duration`, `EndColor`, `OnlyAlpha` etc.
      * Varying data values but still able to hold together in array and process appropriately into separate tweens \
      ![Visitor](https://i.imgur.com/Wm36qAA.png)

## Technical Details

* Neighbor lookup table
  * Used a lookup table to pre-calculate neighbor blocks instead of calculating them every time
  * Serialized values in `NeighborGrid` that are calculated in the editor only once
  

## Future TO-DOs
* Score increase animation
  * Currently, score is at top of screen
    * Problem: hard to see unless you focus on top of screen (which you dont)
    * Solution: Add a score popup animation at the position of a match \
      -> would allow player to feel their score increasing
* Combo counter that increments with multiple completions in a row / within a few seconds of each other
