using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ColorTile : MonoBehaviour
{
    [SerializeField] tileColor color;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.tag == "Player")
        {
            GameMananger.instance.colorTileTrigger(color);
        }
    }

}
