using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastMarker : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 2, 0));
    }
}
