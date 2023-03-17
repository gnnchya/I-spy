using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapScores : MonoBehaviour
{
    public TextMeshProUGUI scoreNav;
    public TextMeshProUGUI scorePanel;

    // Update is called once per frame
    void Update()
    {
        scorePanel.text = scoreNav.text;
    }
}
