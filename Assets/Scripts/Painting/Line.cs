using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//THIS IS DEPRECATED CLASS
//TO IMPLEMENT DRAWING, FIRST I TRIED LINE RENDERER
//BUT IT WAS NOT THE RIGHT SOLUTION OBVIOUSLY


public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private EdgeCollider2D _collider;
    [SerializeField] private GameObject paintWall;
    //[SerializeField] private Paintable paintable;
    private float upCornerY, downCornerY, leftCornerZ, rightCornerZ;

    private readonly List<Vector2> _points = new List<Vector2>();
    void Awake()
    {
        // _collider.transform.position -= transform.position;
        _renderer = GetComponent<LineRenderer>();
        //take the y and z limits
        //declare them globally
        upCornerY = paintWall.GetComponent<Renderer>().bounds.max.y;
        downCornerY = paintWall.GetComponent<Renderer>().bounds.min.y;
        rightCornerZ = paintWall.GetComponent<Renderer>().bounds.max.z;
        leftCornerZ = paintWall.GetComponent<Renderer>().bounds.min.z;

        
        

    }


    public void SetPosition(Vector3 pos)
    {
        
        //take the difference between pos and limits for y and z
        var diffY = Mathf.Min(Mathf.Abs(pos.y - downCornerY), Mathf.Abs(upCornerY - pos.y));
        var diffZ = Mathf.Min(Mathf.Abs(pos.z - leftCornerZ), Mathf.Abs(rightCornerZ - pos.z));
        //clamp the difference between normal width
        var width = Mathf.Clamp(Mathf.Min(diffY, diffZ), 0, 1.0f);
        _renderer.SetWidth(width,width);

        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount - 1, pos);
        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount - 1, pos );//+ (Vector3.up + Vector3.forward) * 1f
        
        //Paintable.instance.CalculatePercentage();
    }

    private void GenerateMesh()
    {
        MeshCollider collider = GetComponent<MeshCollider>();
        if(collider == null)
        {
            collider = gameObject.AddComponent<MeshCollider>();
        }
        Mesh mesh = new Mesh();
        _renderer.BakeMesh(mesh, true);
        collider.sharedMesh = mesh;
    }

}
