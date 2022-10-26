using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ColorTile : MonoBehaviour
{
    [SerializeField] tileColor color;
    bool used; //ensures a second "Enter" doesn't occur with the legs

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.tag == "Player" && !used)
        {
            GameMananger.instance.colorTileTrigger(color);
            used = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && used)
        {
            used = false;
        }
    }

}
