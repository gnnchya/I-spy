using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using TMPro;

public class RandomObject : MonoBehaviour
{
    public TextAsset labelsFile;
    public TextMeshProUGUI item;

    private string[] labels;
    private string[] used;

    // Start is called before the first frame update
    void Start()
    {
        this.labels = Regex.Split(this.labelsFile.text, "\n|\r|\r\n")
           .Where(s => !String.IsNullOrEmpty(s)).ToArray();
        this.used = new string[0];
        Random();
    }

    // Update is called once per frame
    public void Random()
    {
        if (labels.Length == 0)
        {
            // All items have been used, reset the list
            labels = used;
            used = new string[0];
        }
        // Randomly select an item from the labels array
        int randomIndex = UnityEngine.Random.Range(0, labels.Length);
        string itemText = labels[randomIndex];
        // Add the selected item to the used array and remove it from labels
        used = used.Concat(new string[] { itemText }).ToArray();
        labels = labels.Where((val, idx) => idx != randomIndex).ToArray();
        // Set the text of the item TextMeshProUGUI component
        item.text = itemText;
    }
}
