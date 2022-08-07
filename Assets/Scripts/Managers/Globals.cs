using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals instance { get; private set; }

    [SerializeField] public float limitZ;
    [SerializeField] public float finishX;
    public Color paintColor = new Color32(255, 0, 0, 1);
    public int paintedPercentage = 0;
    public GameObject mainPlayer;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
