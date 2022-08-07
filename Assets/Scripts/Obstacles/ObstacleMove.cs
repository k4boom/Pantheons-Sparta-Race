using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleMove : MonoBehaviour
{
    
    [SerializeField] private float _limitZ;
    [SerializeField] private int _direction;
    [SerializeField] private float _speed;
    private float oldZ;

    private void Start()
    {
        _direction = Random.value > 0.5 ? 1 : -1;
        oldZ = transform.position.z;
    }
    void Update()
    {
        ObstacleMovement();

    }

    private void ObstacleMovement()
    {
        if(Mathf.Abs(transform.position.z - oldZ) > _limitZ)
        {
            _direction *= -1;
            transform.position = new Vector3(transform.position.x, transform.position.y, oldZ -_direction * _limitZ);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + _direction * _speed * Time.deltaTime);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Opponent"))
        {
            collision.gameObject.GetComponent<OpponentController>().StartOver();
        }
    }
}
