using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MimeMouv : MonoBehaviour
{
    [Header ("Déplacement")]
    public float speed = 5;

    private Vector3 endRun;


    [Header ("Affichage lettre")]
    public float wait = 1f;
    public Text uiLetters;

    public string newLetter;
    public char letter;


    private void Start()
    {
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

    }


    private IEnumerator Reveal()
    {
        uiLetters.text = "" + letter;

        if(newLetter != "")
        {
            yield return new WaitForSeconds(wait);
            uiLetters.text = "" + newLetter;
        }

        Debug.Log(newLetter + letter);

        // Destruction du gomeObject après un certain temps
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
