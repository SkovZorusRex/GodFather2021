using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isMoving;
    private Vector3 originalPos, targetPos;
    private Vector3 up = new Vector3(0,3.5f,0);
    private Vector3 down = new Vector3(0, -3.5f, 0);
    private float timeToMove = 0.2f;

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !isMoving && transform.position.y != 3.5f)
            StartCoroutine(MovePlayer(up));
        if (Input.GetKey(KeyCode.DownArrow) && !isMoving && transform.position.y != -3.5f)
            StartCoroutine(MovePlayer(down));
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        originalPos = transform.position;
        targetPos = originalPos + direction;

        while(elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }

    /* public Rigidbody2D rb;
    public float moveSpeed = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveDirection = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(0, moveDirection * moveSpeed);
    }
    */
}
