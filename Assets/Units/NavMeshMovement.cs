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
    public event Action OnStartMoving;

    private void Awake()
    {
        //agent = GetComponent<NavMeshAgent>();
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
}
