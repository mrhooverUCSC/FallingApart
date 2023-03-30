using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarredRoomManager : MonoBehaviour
{
    bool[] mainBars = new bool[4]; // 0 is out, 1 is retracted. so all 1's means the door is openable

    bool[] leftLevers = new bool[5];
    [SerializeField] GameObject[] leftBars = new GameObject[5];
    [SerializeField] int[][] leftCombo = new int[][] { 
                                            new int[] {1,2 },
                                            new int[] {2,4 },
                                            new int[] {1,4 },
                                            new int[] {0,2 },
                                            new int[] {0 },
    };
    bool[] rightLevers = new bool[5];
    [SerializeField] GameObject[] rightBars = new GameObject[5];
    [SerializeField] int[][] rightCombo = new int[][] {
                                            new int[] {0 },
                                            new int[] {0,3 },
                                            new int[] {1 },
                                            new int[] {1,4 },
                                            new int[] {1 },
    };

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
        
        bool fail = false;
        for(int i = 0; i < leftCombo.Length; ++i)
        {
            fail = false;
            for(int j = 0; j < leftCombo[i].Length; ++j)
            {
                if (!leftLevers[leftCombo[i][j]])
                {
                    fail = true;
                }
            }
            if (fail)
            {
                leftBars[i].GetComponent<Animator>().SetBool("Retract", false);
            }
            else
            {
                leftBars[i].GetComponent<Animator>().SetBool("Retract", true);
            }
        }
    }

    public void rightBar(bool b, int register)
    {
        rightLevers[register] = b;

        bool fail = false;
        for (int i = 0; i < rightCombo.Length; ++i)
        {
            fail = false;
            for (int j = 0; j < rightCombo[i].Length; ++j)
            {
                if (!rightLevers[rightCombo[i][j]])
                {
                    fail = true;
                }
            }
            if (fail)
            {
                rightBars[i].GetComponent<Animator>().SetBool("Retract", false);
            }
            else
            {
                rightBars[i].GetComponent<Animator>().SetBool("Retract", true);
            }
        }
    }

}
