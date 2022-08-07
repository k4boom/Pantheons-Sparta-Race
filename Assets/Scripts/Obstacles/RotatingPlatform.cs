using UnityEngine.AI;
using System.Collections;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    private int _direction;

    private void Awake()
    {
        _direction = Random.value > 0.5 ? 1 : -1;
    }
    // Update is called once per frame
    void Update()
    {

        transform.Rotate(new Vector3(0, 0, _direction * _rotationSpeed * Time.deltaTime));
    }

   

    private void OnCollisionStay(Collision collision)
    {
        collision.rigidbody.AddForce(-Vector3.forward * 50 * _direction * Time.deltaTime, ForceMode.Impulse);
    }

}
