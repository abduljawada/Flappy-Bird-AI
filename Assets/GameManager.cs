using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    public FlappyBirdAgent flappyBirdAgent;
    
    public TMPro.TMP_Text scoreText;
    private int _score;
    
    [Header("Speed Management")]
    [SerializeField] public float speed = 1f;
    [SerializeField] private float speedIncrease = 90f;
    [SerializeField] private float initialRoundTime = 10f;
    private float _initialSpeed;
    private float _roundDistance;
    private float _currentRoundDistance;
    
    [Header("Spawning")]
    public GameObject pipePrefab;
    [SerializeField] private float spawnDistance = 10f;
    public float maxOffset = 4f;
    private float _distanceSinceSpawn;
    private readonly Queue<GameObject> _pipeQueue = new();
    private GameObject _nextPipe;
    private GameObject _previousPipe;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Screen.SetResolution(1080, 1920, true);
        _roundDistance = speed * initialRoundTime;
        _initialSpeed = speed;
    }

    private void Update()
    {
        var deltaDistance = speed * Time.deltaTime;
        _currentRoundDistance += deltaDistance;
        _distanceSinceSpawn += deltaDistance;
        
        if (_currentRoundDistance >= _roundDistance)
        {
            _currentRoundDistance = 0f;
            speed += _roundDistance / speedIncrease;
        }

        if (!(_distanceSinceSpawn >= spawnDistance)) return;
        _distanceSinceSpawn = 0;
        SpawnPipe();
    }

    private void SpawnPipe()
    {
        var position = pipePrefab.transform.position;
        var pipe = Instantiate(pipePrefab,
            new Vector3(position.x, position.y + Random.Range(-maxOffset, maxOffset), 0), Quaternion.identity);
        _pipeQueue.Enqueue(pipe);
    }

    public void AddScore()
    {
        _score++;
        scoreText.text = _score.ToString();
        flappyBirdAgent.GetPoint();
    }

    public void DequeuePipe()
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
        speed = _initialSpeed;
        _currentRoundDistance = 0;
        _distanceSinceSpawn = 0;
        
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
        SpawnPipe();
        _nextPipe = _pipeQueue.Dequeue();
        StopAllCoroutines();
    }
}
