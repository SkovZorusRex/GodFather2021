using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMouv : MonoBehaviour
{
    public float speed;

    private Vector3 endPos;

    private void Start()
    {
        endPos = new Vector3(-20, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPos, step);
    }
}
