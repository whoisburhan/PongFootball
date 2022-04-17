using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadyBug : MonoBehaviour
{
    [SerializeField] private Transform[] targetPositions;

    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 3.5f;

    private float waitTimer;
    private int randomSpot;

    private void Start()
    {
        randomSpot = Random.Range(0, targetPositions.Length);
        waitTimer = Random.Range(minWaitTime, maxWaitTime);
    }

    private void Update()
    {
        Vector3 vectorToTarget = targetPositions[randomSpot].position - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 5f);


        transform.position = Vector2.MoveTowards(transform.position, targetPositions[randomSpot].position, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position,targetPositions[randomSpot].position) < 0.2f)
        {
            if(waitTimer <= 0)
            {
                randomSpot = Random.Range(0, targetPositions.Length);
                waitTimer = Random.Range(minWaitTime, maxWaitTime);
            }
            else
            {
                waitTimer -= Time.deltaTime;
            }
        }
    }


}
