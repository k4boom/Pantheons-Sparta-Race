using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorObstacle : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, _rotationSpeed * Time.deltaTime, 0));
        
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().AddForceBack();
            
        }
    }
}
