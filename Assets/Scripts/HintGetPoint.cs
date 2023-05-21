using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HintGetPoint : MonoBehaviour
{
    public TextMeshProUGUI score;
    public RandomHint randomizer;
    private int current_score = 0;
    public AudioSource correctSound;

    // Update is called once per frame
    public void Add(string item = "")
    {
        // Split the input string by whitespace
        string[] words = item.Split(' ');

        if (words.Length > 1)
        {
            switch (words[1])
            {
                case ("Triangle"):
                case ("Rectangle"):
                    current_score += 20;
                    break;
                default:
                    current_score += 10;
                    break;

            }
        }
        correctSound.Play();
        score.text = current_score.ToString();
        randomizer.Random();
    }

    public void Random()
    {
        randomizer.Random();
    }
}
