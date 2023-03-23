using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TempGetPoint : MonoBehaviour
{
    public TextMeshProUGUI score;
    public RandomObject randomizer;
    private int current_score = 0;
    public AudioSource correctSound;

    // Update is called once per frame
    public void Add(string item = "")
    {
        switch (item)
        {
            case ("Luggage"):
            case ("Chair"):
            case ("Table"):
            case ("Sofa"):
            case ("Bicycle"):
                current_score += 20;
                break;
            default:
                current_score += 10;
                break;

        }
        correctSound.Play();
        score.text = current_score.ToString();
        randomizer.Random(item);
    }

    public void Random()
    {
        randomizer.Random();
    }
}
