using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactable : MonoBehaviour
{
    public BodyPart partNeeded;
    public bool partInUse; //false if no body part on it, true if there is a body part on it
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
    }
}
