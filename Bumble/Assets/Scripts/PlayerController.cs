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

    void Start()
    {
        cam = Camera.main;
        trail = gameObject.GetComponentInChildren<TrailRenderer>();
        trail.time = maxTrailTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z);
            targetPos = cam.ScreenToWorldPoint(mousePos);
        }

        trail.enabled = Vector3.Distance(transform.position, targetPos) / speed <= maxTrailTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}
