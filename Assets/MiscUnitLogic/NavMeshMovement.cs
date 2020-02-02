using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovement : MonoBehaviour, IMovable
{
    [SerializeField]
    private float speed;
    public float Speed { get => speed; private set => speed = value; }
    [SerializeField]
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void Stop()
    {
        agent.isStopped = true;
    }

    public bool isMoving()
    {
        return agent.hasPath;
    }
}
