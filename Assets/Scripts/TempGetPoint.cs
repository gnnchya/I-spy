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
    public void Add()
    {
        current_score += 10;
        correctSound.Play();
        score.text = current_score.ToString();
        randomizer.Random();
    }
}
