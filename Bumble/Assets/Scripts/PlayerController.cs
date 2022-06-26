using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxTrailTime = 1f;

    private Camera cam;
    private Vector3 targetPos;

    private TrailRenderer trail;

    private enum State { Normal, Bumble }
    private State currentState;

    private void Start()
    {
        cam = Camera.main;

        currentState = State.Normal;

        trail = gameObject.GetComponentInChildren<TrailRenderer>();
        trail.time = maxTrailTime;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentState == State.Normal)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z);
            targetPos = cam.ScreenToWorldPoint(mousePos);
        }

        //trail.enabled = Vector3.Distance(transform.position, targetPos) / speed <= maxTrailTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Bumble(3f, .1f, .01f, .001f, .005f));
        }

    }

    IEnumerator Bumble(float bumbleTime, float circleSize, float circleSizeVelocity, float circleVelocity, float circleAcceleration)
    {
        currentState = State.Bumble;

        float endBumbleTime = Time.time + bumbleTime;
        float xPos;
        float yPos;
        Vector3 startPos = transform.position;

        while (Time.time < endBumbleTime)
        {
            xPos = Mathf.Sin(Time.time * circleVelocity) * circleSize + startPos.x;
            yPos = Mathf.Cos(Time.time * circleVelocity) * circleSize + startPos.y;

            transform.position = new Vector3(xPos, yPos, startPos.z);

            circleSize += circleSizeVelocity;
            circleVelocity += circleAcceleration;
            
            yield return null;
        }

        targetPos = transform.position;
        currentState = State.Normal;
    }

}
