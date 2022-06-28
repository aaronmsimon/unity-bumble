using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxTrailTime = 1f;

    private Camera cam;
    private Vector3 targetPos;
    private Quaternion targetRot;

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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z);
            targetPos = cam.ScreenToWorldPoint(mousePos);
            targetRot = Quaternion.identity;
        }

        //trail.enabled = Vector3.Distance(transform.position, targetPos) / speed <= maxTrailTime;

        if (currentState == State.Normal)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            transform.rotation = targetRot;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(BumbleCircle(3f, .1f, .01f, 10f));
        }
    }

    IEnumerator BumbleCircle(float bumbleTime, float circleSize, float growthRate, float bumbleSpeed)
    {
        currentState = State.Bumble;

        float endBumbleTime = Time.time + bumbleTime;
        Vector3 startPos = transform.position;
        float xPos;
        float yPos;
        Vector3 dir;
        Vector3 angle;

        while (Time.time < endBumbleTime)
        {
            xPos = Mathf.Sin(Time.time * bumbleSpeed) * circleSize + startPos.x;
            yPos = Mathf.Cos(Time.time * bumbleSpeed) * circleSize + startPos.y;

            transform.position = new Vector3(xPos, yPos, startPos.z);

            dir = startPos - transform.position;
            angle = Quaternion.FromToRotation(Vector3.up, new Vector3(dir.x, dir.y, 0)).eulerAngles;

            transform.rotation = Quaternion.Euler(0, 0, angle.z);

            circleSize += growthRate;
            
            yield return null;
        }

        targetPos = transform.position;
        targetRot = Quaternion.identity;
        currentState = State.Normal;
    }

}
