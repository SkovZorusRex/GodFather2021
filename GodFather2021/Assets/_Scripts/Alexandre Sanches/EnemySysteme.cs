using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySysteme : MonoBehaviour
{
    public static EnemySysteme Instance { get; private set; }

    private PlayerController playerController;

    [Header ("Paramètre de déploiement")]
    public List<Transform> posPoint;
    public GameObject mimePrefab;
    public GameObject obstaclePrefab;


    private Transform spawnPoint;

    private string letters = "iouy"; // ABCDEFGHIJKLMNOPQRSTUVWXYZ
    private string letterForUI;

    [Header ("Sprite")]
    public Sprite I_sprite;
    public Sprite O_sprite;
    public Sprite U_sprite;
    public Sprite Y_sprite;

    [Header ("Waves")]
    public WaveManager[] waveManagers;

    private void Start()
    {
        Instance = this;
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
        StartCoroutine(StartWaves());
    }


    // Permet l'apparition d'un mime à partir d'un point aléatoire ou défini
    // timeBetween -> temps entre chaque mime si et seulement si nbMime est supérieur à 1
    // isSameSpawn -> 0 = aléatoire entre les spawn ; autre (1,2,3) = spawn préféfini selon la liste "posPoint"
    private IEnumerator SpawnMime(int nbMime = 1, float timeBetween = 0, int isSameSpawn = 0, bool isSameLetter = false, float propChangeValue = 0, float propChangeLine = 0)
    {
        // Permet de définir si le spawn est aléatoir ou prédéfini
        if(isSameSpawn != 0)
        {
            spawnPoint = posPoint[isSameSpawn-1];
        }

        // Si isSameLetter est vrai => ne changera pas
        letterForUI = ChoiseLetter(letters);


        // Apparition des Mimes 
        for (int i = 0; i < nbMime;  i++){

            // Spawn aléatoirement
            if (isSameSpawn == 0) {
                spawnPoint = posPoint[Random.Range(0, posPoint.Count)];
            }
            
            // Création du mime sur la scène
            GameObject mimeInt = Instantiate(mimePrefab, spawnPoint);
            MimeMouv mimeMouv = mimeInt.GetComponent<MimeMouv>();
            mimeMouv.playerController = playerController;
            mimeMouv.currentLane = spawnPoint.name;

            // Choix aléatoir de la lettre et envoie l'info au mime
            if (!isSameLetter)
            {
                letterForUI = ChoiseLetter(letters);
            }
            mimeMouv.letter = letterForUI;
            UpdateSprite(letterForUI, mimeInt);



            // Chance de changer aléatoire de la lettre et/ou l'emplacement + envoie l'info au mime
            if (ChanceSwitch(propChangeValue))
            {
                string newletterForUI = "";
                foreach (char a in letters)
                {
                    if(a + "" != letterForUI)
                    {
                        newletterForUI += a;
                    }
                }
                newletterForUI = ChoiseLetter(newletterForUI);
                mimeMouv.newLetter = newletterForUI;
            }

            if (ChanceSwitch(propChangeLine))
            {
                List<Transform> spawnCanBeTake = new List<Transform>();
                // copy de la liste
                foreach (Transform a in posPoint)
                {
                    if(a.name != mimeMouv.currentLane)
                    {
                        spawnCanBeTake.Add(a);
                    }
                }

                // Nouveau spawn
                Transform newSpawnPoint = spawnCanBeTake[Random.Range(0, spawnCanBeTake.Count)];
                mimeMouv.newLine = newSpawnPoint;
            }


            yield return new WaitForSeconds(timeBetween);
        }

        // Autorisation de commencer la prochiane vague
    }


    private IEnumerator SpawnObstacle(ObstacleManager[] allObstacle)
    {
        foreach(ObstacleManager a in allObstacle)
        {
            yield return new WaitForSeconds(a.timeStart);

            for (int i = 0; i < a.numberObstacle; i++)
            {
                if (a.numberLineTaken == 1)
                {
                    if (a.whichLine == 0)
                    {
                        Instantiate(obstaclePrefab, posPoint[Random.Range(0, posPoint.Count)]);
                    }
                    else
                    {
                        Instantiate(obstaclePrefab, posPoint[a.whichLine - 1]);
                    }
                }
                else
                {
                    if (a.whichLine == 0)
                    {
                        Instantiate(obstaclePrefab, posPoint[0]);
                        Instantiate(obstaclePrefab, posPoint[2]);
                    }
                    else if (a.whichLine == 1)
                    {
                        Debug.LogWarning("whichLine = 1 + numberLineTaken = 2. L'obstacle sera placé sur le point le plus au possible");
                        Instantiate(obstaclePrefab, posPoint[1]);
                        Instantiate(obstaclePrefab, posPoint[0]);

                    }
                    else
                    {
                        Instantiate(obstaclePrefab, posPoint[a.whichLine - 1]);
                        Instantiate(obstaclePrefab, posPoint[a.whichLine - 2]);
                    }
                }

                yield return new WaitForSeconds(a.timeBetweenTwo);
            }
        }
    }


    private bool ChanceSwitch(float chanceValue)
    {
        if (chanceValue != 0f && Random.Range(0f, 1f) <= chanceValue)
        {
            return true;
        }
        return false;
    }


    private IEnumerator StartWaves()
    {
        foreach (WaveManager waveInfo in waveManagers)
        {
            // Attente entre 2 vague
            yield return new WaitForSeconds(waveInfo.timeBeforeWaveStart);

            // Vague en elle même
            yield return StartCoroutine(SpawnMime(waveInfo.numberMime, waveInfo.timeBetweenMime, waveInfo.spawnPosition, waveInfo.haveSameLetter, waveInfo.propChangeLetter, waveInfo.propChangeLine));
            yield return StartCoroutine(SpawnObstacle(waveInfo.obstacleArray));
        }
    }


    // Permet de choisir aléatoirement une lettre depuis une liste prédéfinie
    public string ChoiseLetter(string choiseLetters)
    {
        char c = choiseLetters[Random.Range(0, choiseLetters.Length)];
        return c + "";
    }


    // Permet de changer le visuel du mime
    public void UpdateSprite(string character, GameObject instant)
    {
        SpriteRenderer sr = instant.gameObject.GetComponent<SpriteRenderer>();
        switch (character)
        {
            case "i":
                sr.sprite = I_sprite;
                break;
            case "o":
                sr.sprite = O_sprite;
                break;
            case "u":
                sr.sprite = U_sprite;
                break;
            case "y":
                sr.sprite = Y_sprite;
                break;
        }
    }
}