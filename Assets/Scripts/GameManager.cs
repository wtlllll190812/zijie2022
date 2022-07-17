using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score
    {
        set 
        {
            _score = value;
            if (_score == endScore)
                gameEnd?.Invoke();
            targetPos = new(1.2f * score / endScore, sun.localPosition.x, sun.localPosition.z);
        }
        get { return _score; }
    }
    public int endScore;
    public Transform sun;
    public Transform player;
    public UnityEvent gameEnd;
    public bool gameStarted=false;

    public int _score;
    private Vector3 targetPos;

    void Awake()
    {
        if(instance==null)
            instance = this;
        else
            Destroy(this);
    }

    public void GameEnd()
    {
        gameStarted = false;
        //SceneManager.LoadScene(0);
    }

    void Update()
    {
        sun.localPosition = Vector3.Lerp(sun.localPosition, targetPos,0.1f);
        sun.parent.transform.LookAt(player.position,Vector3.forward);
    }
    
    public void GameStart()
    {
        score = 0;
        gameStarted = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
