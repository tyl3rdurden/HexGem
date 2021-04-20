using Sirenix.OdinInspector;
using UnityEngine;

public class UI_InGame_BonusTimeAnimation : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private ItemActivator itemActivator;
    
    [ChildGameObjectsOnly]
    [SerializeField] private RectTransform bonusTime;
    [SerializeField] private DOTweenPlayerSO bonusTimeAnim;

    void Awake()
    {
        TurnOffCanvas();

        SubscribeStateChanges();
        SetAnimation();
    }

    private void SubscribeStateChanges()
    {
        gameStateManager.GameStateDic[GameStateType.Bonus].OnPreEnterState += StartAnimation;
        gameStateManager.GameStateDic[GameStateType.Bonus].OnExitState += TurnOffCanvas;
    }

    private void SetAnimation()
    {
        bonusTimeAnim.SetTargetTransform(bonusTime);
        bonusTimeAnim.OnFinish += OnFinishAnimation;
    }

    private void StartAnimation()
    {
        if (DoesItemExist() == false)
        {
            gameStateManager.ChangeState(GameStateType.Finish);
            return;
        }

        bonusTime.localScale = Vector3.zero;
        
        gameObject.SetActive(true);
        bonusTimeAnim.Play();
    }

    private void OnFinishAnimation()
    {
        itemActivator.ActivateRemainingItems();
    }

    private void TurnOffCanvas()
    {
        gameObject.SetActive(false);
    }

    private bool DoesItemExist()
    {
        for (byte width = 0; width < HexGrid.Width; width++)
        {
            for (byte height = 0; height < HexGrid.Height; height++)
            {
                Block currentBlock = hexGrid.GetBlockAt(width, height);

                if (IsCurrentBlockItem())
                    return true;

                bool IsCurrentBlockItem()
                {
                    return currentBlock.Type > BlockType.Item_Start;
                }
            }
        }

        return false;
    }
}