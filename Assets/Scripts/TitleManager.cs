using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum level_name //basically tracks where the player spawns in at
{
    starting,
    colors
}
public class TitleManager : MonoBehaviour
{
    public static TitleManager instance { get; private set; }
    public static level_name level;
    public bool checkpointAvailable = true;
    public bool checkpointUsed = false;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    public void EnterLevel()
    {
        checkpointUsed = false;
        SceneManager.LoadScene("FallingApart");
        level = level_name.starting;
    }

    public void Checkpoint()
    {
        if(checkpointAvailable == true)
        {
            checkpointUsed = true;
            SceneManager.LoadScene("FallingApart");
            level = level_name.starting;
        }
    }

}
