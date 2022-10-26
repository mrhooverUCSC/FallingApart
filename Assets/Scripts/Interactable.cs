using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Interactables need to be in the "isInteractable" layer, have a Mesh Collider, and an Outline set to "Outline Visible" with 0 opacity
public class Interactable : MonoBehaviour
{
    public BodyPart partNeeded;
    public bool partInUse; //false if no body part on it, true if there is a body part on it
    public bool loosePart; //deletes itself if it's just a body part
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void interact()
    {
        Debug.Log("bye");
        if (loosePart)
        {
            Destroy(gameObject);
        }
    }
}
