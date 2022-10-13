using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BodyPart
{
    LEG,
    ARM,
    HEAD
}
//Keeps track of body parts, and the UI
public class Body : MonoBehaviour
{
    //2 = both on, 0 = both off
    private int legs = 2;
    private int arms = 2;
    //Need a reference to the gameobjects, just in case
    /*private GameObject legRightGO;
    private GameObject legLeftGO;
    private GameObject armRightGO;
    private GameObject armLeftGO;*/
    //UI images are children. Get them with GetChild    
    public void Start()
    {
    }
    public bool partAvailable(BodyPart bp) //returns true if that bodypart is available for use
    {
        if (bp == BodyPart.ARM)
        {
            if(arms > 0)
            {
                return true;
            }
        }
        else if(bp == BodyPart.LEG)
        {
            if(legs > 0)
            {
                return true;
            }
        }
        return false;
    }
    public bool partSpaceAvailable(BodyPart bp) //returns true if there is space for that bodypart
    {
        if (bp == BodyPart.ARM)
        {
            if (arms < 2)
            {
                return true;
            }
        }
        else if (bp == BodyPart.LEG)
        {
            if (legs < 2)
            {
                return true;
            }
        }
        return false;
    }
    public void usePart(BodyPart bp)
    {
        if (bp == BodyPart.ARM)
        {
            arms--;
            if(arms == 1)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
            }
            else if (arms == 0)
            {
                transform.GetChild(1).GetComponent<Image>().color = Color.red;
            }
        }
        else if (bp == BodyPart.LEG)
        {
            legs--;
            if (legs == 1)
            {
                transform.GetChild(2).GetComponent<Image>().color = Color.red;
            }
            else if (legs == 0)
            {
                transform.GetChild(3).GetComponent<Image>().color = Color.red;
            }
        }
    }
    public void addPart(BodyPart bp)
    {
        if (bp == BodyPart.ARM)
        {
            arms++;
            if (arms == 2)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
            else if (arms == 1)
            {
                transform.GetChild(1).GetComponent<Image>().color = Color.white;
            }
        }
        else if (bp == BodyPart.LEG)
        {
            legs--;
            if (legs == 2)
            {
                transform.GetChild(2).GetComponent<Image>().color = Color.white;
            }
            else if (legs == 1)
            {
                transform.GetChild(3).GetComponent<Image>().color = Color.white;
            }
        }
    }
}
