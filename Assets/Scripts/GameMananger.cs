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
    [SerializeField] GameObject tileDisplay;
    tileColor[] combo = { tileColor.RED, tileColor.GREEN, tileColor.YELLOW, tileColor.BLUE, tileColor.GREEN, tileColor.BLUE, tileColor.YELLOW, tileColor.GREEN, tileColor.BLUE, tileColor.RED};
    private int position = 0;

    //singleton
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
        for(int i = 0; i < combo.Length; ++i)
        {
            tileDisplay.transform.GetChild(i).GetComponent<Image>().color = getColor(false, combo[i]);
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
            tileDisplay.transform.GetChild(position).GetComponent<Image>().color = getColor(true, c);
            position++;
        }
    }
    public Color getColor(bool on, tileColor color) //returns a "proper" color for tile color display
    {
        if (on && color == tileColor.RED)
        {
            return Color.red;
        }
        else if (on && color == tileColor.BLUE)
        {
            return Color.blue;
        }
        else if (on && color == tileColor.GREEN)
        {
            return new Color(0, .9f, 0, 1);
        }
        else if (on && color == tileColor.YELLOW)
        {
            return new Color(1, 1, 0, 1);
        }
        else if (!on && color == tileColor.RED)
        {
            return new Color(.5f, 0, 0, 1);
        }
        else if (!on && color == tileColor.BLUE)
        {
            return new Color(0, .08f, .5f, 1);
        }
        else if (!on && color == tileColor.GREEN)
        {
            return new Color(0, .5f, 0, 1);
        }
        else if (!on && color == tileColor.YELLOW)
        {
            return new Color(0.6f, 0.6f, 0, 1);
        }
        return Color.black;
    }
}
