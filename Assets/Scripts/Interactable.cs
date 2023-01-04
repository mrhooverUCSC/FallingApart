using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//Interactables need to be in the "isInteractable" layer, have a Mesh Collider, and an Outline set to "Outline Visible" with 0 opacity
public class Interactable : MonoBehaviour
{
    public BodyPart partNeeded;
    public bool usePart; //false if the part is not used in the operation. true if part is used in operation
    public bool partInUse; //false if no body part on it, true if there is a body part on it
    public bool loosePart; //deletes itself if it's just a body part
    public UnityEvent onDestroyEvent; //Event to tell pressure plates when destroyed
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
        if (loosePart)
        {
            onDestroyEvent.Invoke(); //invoke event when being destroyed
            Destroy(gameObject);
        }
    }
}
