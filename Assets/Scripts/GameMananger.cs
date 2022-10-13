using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum tileColor
{
    RED,
    GREEN,
    BLUE,
    YELLOW
}
public class GameMananger : MonoBehaviour
{
    public static GameMananger instance; //singleton
    [SerializeField] Text gameOver;
    tileColor[] combo = { tileColor.RED, tileColor.GREEN, tileColor.YELLOW, tileColor.BLUE, tileColor.GREEN, tileColor.BLUE, tileColor.YELLOW, tileColor.GREEN, tileColor.BLUE, tileColor.RED};
    private int position = 0;

    private void Awake()
    {
        //singleton
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void colorTileTrigger(tileColor c)
    {
        Debug.Log(c);
        if(c != combo[position])
        {
            gameOver.text = "GameOver";
        }
        else
        {
            position++;
        }
    }
}
