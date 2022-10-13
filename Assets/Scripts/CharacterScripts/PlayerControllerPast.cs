using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Past has the ability to set a bookmark and teleport back to it
//RMB to set, LMB to go to
public class PlayerControllerPast : PlayerController
{
    Vector3 bookmark;
    bool bookmarkSaved = false;
    Image UIIcon;
    GameObject floorIconPrefab;
    GameObject floorIcon;
    public override void Start()
    {
        base.Start();
        floorIconPrefab = Resources.Load("Prefabs/PastFloorIcon") as GameObject;
        UIIcon = GameObject.FindGameObjectWithTag("CanvasPastIcon").GetComponent<Image>();
        UIIcon.enabled = true;
    }

    public override void Update()
    {
        base.Update(); //call parent update
        //Debug.Log(GetComponent<Transform>().position);
        if (Input.GetMouseButtonDown(1)) //on RMB, set a teleport
        {
            if (bookmarkSaved)
            {
                Destroy(floorIcon);
            }
            bookmarkSaved = true;
            bookmark = GetComponent<Transform>().position;
            floorIcon = Instantiate(floorIconPrefab);
            floorIcon.transform.position = GetComponent<Transform>().position - new Vector3(0,playerHeight * 0.48f, 0);
            UIIcon.color = new Color(1, 0, 1, 1);
        }
        if(Input.GetMouseButtonDown(0) && bookmarkSaved) //if a teleport set, LMB to go to it
        {
            GetComponent<Transform>().position = bookmark;
            bookmarkSaved = false;
            Destroy(floorIcon);
            UIIcon.color = new Color(1, 1, 1, 1);
        }
    }
}
