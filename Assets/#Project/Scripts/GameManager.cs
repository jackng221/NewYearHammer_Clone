using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeScale = 2f;
    void Start()
    {
        Time.timeScale = timeScale;
    }

}
