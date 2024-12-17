using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private GameObject timerPrefab;
    [SerializeField] private GameObject stopWatchPrefab;
    [SerializeField] private GameObject stickyNotePrefabYellow;
    [SerializeField] private GameObject stickyNotePrefabCyan;
    [SerializeField] private GameObject stickyNotePrefabPink;
    
    public void AddTimer()
    {
        GameObject timer = Instantiate(timerPrefab);
    }

    public void AddStopWatch()
    {
        GameObject stopWatch = Instantiate(stopWatchPrefab);
    }

    public void AddStickyNote(string colour)
    {
        switch (colour)
        {
            case "Yellow": GameObject yellowStickyNote = Instantiate(stickyNotePrefabYellow); break;
            case "Cyan": GameObject cyanStickyNote = Instantiate(stickyNotePrefabCyan); break;
            case "Pink": GameObject pinkStickyNote = Instantiate(stickyNotePrefabPink); break;
        }
    }
    
    public void DeleteGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    
}
