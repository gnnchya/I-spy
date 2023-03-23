using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class countStart : MonoBehaviour
{
    public float timeRemaining = 5;
    public TextMeshProUGUI count;
    public GameObject countObject;
    public GameObject NoCamDetected;
    public GameObject Item;
    public GameObject toBeDetectedItem;
    public AudioSource StartSound;
    public AudioSource clockTick;
    public GameObject skipButton;

    private bool isTickPlayed = false;
    private bool isStartPlayed = false;
    private bool done = false;

    private void Update()
    {
        if (!done && !NoCamDetected.activeSelf)
        {
            if (Mathf.RoundToInt(timeRemaining) > 1)
            {
                timeRemaining -= Time.deltaTime;
                count.text = Mathf.RoundToInt(timeRemaining - 1).ToString();

            }
            else
            {
                if (!isStartPlayed)
                {
                    isStartPlayed = true;
                    StartSound.Play();
                }
                countObject.SetActive(false);
                Item.SetActive(true);
                skipButton.SetActive(true);
                toBeDetectedItem.SetActive(true);
                done = true;

            }

            if (Mathf.RoundToInt(timeRemaining) < 5)
            {
                if (!isTickPlayed)
                {
                    isTickPlayed = true;
                    clockTick.Play();
                }
            }
        }   
    }
}
