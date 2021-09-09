using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySysteme : MonoBehaviour
{
    public static EnemySysteme Instance { get; private set; }

    private PlayerController playerController;

    [Header ("Param�tre de d�ploiement")]
    public List<Transform> posPoint;
    public GameObject mimePrefab;

    private Transform spawnPoint;
    private List<Transform> spawnCanBeTake = new List<Transform>();
    private int randomeSpawn;

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


    // Permet l'apparition d'un mime � partir d'un point al�atoire ou d�fini
    // timeBetween -> temps entre chaque mime si et seulement si nbMime est sup�rieur � 1
    // isSameSpawn -> 0 = al�atoire entre les spawn ; autre (1,2,3) = spawn pr�f�fini selon la liste "posPoint"
    private IEnumerator SpawnMime(int nbMime = 1, float timeBetween = 0, int isSameSpawn = 0, bool isSameLetter = false, float propChangeValue = 0, float propChangeLine = 0)
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

            // Spawn al�atoirement
            if (isSameSpawn == 0) {
                randomeSpawn = Random.Range(0, posPoint.Count);
                spawnPoint = posPoint[randomeSpawn];
            }
            
            // Cr�ation du mime sur la sc�ne
            GameObject mimeInt = Instantiate(mimePrefab, spawnPoint);
            MimeMouv mimeMouv = mimeInt.GetComponent<MimeMouv>();
            mimeMouv.playerController = playerController;
            mimeMouv.currentLane = spawnPoint.name;

            // Choix al�atoir de la lettre et envoie l'info au mime
            if (!isSameLetter)
            {
                letterForUI = ChoiseLetter();
            }
            mimeMouv.letter = letterForUI;
            UpdateSprite(letterForUI, mimeInt);



            // Chance de changer al�atoire de la lettre et/ou l'emplacement + envoie l'info au mime
            if (ChanceSwitch(propChangeValue))
            {
                string newletterForUI = ChoiseLetter();
                mimeMouv.newLetter = newletterForUI;
            }

            if (ChanceSwitch(propChangeLine))
            {                
                if(spawnCanBeTake != null)
                {
                    spawnCanBeTake.Clear();
                }

                // copy de la liste
                foreach(Transform a in posPoint)
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

            // Vague en elle m�me
            yield return StartCoroutine(SpawnMime(waveInfo.numberMime, waveInfo.timeBetweenMime, waveInfo.spawnPosition, waveInfo.haveSameLetter, waveInfo.propChangeLetter, waveInfo.propChangeLine));
        }
    }


    // Permet de choisir al�atoirement une lettre depuis une liste pr�d�finie
    public string ChoiseLetter()
    {
        char c = letters[Random.Range(0, letters.Length)];
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