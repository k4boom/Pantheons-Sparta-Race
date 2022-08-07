using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement _pm;
    [SerializeField] private Transform paintingWall;
    // Update is called once per frame
    void Update()
    {
        switch(_pm._gameMode)
        {
            case PlayerMovement.GameMode.Running:
                transform.position = player.position + new Vector3(2.5f, 1.0f, 0);
                break;
            case PlayerMovement.GameMode.Idle:
            case PlayerMovement.GameMode.Ending:
                Vector3 target = paintingWall.position + new Vector3(10, 3, 0);
                transform.position = Vector3.MoveTowards(transform.position, target, 6.0f * Time.deltaTime);
                break;
        }
    }
}
