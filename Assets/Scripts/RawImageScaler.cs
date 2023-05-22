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
        // rectTransform.anchorMin = Vector2.zero;
        // rectTransform.anchorMax = Vector2.one;
        // rectTransform.sizeDelta = Vector2.zero;
        // // rectTransform.anchoredPosition = Vector2.zero;
        // float screenWidth = Screen.width;
        // rectTransform.Height = Screen.width;
        rectTransform.sizeDelta = new Vector2(Screen.height, Screen.width);
        rectTransform.Rotate(new Vector3(0, 0, 90));
        rectTransform.localScale = new Vector3(-1, 1, 1);
    }

}
