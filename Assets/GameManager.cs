using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public FlappyBirdAgent flappyBirdAgent;
    public GameObject pipePrefab;
    public float maxOffset = 4f;
    public float timeBtwSpawns = 2f;
    private float _timeTillSpawn;
    private int _score;
    public static GameManager Singleton;
    public TMPro.TMP_Text scoreText;
    private Queue<GameObject> _pipeQueue = new();
    private GameObject _nextPipe;
    private GameObject _previousPipe;
    [SerializeField] private float pipeChangeDelay = 0.1f;
        
    public GameManager()
    {
        _timeTillSpawn = 0;
        _score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1080, 1920, true);
        Singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        _timeTillSpawn -= Time.deltaTime;
        if (_timeTillSpawn <= 0)
        {
            _timeTillSpawn = timeBtwSpawns;
            SpawnPipe();
        }
    }

    private void SpawnPipe()
    {
        var position = pipePrefab.transform.position;
        GameObject pipe = Instantiate(pipePrefab,
            new Vector3(position.x, position.y + Random.Range(-maxOffset, maxOffset), 0), Quaternion.identity);
        _pipeQueue.Enqueue(pipe);
    }

    public void AddScore()
    {
        _score++;
        scoreText.text = _score.ToString();
        flappyBirdAgent.GetPoint();
        StartCoroutine(DelayDequeuePipe());
    }

    IEnumerator DelayDequeuePipe()
    {
        yield return new WaitForSeconds(pipeChangeDelay);
        DequeuePipe();
    }

    private void DequeuePipe()
    {
        _previousPipe = _nextPipe;
        _nextPipe = _pipeQueue.Dequeue();
    }

    public Vector2 GetNextPipePos()
    {
        return _nextPipe.transform.position;
    }

    private void EmptyQueue()
    {
        _pipeQueue.Clear();
    }

    private void ResetScore()
    {
        _score = 0;
        scoreText.text = _score.ToString();
    }

    public void ResetGame()
    {
        ResetScore();
        
        if (_nextPipe)
        {
            Destroy(_nextPipe);
        }

        if (_previousPipe)
        {
            Destroy(_previousPipe);
        }

        foreach (var pipe in _pipeQueue)
        {
            Destroy(pipe);
        }
        
        EmptyQueue();
        _timeTillSpawn = timeBtwSpawns;
        SpawnPipe();
        _nextPipe = _pipeQueue.Dequeue();
        StopAllCoroutines();
    }
}
