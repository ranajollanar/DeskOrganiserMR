using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private GameObject timerPrefab;

    public void AddTimer()
    {
        GameObject timer = Instantiate(timerPrefab);
    }
}
