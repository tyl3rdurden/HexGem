using UnityEngine;

public class ParticleFXAnimator : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private FXGrid fxGrid;
    [SerializeField] private BlockSelectionProcessor blockSelectionProcessor;

    [SerializeField] private SelectedBlockSet selectedBlockSet;
    
    private void Start()
    {
        PopulateFXGrid();
        InputSubscription();
        gameStateManager.OnChangeState += OnChangeState;
    }

    private void OnChangeState(GameStateType gameStateType)
    {
        switch (gameStateType)
        {
            case GameStateType.Finish:
                break;
        }
    }

    private void PopulateFXGrid()
    {
        Transform cachedTransform = transform;

        int width = 0;
        int height = 0;
        
        for (int i = 0; i < cachedTransform.childCount; i++)
        {
            var child = cachedTransform.GetChild(i);
            
            if (height > HexGrid.Height - 1)
            {
                height = 0;
                width++;
            }

            fxGrid.ParticleFXs[width, height] = child.GetComponent<ParticleSystem>();
            
            height++;
        }
    }

    private void InputSubscription()
    {
        blockSelectionProcessor.OnMatchBlocks += PlayMatchParticleFX;
    }

    private void PlayMatchParticleFX()
    {
        for (int i = 0; i < selectedBlockSet.CoordList.Count; i++)
        {
            fxGrid.ParticleFXs[selectedBlockSet.CoordList[i].X, selectedBlockSet.CoordList[i].Y].Play();
        }
    }
}