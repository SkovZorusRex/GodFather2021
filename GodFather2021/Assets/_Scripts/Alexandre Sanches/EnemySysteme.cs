using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySysteme : MonoBehaviour
{
    [Header ("Param�tre de d�ploiement")]
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


    // Permet l'apparition d'un mime � partir d'un point al�atoire ou d�fini
    // timeBetween -> temps entre chaque mime si et seulement si nbMime est sup�rieur � 1
    // isSameSpawn -> 0 = al�atoire entre les spawn ; autre (1,2,3) = spawn pr�f�fini selon la liste "posPoint"
    private IEnumerator SpawnMime(int nbMime = 1, float timeBetween = 0, int isSameSpawn = 0, bool isSameLetter = false, float propChangeValue = 0)
    {
        // Permet de d�finir si le spawn est al�atoir ou pr�d�fini
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

            // Vague en elle m�me
            yield return StartCoroutine(SpawnMime(waveInfo.numberMime, waveInfo.timeBetweenMime, waveInfo.spawnPosition, waveInfo.haveSameLetter, waveInfo.propChangeLetter));
        }
    }


    // Permet de choisir al�atoirement une lettre depuis une liste pr�d�finie
    private char ChoiseLetter()
    {
        char c = letters[Random.Range(0, letters.Length)];
        return c; 
    }
}