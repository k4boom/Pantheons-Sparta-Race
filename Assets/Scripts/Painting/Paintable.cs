using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Paintable : MonoBehaviour
{
    private Vector3 _startPoint;
    private float _endPointY;
    Texture2D texture;
    private float upRisingSpeed = 10.0f;
    private void Awake()
    {
        _startPoint = transform.position;
        _endPointY = Globals.instance.mainPlayer.transform.position.y + 3.0f;
        GenerateTexture();
    }

    public void PaintTexture()
    {
        var cam = Camera.main;
        if (!Input.GetMouseButton(0))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;
        Color color = Globals.instance.paintColor;//new Color32(255, 0, 0, 1);
        var brushSize = 9;
        Color[] colors = new Color[brushSize * brushSize];
        for(int i =0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        tex.SetPixels((int)pixelUV.x, (int)pixelUV.y, brushSize, brushSize, colors);
        //tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, color);
        tex.Apply();
        Globals.instance.paintedPercentage =  CalculatePercentage();
        GameManager.instance.OutputPercentage();
    }

    public void GenerateTexture()
    {
        texture = new Texture2D(128, 128);
        GetComponent<Renderer>().material.mainTexture = texture;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, Color.white);
            }
        }
        texture.Apply();
    }


    public int CalculatePercentage()
    {
        var data = texture.GetPixels();
        float count = 0;
        float length = data.Length;
        foreach(Color pixel in data)
        {
            if(pixel == Globals.instance.paintColor)
            {
                count++;
            }
        }
        int result = (int)((count / length) * 100);

        return result;
    }

    public void BringBoardInPlace()
    {
        var endPoint = new Vector3(transform.position.x, _endPointY, transform.position.z);
        transform.position = (Vector3.MoveTowards(transform.position, endPoint, upRisingSpeed * Time.fixedDeltaTime));

    }

    public void StartOver()
    {
        transform.position = _startPoint;
    }


}
