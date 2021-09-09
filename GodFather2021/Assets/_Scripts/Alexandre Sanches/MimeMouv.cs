using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MimeMouv : MonoBehaviour
{
    public PlayerController playerController;
    public float playerInputRange = 2f;

    public string currentLane;

    [Header ("Mouvement")]
    public float speed = 5;

    private Vector3 endRun;
    private Rigidbody2D rb;
    private Collider2D colliderMime;

    [Header ("Display")]
    public float wait = 1f;
    public Text uiLetters;

    public string newLetter;
    public string letter;

    [Header("Death Settings")]
    public float maxUpForce;
    public float minUpForce;
    public float maxAngle;
    public float maxTorque;
    public float minTorque;


    private void Start()
    {

        rb = gameObject.GetComponent<Rigidbody2D>();
        colliderMime = gameObject.GetComponent<Collider2D>();
        colliderMime.isTrigger = true;

        // Remise à zero de l'UI
        uiLetters.text = "";

        // Détermination de la position d'arriver
        endRun = new Vector3(-transform.position.x, transform.position.y);

        // Début du décompt pour l'affichage de la lettre
        StartCoroutine(Reveal());
    }

    private void Update()
    {
        // Déplacement du mime
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endRun, step);

        if(Input.inputString.Contains(letter) && Vector2.Distance(playerController.transform.position, transform.position) <= playerInputRange && playerController.currentLane == currentLane)
        {
            Death();
        }
        if (Application.isEditor && Input.GetKeyDown(KeyCode.E))
        {
            Death();
        }
    }


    private IEnumerator Reveal()
    {
        uiLetters.text = letter.ToUpper();

        if(newLetter != "")
        {
            yield return new WaitForSeconds(wait);
            uiLetters.text = newLetter.ToUpper();
            EnemySysteme.Instance.UpdateSprite(newLetter, this.gameObject);
            letter = newLetter;
        }

        // Destruction du gomeObject après un certain temps
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void Death()
    {
        ScoreManager.Instance.isMimeWasKick = true;
        colliderMime.enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1;
        rb.drag = 1;

        rb.AddForce(new Vector2(Random.Range(minUpForce,maxUpForce), Random.Range(-maxAngle,maxAngle)));
        rb.AddTorque(Random.Range(maxTorque, minTorque));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerController.Damaged();
            Death();
        }
    }
}
