using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float rotationModifier = 5f;
    Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            rigidBody2D.velocity = new Vector2(0, jumpForce);
            transform.eulerAngles = new Vector3(0, 0, 30);
        }
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - (rotationModifier * Time.deltaTime));
        if (transform.eulerAngles.z < 270 && transform.eulerAngles.z > 30)
        {
            transform.eulerAngles = new Vector3(0, 0, -90);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine("Die");
    }
    IEnumerator Die()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
