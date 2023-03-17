using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 60;
    public TextMeshProUGUI textMeshProUGUI;
    public GameObject gameOverPanel;
    public GameObject scope;

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            textMeshProUGUI.text = Mathf.RoundToInt(timeRemaining).ToString();
        }
        else
        {
            timeRemaining = 0;
            textMeshProUGUI.text = "0";
            gameOverPanel.SetActive(true);
            scope.SetActive(false);
        }
    }
}