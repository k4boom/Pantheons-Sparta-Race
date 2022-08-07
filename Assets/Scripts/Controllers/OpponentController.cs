using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class OpponentController : Competitor
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private LineRenderer line;
    
    void Start()
    {
        agent.speed = Random.Range(1.0f, 2.0f);
        agent = GetComponent<NavMeshAgent>();
        _startPosition = transform.position;

    }

    public void SetTargetVector(Transform finishLine)
    {
        Vector3 endPosition = new Vector3(finishLine.position.x, 0, Random.Range(_limitZ, _limitZ));
        agent.SetDestination(endPosition);//Vector3.MoveTowards(transform.position, _endPosition, 5f));
    }

    public new void StartOver()
    {
        agent.Warp(_startPosition);
        SetTargetVector(GameManager.instance.finishLine);
    }

    IEnumerator OffMeshHandler()
    {
        yield return new WaitForSeconds(0.5f);
        agent.autoTraverseOffMeshLink = true;
    }

    /// <summary>
    /// FUNCTIONS THAT USE LINERENDERER TO DEBUG PATHFINDING
    /// </summary>
    void getPath()
    {
        line.SetPosition(0, transform.position); //set the line's origin

        DrawPath(agent.path);

    }

    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        line.SetVertexCount(path.corners.Length); //set the array of positions to the amount of cornerss

        for (var i = 1; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }

}
