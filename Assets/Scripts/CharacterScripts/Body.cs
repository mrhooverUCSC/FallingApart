using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BodyPart //we'll see if the arms need to differentiate eventually
{
    LEFTLEG,
    RIGHTLEG,
    LEG, //leg is for either one
    ARM,
    HEAD,
    NONE
}
public enum BodyPartState
{
    ON,
    CONTROLLED,
    OFF
}
enum ControlledPart
{
    BODY,
    LEFTLEG,
    RIGHTLEG
}

//Keeps track of body parts, and the UI
public class Body : MonoBehaviour
{
    GameObject rightLegPrefab;
    GameObject leftLegPrefab;
    ControlledPart cp = ControlledPart.BODY; //keeps track of the currently controlled part
    bool hidden = true;
    //legs are individual, true is on, 
    BodyPartState leftArm = BodyPartState.ON;
    BodyPartState rightArm = BodyPartState.ON;
    BodyPartState leftLeg = BodyPartState.ON;
    BodyPartState rightLeg = BodyPartState.ON;

    //slots to put instantiated parts into
    GameObject rightLegGO;
    GameObject leftLegGO;
    GameObject rightArmGO;
    GameObject leftArmGO;
    //UI images are children. Get them with GetChild: 0 - left arm, 1 - right arm, 2 - left leg, 3 - right leg, 4 - body, 5 - head
    //CannotMoveAlert is child 6
    public void Start()
    {
        rightLegPrefab = Resources.Load<GameObject>("Prefabs/RightLeg");
        leftLegPrefab = Resources.Load<GameObject>("Prefabs/LeftLeg");
    }
    public void Update()
    {
    }
    public bool partAvailable(BodyPart bp) //returns true if that bodypart is available for use. Disconnected is false.
    {
        if (bp == BodyPart.ARM)
        {
            if(leftArm == BodyPartState.ON || rightArm == BodyPartState.ON)
            {
                return true;
            }
        }
        else if(bp == BodyPart.RIGHTLEG)
        {
            if(rightLeg == BodyPartState.ON)
            {
                return true;
            }
        }
        else if (bp == BodyPart.LEFTLEG)
        {
            if (leftLeg == BodyPartState.ON)
            {
                return true;
            }
        }
        else if(bp == BodyPart.LEG)
        {
            if(rightLeg == BodyPartState.ON || leftLeg == BodyPartState.ON)
            {
                return true;
            }
        }
        return false;
    }
    public bool partSpaceAvailable(BodyPart bp) //returns true if there is space for that bodypart. if a part is Disconnected, returns true.
    {
        //Debug.Log("leftarm: " + leftArm + " |rightarm: " + rightArm + "|leftleg: " + leftLeg + "|rightLeg: " + rightLeg); 
        if (bp == BodyPart.ARM)
        {
            if (leftArm != BodyPartState.ON || rightArm != BodyPartState.ON)
            {
                return true;
            }
        }
        else if (bp == BodyPart.LEFTLEG)
        {
            if (leftLeg != BodyPartState.ON)
            {
                return true;
            }
        }
        else if(bp == BodyPart.RIGHTLEG)
        {
            if(rightLeg != BodyPartState.ON)
            {
                return true;
            }
        }
        else if (bp == BodyPart.LEG)
        {
            if(leftLeg != BodyPartState.ON || rightLeg != BodyPartState.ON)
            {
                return true;
            }
        }
        return false;
    }
    public void usePart(BodyPart bp)//only call after checking partAvailable. Cannot remotely use Disconnected parts. Intended for "interact" functions
    {
        Debug.Log("usepart" + bp);
        if (bp == BodyPart.ARM)
        {
            if(leftArm == BodyPartState.ON)
            {
                leftArm = BodyPartState.OFF;
                gameObject.SetActive(true);
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
            }
            else if (rightArm == BodyPartState.ON)
            {
                rightArm = BodyPartState.OFF;
                gameObject.SetActive(true);
                transform.GetChild(1).GetComponent<Image>().color = Color.red;
            }
        }
        else if(bp == BodyPart.LEG)
        {
            if (leftLeg == BodyPartState.ON)
            {
                leftLeg = BodyPartState.OFF;
                gameObject.SetActive(true);
                transform.GetChild(2).GetComponent<Image>().color = Color.red;
                transform.GetChild(6).GetComponent<Text>().enabled = false;
            }
            else if (rightLeg == BodyPartState.ON)
            {
                rightLeg = BodyPartState.OFF;
                gameObject.SetActive(true);
                transform.GetChild(3).GetComponent<Image>().color = Color.red;
                transform.GetChild(7).GetComponent<Text>().enabled = false;
            }
        }
        else if (bp == BodyPart.LEFTLEG)
        {
            if (leftLeg == BodyPartState.ON)
            {
                leftLeg = BodyPartState.OFF;
                gameObject.SetActive(true);
                transform.GetChild(2).GetComponent<Image>().color = Color.red;
                transform.GetChild(6).GetComponent<Text>().enabled = false;
            }
        }
        else if (bp == BodyPart.RIGHTLEG)
        {
            if (rightLeg == BodyPartState.ON)
            {
                rightLeg = BodyPartState.OFF;
                gameObject.SetActive(true);
                transform.GetChild(3).GetComponent<Image>().color = Color.red;
                transform.GetChild(7).GetComponent<Text>().enabled = false;
            }
        }
        canMove();
    }
    public void addPart(BodyPart bp)//only call after checking partSpaceAbailable. Can replace Disconnected parts.
    {
        Debug.Log("gainpart" + bp);
        if (bp == BodyPart.ARM)
        {
            if (leftArm != BodyPartState.ON)
            {
                leftArm = BodyPartState.ON;
                transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
            else if (rightArm != BodyPartState.ON)
            {
                rightArm = BodyPartState.ON;
                transform.GetChild(1).GetComponent<Image>().color = Color.white;
            }
        }
        else if (bp == BodyPart.LEG)
        {
            if (leftLeg == BodyPartState.OFF)
            {
                leftLeg = BodyPartState.ON;
                transform.GetChild(2).GetComponent<Image>().color = Color.white;
                transform.GetChild(6).GetComponent<Text>().enabled = true;
            }
            else if(leftLeg == BodyPartState.CONTROLLED)
            {
                leftLeg = BodyPartState.ON;
                transform.GetChild(2).GetComponent<Image>().color = Color.white;
                transform.GetChild(6).GetComponent<Text>().enabled = true;
            }
            else if (rightLeg == BodyPartState.OFF)
            {
                rightLeg = BodyPartState.ON;
                transform.GetChild(3).GetComponent<Image>().color = Color.white;
                transform.GetChild(7).GetComponent<Text>().enabled = true;
            }
            else if(rightLeg == BodyPartState.CONTROLLED)
            {
                rightLeg = BodyPartState.ON;
                transform.GetChild(3).GetComponent<Image>().color = Color.white;
                transform.GetChild(7).GetComponent<Text>().enabled = true;
            }
        }
        else if (bp == BodyPart.LEFTLEG)
        {
            if (leftLeg == BodyPartState.OFF)
            {
                leftLeg = BodyPartState.ON;
                transform.GetChild(2).GetComponent<Image>().color = Color.white;
                transform.GetChild(6).GetComponent<Text>().enabled = true;
            }
            else if (leftLeg == BodyPartState.CONTROLLED)
            {
                leftLeg = BodyPartState.ON;
                transform.GetChild(2).GetComponent<Image>().color = Color.white;
                transform.GetChild(6).GetComponent<Text>().enabled = true;
            }

        }
        else if (bp == BodyPart.RIGHTLEG)
        {
            if (rightLeg == BodyPartState.OFF)
            {
                rightLeg = BodyPartState.ON;
                transform.GetChild(3).GetComponent<Image>().color = Color.white;
                transform.GetChild(7).GetComponent<Text>().enabled = true;
            }
            else if (rightLeg == BodyPartState.CONTROLLED)
            {
                rightLeg = BodyPartState.ON;
                transform.GetChild(3).GetComponent<Image>().color = Color.white;
                transform.GetChild(7).GetComponent<Text>().enabled = true;
            }
        }
    }
    //controlPart:
    //if ON -> CONTROLLED, return the game object
    //if CONTROLLED -> return the game object
    //if OFF -> return null
    public GameObject controlPart(BodyPart bp, GameObject Player)
    {
        if (hidden) //ensures you can't remove legs before hitting the first lever
        {
            if (bp == BodyPart.LEFTLEG)
            {
                if (leftLeg == BodyPartState.ON)
                {
                    leftLeg = BodyPartState.CONTROLLED;
                    transform.GetChild(2).GetComponent<Image>().color = Color.yellow;
                    leftLegGO = Instantiate(leftLegPrefab);
                    leftLegGO.transform.parent = Player.transform.parent;
                    leftLegGO.transform.position = Player.transform.position;
                    leftLegGO.GetComponent<CharacterController>().enabled = true;
                    canMove();
                    return leftLegGO;
                }
                else if (leftLeg == BodyPartState.CONTROLLED)
                {
                    canMove();
                    return leftLegGO;
                }
                else if (leftLeg == BodyPartState.OFF)
                {
                    canMove();
                    return null;
                }
            }
            else if (bp == BodyPart.RIGHTLEG)
            {
                if (rightLeg == BodyPartState.ON)
                {
                    rightLeg = BodyPartState.CONTROLLED;
                    transform.GetChild(3).GetComponent<Image>().color = Color.yellow;
                    rightLegGO = Instantiate(rightLegPrefab);
                    rightLegGO.transform.parent = Player.transform.parent;
                    rightLegGO.transform.position = Player.transform.position;
                    rightLegGO.GetComponent<CharacterController>().enabled = true;
                    canMove();
                    return rightLegGO;
                }
                else if (rightLeg == BodyPartState.CONTROLLED)
                {
                    canMove();
                    return rightLegGO;
                }
                else if (leftLeg == BodyPartState.OFF)
                {
                    canMove();
                    return null;
                }
            }
        }
        canMove();
        return null;
    }
    public bool canMove()
    {
        if(leftLeg == BodyPartState.ON || rightLeg == BodyPartState.ON)
        {
            transform.GetChild(6).gameObject.SetActive(false);
            return true;
        }
        transform.GetChild(6).gameObject.SetActive(true); //give alert about not being able to move
        return false;
    }
}
