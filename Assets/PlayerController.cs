using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float rotationModifier = 5f;
    private Rigidbody2D _rigidBody2D;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _rigidBody2D.velocity = new Vector2(0, jumpForce);
            transform.eulerAngles = new Vector3(0, 0, 30);
        }

        var transform1 = transform;
        transform1.eulerAngles = new Vector3(0, 0, transform1.eulerAngles.z - (rotationModifier * Time.deltaTime));
        if (transform.eulerAngles.z is < 270 and > 30)
        {
            transform.eulerAngles = new Vector3(0, 0, -90);
        }
    }

    private void OnCollisionEnter2D()
    {
        StartCoroutine(nameof(Die));
    }

    private IEnumerator Die()
    {
        var audioSource = GetComponent<AudioSource>();

        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
