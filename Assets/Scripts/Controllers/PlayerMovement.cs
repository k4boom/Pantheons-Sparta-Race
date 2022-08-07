using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Competitor
{
    private float _lastFrameFingerPositionX;
    private float _moveFactorX;

    [Header("Movement Parameters")]
    public float _speedZ;
    [SerializeField] private float _speedX;
    [SerializeField] private float _rotationFactor = 1.5f;
    [SerializeField] private float _lerpSpeed;
    //[SerializeField] Vector3 _endPosition;
    //public Vector3 _startPosition;
    //private float _limitZ;
    public float MoveFactorX => _moveFactorX;
    private float newPosZ;

    [Header("Components")]
    [SerializeField] private DrawManager drawManager;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] public Animator animator;

    //Game State Parameters
    public enum GameMode { Idle, Running, Ending };
    public GameMode _gameMode;
    private bool isForcedBack;

    private void Start()
    {
        _startPosition = transform.position;
        _limitZ = Globals.instance.limitZ;
        animator = GetComponent<Animator>();
        _gameMode = GameMode.Running;
        _endPosition = new Vector3(Globals.instance.finishX, transform.position.y, transform.position.z);
    }


    void FixedUpdate()
    {
        switch(_gameMode)
        {
            case GameMode.Idle:
                break;
            case GameMode.Running:
                InputHandler();
                MoveChar();
                GameManager.instance.FindRanking();
                break;
            case GameMode.Ending:
                BringCharToEndPoint();
                drawManager.board.BringBoardInPlace();
                drawManager.DrawInput();
                break;
        };

        if (isForcedBack) _rb.AddForce(Vector3.right * 5, ForceMode.Impulse);
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Finish Line Change
        if (collision.transform.CompareTag("Finish"))
        {
            _gameMode = GameMode.Ending;
        }

        if (collision.transform.CompareTag("ObstacleToStartOver"))
        {
            StartOver();
        }
    }

    public new void StartOver()
    {
        transform.position = _startPosition;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        animator.SetBool("isRunning", true);
        _gameMode = GameMode.Running;
        drawManager.board.GenerateTexture();
    }

    //Create Force Effect for Rotator Obstacle Hit
    public void AddForceBack()
    {
        isForcedBack = true;
        animator.SetTrigger("isForcedBack");
        StartCoroutine(ChangeStateBack());
    }


    IEnumerator ChangeStateBack()
    {
        yield return new WaitForSeconds(1f);
        isForcedBack = false;
    }

    

    private void BringCharToEndPoint()
    {
        //Bring to the Position
        _rb.MovePosition(Vector3.MoveTowards(transform.position,_endPosition, 5f * Time.fixedDeltaTime));
        //Change the Animation
        animator.SetBool("isRunning", false);

        _rb.constraints = RigidbodyConstraints.FreezePositionY;

    }

    private void MoveChar()
    {
        //MOVEMENT HANDLER WITH TRANSFORM. IT IS USED FOR ROTATION.
        newPosZ = Mathf.Clamp(transform.position.z + _speedZ * _moveFactorX, -_limitZ, _limitZ);

        Vector3 targetPos = new Vector3(transform.position.x - _speedX * Time.fixedDeltaTime, //x
                                     _rb.velocity.y,    //y
                                     Mathf.Lerp(transform.position.z, newPosZ, _lerpSpeed * Time.fixedDeltaTime)     //z
                                     );
        //_rb.MovePosition(targetPos);

        //MOVEMENT HANDLER WITH VELOCITY
        _rb.velocity = new Vector3(-_speedX, //x
                                      0,    //y
                                      _moveFactorX * _speedZ    //z
                                      );
        if (transform.position.z > _limitZ) transform.position = new Vector3(transform.position.x, transform.position.y, _limitZ);
        if (transform.position.z < -_limitZ) transform.position = new Vector3(transform.position.x, transform.position.y, -_limitZ);

        //ROTATION HANDLER
        float angle = _moveFactorX * Vector2.Angle(new Vector2(-1, targetPos.z), new Vector2(-1, 0));
        float newRotationY = Mathf.Clamp(-90 + angle * _rotationFactor, -180, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, newRotationY, 0), Time.fixedDeltaTime * 5.0f);
    }

    private void InputHandler()
    {
        //INPUT HANDLER
        if (Input.GetMouseButtonDown(0))
        {
            _lastFrameFingerPositionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {

            if (Mathf.Abs(Input.mousePosition.x - _lastFrameFingerPositionX) > 2)
            {
                _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX > 0 ? 1 : -1;
            }
            _lastFrameFingerPositionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _moveFactorX = 0f;
        } 
        else // if nothin happens, reset the movement
        {
            _moveFactorX = 0f;
        }

    }
}
