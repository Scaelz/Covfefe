using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovement : MonoBehaviour, IMovable
{
    [SerializeField]
    private float speed;
    public float Speed { get => speed; private set => speed = value; }
    bool destinationSet = false;
    public Vector3 CurrentDestination { get; private set; }

    [SerializeField]
    private NavMeshAgent agent;

    public event Action OnDestinationReached;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
        CurrentDestination = position;
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
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= .1f)
        {
            OnDestinationReached?.Invoke();
        }
    }
}
