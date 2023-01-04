using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarredRoomManager : MonoBehaviour
{
    bool[] mainBars = new bool[4]; // 0 is out, 1 is retracted. so all 1's means the door is openable

    bool[] leftLevers = new bool[7];
    [SerializeField] GameObject[] leftBars = new GameObject[4];
    [SerializeField] int[][] leftCombo = new int[10][];


    bool[] rightLevers = new bool[7];
    bool frozen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mainBar(bool b, int register)
    {
        mainBars[register] = b;
    }

    public bool checkDoor()
    {
        for(int i = 0; i < mainBars.Length; ++i)
        {
            if(mainBars[i] == false)
            {
                return false;
            }
        }
        return true;
    }

    public void leftBar(bool b, int register)
    {
        leftLevers[register] = b;

        if(leftLevers[0] && leftLevers[1])
        {
            leftBars[0].GetComponent<Animator>().SetBool("Retract", true);
        }
        else
        {
            leftBars[0].GetComponent<Animator>().SetBool("Retract", false);
        }
        if (leftLevers[2] && leftLevers[3])
        {
            leftBars[1].GetComponent<Animator>().SetBool("Retract", true);
        }
        else
        {
            leftBars[1].GetComponent<Animator>().SetBool("Retract", false);
        }
    }

    public void rightBar(bool b, int register)
    {

    }

}
