using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isMovingVer;
    private bool isMovingHor;
    private bool damaged;
    private Vector3 originalPos, targetPos;
    private Vector3 up = new Vector3(0, 3.5f, 0);
    private Vector3 down = new Vector3(0, -3.5f, 0);
    private Vector3 left = new Vector3(-5.5f, 0, 0);
    private Vector3 right = new Vector3(5.5f, 0, 0);
    //private float timeToMove = 0.2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isMovingVer && transform.position.y != 3.5f)
            StartCoroutine(MovePlayerVerticaly(up, 0.2f));
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isMovingVer && transform.position.y != -3.5f)
            StartCoroutine(MovePlayerVerticaly(down, 0.2f));
        if (Input.GetKey(KeyCode.A) && !isMovingVer && !damaged)
            Damaged();
        else if (Input.GetKey(KeyCode.A) && damaged && !isMovingVer)
        {
            StartCoroutine(MovePlayerHorizontaly(left, 0.2f));
            Invoke("SecondDamage", 0.5f);
        }
    }

    public void Damaged()
    {
        if (damaged)
        {
            SecondDamage();
        }
        else
        {
            damaged = true;
            StartCoroutine(MovePlayerHorizontaly(left, 0.2f));
            StartCoroutine(Recup(3));
        }

    }

    public IEnumerator Recup(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        if (isMovingVer)
        {
            StartCoroutine(Recup(0.2f));
        }
        else
        {
            StartCoroutine(MovePlayerHorizontaly(right, 0.6f));
            damaged = false;
        }
    }

    void SecondDamage()
    {
        GameManager.instance.GameOver();
    }

    private IEnumerator MovePlayerVerticaly(Vector3 direction, float timeToMove)
    {
        isMovingVer = true;

        float elapsedTime = 0;

        originalPos = transform.position;
        targetPos = originalPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMovingVer = false;
    }

    private IEnumerator MovePlayerHorizontaly(Vector3 direction, float timeToMove)
    {
        isMovingVer = true;

        float elapsedTime = 0;

        originalPos = transform.position;
        targetPos = originalPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMovingVer = false;
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
