using UnityEngine;
using UnityEngine.UI;

public class IspyScaler : MonoBehaviour
{
    public Image scope;
    public Image Ispy;
    public float scopeScaleFactor = 1.8f;
    public float IspyScaleFactor = 1.8f;

    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float scopeWidth = scope.sprite.rect.width;
        float scopeHeight = scope.sprite.rect.height;

        float IspyWidth = Ispy.sprite.rect.width;
        float IspyHeight = Ispy.sprite.rect.height;

        float screenRatio = screenWidth / screenHeight;
        float scopeRatio = scopeWidth / scopeHeight;
        float IspyRatio = IspyWidth / IspyHeight;

        float scopeScale;
        float IspyScale;

        if (screenRatio > scopeRatio)
        {
            scopeScale = screenHeight / scopeHeight * scopeScaleFactor;
        }
        else
        {
            scopeScale = screenWidth / scopeWidth * scopeScaleFactor;
        }


        if (screenRatio > IspyRatio)
        {
            IspyScale = screenHeight / IspyHeight * IspyScaleFactor;
        }
        else
        {
            IspyScale = screenWidth / IspyWidth * IspyScaleFactor;
        }

        scope.GetComponent<RectTransform>().localScale = new Vector3(scopeScale, scopeScale, 1f);
        float scopeTop = (scope.GetComponent<RectTransform>().rect.height * scopeScale)/2;

        Ispy.GetComponent<RectTransform>().localScale = new Vector3(IspyScale, IspyScale, 1f);
        float imageTop = (Ispy.GetComponent<RectTransform>().rect.height * IspyScale)/2;

        Ispy.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0.5f + scopeTop + imageTop);
    }
}
