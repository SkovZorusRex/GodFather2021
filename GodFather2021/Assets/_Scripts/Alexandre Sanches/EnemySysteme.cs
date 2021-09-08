using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySysteme : MonoBehaviour
{
    [Header ("Paramètre de déploiement")]
    public Transform[] posPoint;
    public GameObject mimePrefab;

    private Transform spawnPoint;

    private string letters = "AZER"; // ABCDEFGHIJKLMNOPQRSTUVWXYZ
    private char letterForUI;

    [Header ("Waves")]
    public WaveManager[] waveManagers; // class + constructeur

    private void Start()
    {
        StartCoroutine(StartWaves());
    }


    // Permet l'apparition d'un mime à partir d'un point aléatoire ou défini
    // timeBetween -> temps entre chaque mime si et seulement si nbMime est supérieur à 1
    // isSameSpawn -> 0 = aléatoire entre les spawn ; autre (1,2,3) = spawn préféfini selon la liste "posPoint"
    private IEnumerator SpawnMime(int nbMime = 1, float timeBetween = 0, int isSameSpawn = 0, bool isSameLetter = false, float propChangeValue = 0)
    {
        // Permet de définir si le spawn est aléatoir ou prédéfini
        if(isSameSpawn != 0)
        {
            spawnPoint = posPoint[isSameSpawn-1];
        }

        // Si isSameLetter est vrai => ne changera pas
        letterForUI = ChoiseLetter();

        // Apparition des Mimes 
        for (int i = 0; i < nbMime;  i++){
            if (isSameSpawn == 0) {
                spawnPoint = posPoint[Random.Range(0, posPoint.Length)];
            }
            
            GameObject mimeInt = Instantiate(mimePrefab, spawnPoint);
            if (!isSameLetter)
            {
                letterForUI = ChoiseLetter();
            }
            mimeInt.GetComponent<MimeMouv>().letter = letterForUI;


            if(propChangeValue != 0 && Random.Range(0f,10f) <= propChangeValue)
            {
                Debug.Log(Random.Range(0f, 10f));
                mimeInt.GetComponent<MimeMouv>().newLetter = ChoiseLetter() + "";
            }

            yield return new WaitForSeconds(timeBetween);
        }

        // Autorisation de commencer la prochiane vague
    }



    private IEnumerator StartWaves()
    {
        foreach (WaveManager waveInfo in waveManagers)
        {
            // Attente entre 2 vague
            yield return new WaitForSeconds(waveInfo.timeBeforeWaveStart);

            // Vague en elle même
            yield return StartCoroutine(SpawnMime(waveInfo.numberMime, waveInfo.timeBetweenMime, waveInfo.spawnPosition, waveInfo.haveSameLetter, waveInfo.propChangeLetter));
        }
    }


    // Permet de choisir aléatoirement une lettre depuis une liste prédéfinie
    private char ChoiseLetter()
    {
        char c = letters[Random.Range(0, letters.Length)];
        return c; 
    }
}