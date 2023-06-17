using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class FlappyBirdAgent : Agent
{
    private Rigidbody2D _rb2d;
    private bool _isDead;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallGravity = 3f;
    private bool _earnedPoint;

    public override void Initialize()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        //Debug.Log("Starting Episode");
        // Reset the bird's position and velocity
        transform.position = new Vector3(-0.5f, 0f, 0f);
        _rb2d.velocity = Vector2.zero;
        _isDead = false;
        
        GameManager.Singleton.ResetGame();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe bird's position and velocity
        var pos = transform.position;
        sensor.AddObservation(pos.y);
        sensor.AddObservation(_rb2d.velocity.y);

        // Observe distance and height to next pipe
        Vector2 pipePos = GameManager.Singleton.GetNextPipePos();
        //Debug.Log(pipePos);
        sensor.AddObservation(pipePos.x - pos.x);
        sensor.AddObservation(pipePos.y - pos.y);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetKey(KeyCode.Space)? 1 : 0;
    }
        

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Jump if action is greater than 0.5
        if (actionBuffers.ContinuousActions[0] > 0.5)
        {
            //Debug.Log("Jumping");
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            _rb2d.gravityScale = 1f;
        }
        else if (_rb2d.velocity.y < 0f)
        {
            _rb2d.gravityScale = fallGravity;
        }

        if (_isDead)
        {
            const float reward = -10.0f;
            //Debug.Log(reward + " " + Time.time);
            SetReward(reward);
            EndEpisode();
        }
        else if (_earnedPoint)
        {
            const float reward = 10.0f;
            _earnedPoint = false;
            //Debug.Log(reward + " " + Time.time);
            SetReward(reward);
        }
        else
        {
            var reward = 1 / (Math.Abs(GameManager.Singleton.GetNextPipePos().y - transform.position.y) + 0.1f);
            //Debug.Log(reward + " " + Time.time);
            SetReward(reward);
        }
            
    }

    private void OnCollisionEnter2D()
    {
        //Debug.Log("Dying");
        _isDead = true;
    }

    public void GetPoint()
    {
        _earnedPoint = true;
    }
}