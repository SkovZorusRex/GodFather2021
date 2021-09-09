using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MimeMouv : MonoBehaviour
{
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            Death();
        }
    }


    private IEnumerator Reveal()
    {
        uiLetters.text = letter;

        if(newLetter != "")
        {
            yield return new WaitForSeconds(wait);
            uiLetters.text = newLetter;
            EnemySysteme.Instance.UpdateSprite(newLetter, this.gameObject);
        }

        // Destruction du gomeObject après un certain temps
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void Death()
    {
        Debug.Log("ouiiiiiiiiiii");
        colliderMime.enabled = false;
        rb.gravityScale = 1;
        rb.drag = 1;

        rb.AddForce(new Vector2(Random.Range(minUpForce,maxUpForce), Random.Range(-maxAngle,maxAngle)));
        rb.AddTorque(Random.Range(maxTorque, minTorque));
    }
}
