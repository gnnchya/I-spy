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
    private List<string> used;
    private List<string> excluded;

    // Start is called before the first frame update
    void Start()
    {
        this.labels = Regex.Split(this.labelsFile.text, "\n|\r|\r\n")
           .Where(s => !String.IsNullOrEmpty(s)).ToArray();
        this.used = new List<string>();
        this.excluded = new List<string>();
        Random();
    }

    // Update is called once per frame
    public void Random(string excludedItems = "")
    {
        // Add the excluded items to the excluded list
        if (!String.IsNullOrEmpty(excludedItems))
        {
            this.excluded.Add(excludedItems);
        }

        if (labels.Length == 0)
        {
            // All items have been used or excluded, reset the list
            labels = used.ToArray();
            used.Clear();
        }
        // Randomly select an item from the labels array
        int randomIndex = UnityEngine.Random.Range(0, labels.Length);
        string itemText = labels[randomIndex];

        if (String.IsNullOrEmpty(excludedItems))
        {
            Debug.Log("used" + excludedItems);
            used.Add(itemText);
        } else
        {
            used = used.Where(x => !excluded.Contains(x)).ToList();
        }

        labels = labels.Where((val, idx) => idx != randomIndex).ToArray();
        item.text = itemText;
    }
}
