using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [SerializeField] public Paintable board;

    public void DrawInput()
    {
        board.PaintTexture();
        
    }
}
