using UnityEngine;

public class BlockInputHandler : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private BlockRaycaster raycaster;
    [SerializeField] private BlockSelectionProcessor blockSelectionProcessor;
    
    private void Start()
    {
        inputManager.OnPressEvent += OnPress;
        inputManager.OnDragEvent += OnDrag;
        inputManager.OnReleaseEvent += OnRelease;
    }

    private void OnDestroy()
    {
        inputManager.OnPressEvent -= OnPress;
        inputManager.OnDragEvent -= OnDrag;
        inputManager.OnReleaseEvent -= OnRelease;
    }

    private void OnPress(Vector2 position)
    {
        if (raycaster.RaycastHitBlock(position))
            blockSelectionProcessor.OnSelect(raycaster.CurrentBlock());
    }

    private void OnDrag(Vector2 position)
    {
        if (raycaster.RaycastHitBlock(position))
            blockSelectionProcessor.OnSelect(raycaster.CurrentBlock());
    }

    private void OnRelease()
    {
        blockSelectionProcessor.OnRelease();
    }
}