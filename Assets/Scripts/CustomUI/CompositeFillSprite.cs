using UnityEngine;
using Image = UnityEngine.UI.Image;

public class CompositeFillSprite : MonoBehaviour
{
    [SerializeField] private Image left;
    [SerializeField] private Image center;
    [SerializeField] private Image right;

    private float leftRectWidth;
    private float centerRectWidth;
    private float rightRectWidth;
    
    
    private float leftRectRatio;
    private float centerRectRatio;
    private float rightRectRatio;
    
    private float valueLeftStarts;
    private float valueCenterStarts;

    private float total;
    
    private void Awake()
    {
        leftRectWidth = left.rectTransform.rect.width;
        centerRectWidth = center.rectTransform.rect.width;
        rightRectWidth = right.rectTransform.rect.width;
        
        total = leftRectWidth + centerRectWidth + rightRectWidth;

        leftRectRatio = leftRectWidth / total;
        centerRectRatio = centerRectWidth / total;
        rightRectRatio = rightRectWidth / total;
        
        valueLeftStarts = leftRectRatio;
        valueCenterStarts = leftRectRatio + centerRectRatio;
        
    }

    public void SetValue(float value)
    {
        if (value == 1)
        {
            left.fillAmount = 1;
            center.fillAmount = 1;
            right.fillAmount = 1;
        }
        else if (value > valueCenterStarts)
        {
            left.fillAmount = 1;
            center.fillAmount = 1;
            right.fillAmount = (value - (leftRectRatio + centerRectRatio)) / rightRectRatio;
        }
        else if (value <= valueLeftStarts)
        {
            left.fillAmount = value / leftRectRatio;
            center.fillAmount = 0;
            right.fillAmount = 0;
        }
        else if (value <= valueCenterStarts)
        {
            left.fillAmount = 1;
            center.fillAmount = (value - leftRectRatio) / centerRectRatio;
            right.fillAmount = 0;
        }
    }
}
