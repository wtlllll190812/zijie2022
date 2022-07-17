using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score;


    void Awake()
    {
        if(instance==null)
            instance = this;
        else
            Destroy(this);
    }
}
