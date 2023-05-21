using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomHint : MonoBehaviour
{
    public TextMeshProUGUI item;

    private List<string> hints;
    private List<string> colors;
    // Start is called before the first frame update
    void Start()
    {
        hints = new List<string> { "Triangle", "Rectangle", "Circle" };
        colors = new List<string> { "Red", "Orange", "Yellow", "Green", "Blue", "Purple", "Pink", "Brown", "Black", "White" };
        Random();
    }

    // Random next object
    public void Random()
    {
        int randomIndexHint = UnityEngine.Random.Range(0, hints.Count);
        int randomIndexColor = UnityEngine.Random.Range(0, colors.Count);
        string itemHint = hints[randomIndexHint];
        string itemColor = colors[randomIndexColor];
        item.text = itemColor + " " + itemHint;
    }
}
