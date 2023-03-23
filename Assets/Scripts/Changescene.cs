using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Changescene : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadLandingScene()
    {
        SceneManager.LoadScene("Landing");
    }

    public void LoadGameSceneHint()
    {
        SceneManager.LoadScene("GameSceneHint");
    }
}
