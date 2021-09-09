using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveManager
{
    [Header ("Général")]
    [TextArea (3,5)]
    public string description;
    public float timeBeforeWaveStart = 2f;

    [Header ("Avanced")]
    public int numberMime;
    public float timeBetweenMime;
    public int spawnPosition = 0;
    [Range (0,1)]
    public float propChangeLetter = 0;
    [Range (0,1)]
    public float propChangeLine = 0;
    public bool haveSameLetter;
}
