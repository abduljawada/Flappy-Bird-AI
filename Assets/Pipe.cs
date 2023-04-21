using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float speed = 1f;
    private bool _isScored;

    private AudioSource _source;
    // Start is called before the first frame update
    void Start()
     {
         _source = GetComponent<AudioSource>();
     }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        if (transform.position.x <= -0.5f && !_isScored)
        {
            GameManager.Singleton.AddScore();
            _isScored = true;
            _source.Play();
        }
        if (transform.position.x <= -6)
        {
            Destroy(gameObject);
        }
    }
}
