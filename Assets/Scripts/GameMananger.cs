using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum tileColor
{
    RED,
    GREEN,
    BLUE,
    YELLOW,
    BLACK
}
public class GameMananger : MonoBehaviour
{
    public static GameMananger instance; //singleton
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject tileDisplay;
    [SerializeField] GameObject colorDoor;
    [SerializeField] PlayerController player;
    tileColor[] combo = { tileColor.RED, tileColor.GREEN, tileColor.BLUE, tileColor.GREEN, tileColor.YELLOW, tileColor.BLACK, tileColor.RED, tileColor.RED, tileColor.YELLOW, tileColor.BLUE, tileColor.RED, tileColor.YELLOW, tileColor.RED, tileColor.BLUE, tileColor.BLACK, tileColor.BLUE};
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
        //Debug.Log(c);
        if(c != combo[position])
        {
            StartCoroutine(GameOver());
        }
        else
        {
            tileDisplay.transform.GetChild(position).GetComponent<Image>().color = getColor(true, c);
            position++;
            if(position == combo.Length)
            {
                Debug.Log("yo");
                colorDoor.GetComponent<Animator>().SetBool("Open", true);
            }
        }
    }
    public IEnumerator GameOver()
    {
        gameOver.SetActive(true);
        Destroy(player.GetComponent<CharacterController>());
        Cursor.lockState = CursorLockMode.None; //unlock mouse before going back to main menu
        Cursor.visible = true;
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Title");
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
        else if (on && color == tileColor.BLACK)
        {
            return Color.black;
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
        else if (!on && color == tileColor.BLACK)
        {
            return new Color(0.2f, 0.2f, 0.2f, 1);
        }
        return Color.black;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TitleManager.instance.checkpointAvailable = true;
            Debug.Log("checkpoint");
        }
    }

}
