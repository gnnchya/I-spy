using UnityEngine;
using UnityEngine.UI;

public class RawImageScaler : MonoBehaviour
{
    public RawImage rawImage;

    void Start()
    {
        ScaleRawImage();
    }

    void ScaleRawImage()
    {
        RectTransform rectTransform = rawImage.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
