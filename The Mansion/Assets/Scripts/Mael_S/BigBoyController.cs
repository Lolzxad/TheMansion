using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoyController : MonoBehaviour
{
    public float bigBoySpeed;

    public Transform[] moveSpots;
    private int randomSpot;

    public bool isPatrolling;
    [SerializeField] float waitTime;
    float startWaitTime;

    private void Start()
    {
        isPatrolling = true;

        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
    }

    private void Update()
    {
        if (isPatrolling)
        {
            BBMPA();
        }
    }


    public void BBMPA()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, bigBoySpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
