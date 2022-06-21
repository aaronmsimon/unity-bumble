using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxTrailTime = 1f;

    private Camera cam;
    private Vector3 targetPos;

    private float bumbleTime = 1f;
    private Vector3 startPos;
    private Vector3 midPos;
    private Vector3 endPos;

    private TrailRenderer trail;

    private void Start()
    {
        cam = Camera.main;
        trail = gameObject.GetComponentInChildren<TrailRenderer>();
        trail.time = maxTrailTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z);
            targetPos = cam.ScreenToWorldPoint(mousePos);
        }

        //trail.enabled = Vector3.Distance(transform.position, targetPos) / speed <= maxTrailTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Bumble(5));
        }
    }

    private static Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p0 = Vector3.LerpUnclamped(a, b, t);
        Vector3 p1 = Vector3.LerpUnclamped(b, c, t);

        return Vector3.LerpUnclamped(p0, p1, t);
    }

    IEnumerator Bumble(int loopCount)
    {
        for (int i = 0; i < loopCount; i++)
        {
            float minRadius = 4f;
            float maxRadius = 8f;

            float endRadius = Random.Range(minRadius, maxRadius);
            float midRadius = Random.Range(minRadius, endRadius);

            startPos = transform.position;
            endPos = Random.insideUnitCircle.normalized * endRadius + (Vector2)startPos;

            Vector3 dir = endPos - startPos;
            Vector3 normal = new Vector3(dir.y, -dir.x, 0);

            Vector3 checkLine;
            Vector3 checkPoint;

            do
            {
                midPos = Random.insideUnitCircle.normalized * midRadius + (Vector2)startPos;
                checkLine = (startPos + normal) - (startPos - normal);
                checkPoint = midPos - (startPos - normal);
            }
            while (Mathf.Sign(Vector3.Cross(checkLine, checkPoint).z) < 0);

            float bumbleSpeed = 1f / bumbleTime;
            float percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime * bumbleSpeed;
                transform.position = QuadraticCurve(startPos, midPos, endPos, percent);

                yield return null;
            }

            targetPos = endPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPos, .5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(midPos, .5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPos, .5f);

        Vector3 diff = endPos - startPos;
        Vector3 normal = new Vector3(diff.y, -diff.x, 0);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(startPos + normal, .5f);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(startPos - normal, .5f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(startPos, Vector3.Magnitude(normal));

        //Debug.Log("y2+ " + (midPos - normal + startPos));
        //Debug.Log("y2- " + (midPos - startPos - normal));
    }
}
