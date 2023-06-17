using UnityEngine;

public class Pipe : MonoBehaviour
{
    private bool _isScored;
    private bool _isDequeued;

    private AudioSource _source;
    
    private static GameManager GameManager => GameManager.Singleton;
    
    private void Start()
     {
         _source = GetComponent<AudioSource>();
     }

    private void Update()
    {
        Transform transform1;
        (transform1 = transform).Translate(GameManager.speed * Time.deltaTime * Vector2.left);
        
        switch (transform1.position.x)
        {
            case <= -0.5f when !_isScored:
                GameManager.AddScore();
                _isScored = true;
                _source.Play();
                break;
            case <= -1.7f when !_isDequeued:
                GameManager.DequeuePipe();
                _isDequeued = true;
                break;
            case <= -6:
                Destroy(gameObject);
                break;
        }
    }
}
