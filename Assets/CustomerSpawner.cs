using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField]
    float spawnFrequency = 2.0f;
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    float spawnOffset;
    float timer;

    private void Start()
    {
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnFrequency)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        Customer customer = CustomerPool.Instance.Get();
        customer.transform.position = GetSpawnPosition();
        customer.gameObject.SetActive(true);
    }


    Vector3 GetSpawnPosition()
    {
        int index = Random.Range(0, spawnPoints.Length);

        Vector3 result = new Vector3(spawnPoints[index].position.x + Random.Range(0, spawnOffset),
                                     spawnPoints[index].position.y,
                                     spawnPoints[index].position.z + Random.Range(0, spawnOffset));
        return result;
    }
}
