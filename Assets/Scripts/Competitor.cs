using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Competitor : MonoBehaviour
{
    protected Vector3 _endPosition;
    protected float _limitZ;
    public Vector3 _startPosition;

    public void StartOver()
    {
        transform.position = _startPosition;
    }

}
