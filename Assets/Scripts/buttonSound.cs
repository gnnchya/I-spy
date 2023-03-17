using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSound : MonoBehaviour
{
    public AudioSource audio;

    public void click()
    {
        audio.Play();
    }
}
