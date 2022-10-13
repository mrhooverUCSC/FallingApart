using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Present can pick up and stasis objects
//Only 1 object is stasis-d at a time
public class PlayerControllerPresent : PlayerController
{
    Image UIIcon;
    public override void Start()
    {
        base.Start();
        UIIcon = GameObject.FindGameObjectWithTag("CanvasPresentIcon").GetComponent<Image>();
        UIIcon.enabled = true;
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetMouseButtonDown(0) && held)
        {
            GameObject go = held;
            held.transform.parent = null;
            held = null;
            go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            go.GetComponent<Outline>().OutlineColor = Color.yellow; //mode = outline visible
            go.layer = LayerMask.NameToLayer("isGround");
            Debug.Log(go.GetComponent<Outline>().OutlineMode);
        }
    }
}
