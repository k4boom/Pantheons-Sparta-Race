using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfDonutStick : MonoBehaviour
{
    private int isDown;
    private float startYRotation;
    private float rotationSpeed = 75f;
    private bool hitStatusShouldChange = false;
    private bool shouldHit = true;

    void Start()
    {
        startYRotation = transform.rotation.eulerAngles.y;
        isDown = 0;
        //Invoke("DonutAction", 0.5f);
    }

    void DonutAction()
    {
        StartCoroutine("DonutHit");
    }

    IEnumerator DonutHit()
    {
        while(isDown.Equals(0))
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
            yield return null;
        }
        while(isDown.Equals(1))
        {
            transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
            yield return null;
        } 
        if(isDown.Equals(-1))
        {
            transform.rotation = Quaternion.Euler(0, startYRotation, 0);
            isDown = 0;
            yield return null;
        }
    }

    void DuringHitController()
    {
        //eulerAngles converts negative values to positive values in [180,360]
        if (transform.rotation.eulerAngles.z > 45 && transform.rotation.eulerAngles.z < 90)
        {
            isDown = 1;
        }

        if (isDown.Equals(1) && transform.rotation.eulerAngles.z > 350)
        {
            isDown = -1;
        }
    }
    void Update()
    {

        DuringHitController();
        BetweenHitsController();


    }

    void BetweenHitsController()
    {
        if (shouldHit)
        {
            DonutAction();
            shouldHit = false;
            hitStatusShouldChange = true;
        }

        if (hitStatusShouldChange)
        {
            hitStatusShouldChange = false;
            StartCoroutine("ChangeHitStatus");
        }
    }
    IEnumerator ChangeHitStatus()
    {
        float delay = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(delay);
        shouldHit = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Competitor>().StartOver();
    }


}
