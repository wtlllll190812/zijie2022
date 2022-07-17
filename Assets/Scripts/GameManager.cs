using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score
    {
        set 
        {
            if(score==endScore)
                gameEnd?.Invoke();
            targetPos = new(1.2f * score / endScore, sun.localPosition.x, sun.localPosition.z);
            _score = value;
        }
        get { return _score; }
    }
    public int endScore;
    public Transform sun;
    public Transform player;
    public UnityEvent gameEnd;

    private int _score;
    private Vector3 targetPos;

    void Awake()
    {
        if(instance==null)
            instance = this;
        else
            Destroy(this);
    }

    void Update()
    {
        sun.localPosition = Vector3.Lerp(sun.localPosition, targetPos,0.1f);
        sun.parent.transform.LookAt(player.position,Vector3.forward);
    }
}
