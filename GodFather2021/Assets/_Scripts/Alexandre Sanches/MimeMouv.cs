using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MimeMouv : MonoBehaviour
{
    public PlayerController playerController;
    public float playerInputRange = 2f;

    public string currentLane;
    public ParticleSystem failHit;
    public ParticleSystem successHit;

    [Header("Mouvement")]
    public float speed = 5;

    private Rigidbody2D rb;
    private Collider2D colliderMime;

    private bool isAlive = true;

    [Header("Display")]
    public float wait = 1f;
    public Text uiLetters;


    public string letter;
    public string newLetter;
    public Transform newLine;
    private bool changeLine;
    private Vector3 endPos;


    [Header("Death Settings")]
    public float timeBeforDeath = 5;
    public float maxUpForce;
    public float minUpForce;
    public float maxAngle;
    public float maxTorque;
    public float minTorque;

    private int coup;

    private void Start()
    {
        endPos = new Vector3(-20, transform.position.y);

        rb = gameObject.GetComponent<Rigidbody2D>();
        colliderMime = gameObject.GetComponent<Collider2D>();
        colliderMime.isTrigger = true;

        // Remise à zero de l'UI
        uiLetters.text = "";

        // Début du décompt pour l'affichage de la lettre
        StartCoroutine(Reveal());
    }

    private void Update()
    {
        // Déplacement du mime
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPos, step);

        if (changeLine)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, newLine.position.y), step * 2);
        }

        if (Input.inputString.Contains(letter) && Vector2.Distance(playerController.transform.position, transform.position) <= playerInputRange && playerController.currentLane == currentLane)
        {
            if (isAlive)
            {
                StartCoroutine(PlayFX(successHit));
                Death();
                ScoreManager.Instance.isMimeWasKick = true;
                coup = Random.Range(1, 11);
                if (coup < 8) {
                    FindObjectOfType<AudioManager>().Play("Paf");
                }
                else { FindObjectOfType<AudioManager>().Play("Paf2"); 
                }
            }
        }
        else if ((!Input.inputString.Contains(letter) && Input.inputString != "") && Vector2.Distance(playerController.transform.position, transform.position) <= playerInputRange && playerController.currentLane == currentLane)
        {
            if (isAlive)
            {
                isAlive = false;
                StartCoroutine(PlayFX(failHit));
            }
        }
    }


    private IEnumerator Reveal()
    {
        uiLetters.text = letter.ToUpper();
        yield return new WaitForSeconds(wait);

        if (newLetter != "")
        {
            uiLetters.text = newLetter.ToUpper();
            EnemySysteme.Instance.UpdateSprite(newLetter, this.gameObject);
            letter = newLetter;
            FindObjectOfType<AudioManager>().Play("Changement");
        }

        if (newLine != null)
        {
            endPos = new Vector3(-20, newLine.position.y);
            changeLine = true;
        }

        // Destruction du gomeObject après un certain temps
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }


    public void Death()
    {
        isAlive = false;
        colliderMime.enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1;
        rb.drag = 1;

        rb.AddForce(new Vector2(Random.Range(minUpForce, maxUpForce), Random.Range(-maxAngle, maxAngle)));
        rb.AddTorque(Random.Range(maxTorque, minTorque));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerController.Damaged();
            Death();
        }
    }

    private IEnumerator PlayFX(ParticleSystem ps)
    {
        ps.Play();
        yield return new WaitForSeconds(0.5f);
        ps.Stop();
    }
}
