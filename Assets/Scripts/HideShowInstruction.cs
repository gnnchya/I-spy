using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowInstruction : MonoBehaviour
{
    public GameObject instruction;
    
    public void Show()
    {
        instruction.SetActive(true);
    }

    public void Hide()
    {
        instruction.SetActive(false);
    }
}
