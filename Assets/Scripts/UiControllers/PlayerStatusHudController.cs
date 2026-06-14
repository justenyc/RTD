using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusHudController : MonoBehaviour
{
    public static PlayerStatusHudController Instance;

    [SerializeField] RectTransform healthMeter;
    [SerializeField] RectTransform lightMeter;
    [SerializeField] Image itemThumbnail;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    public void SetHealthMeterScale(float percentage)
    {
        healthMeter.localScale = new Vector2 (percentage, healthMeter.localScale.y);
    }

    public void SetLightMeterScale(float percentage)
    {
        lightMeter.localScale = new Vector2(lightMeter.localScale.x, percentage);
    }

    public void SetCurrentItemThumbnail(Sprite newSprite)
    {
        if(newSprite == null)
        {
            itemThumbnail.color = new Color(1, 1, 1, 0);
        }

        itemThumbnail.sprite = newSprite;
        itemThumbnail.color = new Color(1, 1, 1, 1);
    }
}
