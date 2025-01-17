﻿using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovement : MonoBehaviour, IMovable
{
    [SerializeField] float maxSpeed, startSpeed;
    bool destinationSet = false;
    public Vector3 CurrentDestination { get; private set; }

    public float Speed => agent.speed;

    [SerializeField]
    private NavMeshAgent agent;

    public event Action OnDestinationReached;
    public event Action OnStartMoving;

    private void Awake()
    {
        agent.speed = startSpeed;
    }

    public void SetPriority(int value)
    {
        agent.avoidancePriority = value;
    }

    public void MoveTo(Vector3 position)
    {
        if (agent.isActiveAndEnabled)
        {
            OnStartMoving?.Invoke();
            agent.SetDestination(position);
            CurrentDestination = position;
        }
    }

    public void Stop()
    {
        agent.isStopped = true;
    }

    public bool isMoving()
    {
        return agent.hasPath;
    }

    private void Update()
    {
        float dist = agent.remainingDistance;
        //if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= .3f)
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            OnDestinationReached?.Invoke();
        }
    }

    public void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    public void SetQuality(bool state)
    {
        if (!state)
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
        else
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
    }

    public void ChangeSpeed(float value)
    {
        agent.speed = value;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetDefaultSpeed()
    {
        return startSpeed;
    }
}
