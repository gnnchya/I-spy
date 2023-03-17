using UnityEngine;
using UnityEngine.UI;

public class ImageScaler : MonoBehaviour
{
    public Image image;
    public float scaleFactor = 0.5f; // adjust this to change the scale factor

    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float imageWidth = image.sprite.rect.width;
        float imageHeight = image.sprite.rect.height;

        float screenRatio = screenWidth / screenHeight;
        float imageRatio = imageWidth / imageHeight;

        float scale = 0.2f;

        if (screenRatio > imageRatio)
        {
            scale = screenHeight / imageHeight * scaleFactor;
        }
        else
        {
            scale = screenWidth / imageWidth * scaleFactor;
        }

        image.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1f);
    }
}

