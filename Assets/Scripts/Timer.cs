using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 60;
    public TextMeshProUGUI textMeshProUGUI;
    public GameObject gameOverPanel;
    public GameObject scope;
    public GameObject Ispy;
    public GameObject textBox;
    public GameObject toBeDetectedItem;
    public GameObject SkipButton;
    public AudioSource timeOutSound;
    public AudioSource clockTick;

    public GameObject count;


    private bool isEndPlayed = false;
    private bool isTickPlayed = false;

    private void Update()
    {
        if (!count.activeSelf)
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
                Ispy.SetActive(false);
                textBox.SetActive(false);
                toBeDetectedItem.SetActive(false);
                SkipButton.SetActive(false);
                if (!isEndPlayed)
                {
                    isEndPlayed = true;
                    timeOutSound.Play();
                }

            }

            if (Mathf.RoundToInt(timeRemaining) < 11 && timeRemaining > 0)
            {
                textMeshProUGUI.color = Color.red;
                if (!isTickPlayed)
                {
                    isTickPlayed = true;
                    clockTick.Play();
                }

            }
        }
    }
       
}