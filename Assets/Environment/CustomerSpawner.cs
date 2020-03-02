using System;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField]
    float spawnFrequency = 2.0f;
    float defaultFrequency;
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    float spawnOffset;
    float timer;

    public event Action<float> OnSpawnFrequencyChanged;

    private void Start()
    {
        defaultFrequency = spawnFrequency;
        PopularitySystem.Instance.OnPopularityChanged += ChangeSpawnFrequency;
        ChangeSpawnFrequency(PopularitySystem.Instance.GetPopularity());
    }

    void ChangeSpawnFrequency(float popularity)
    {
        spawnFrequency = defaultFrequency / popularity;
        OnSpawnFrequencyChanged?.Invoke(1 / spawnFrequency);
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
        int index = UnityEngine.Random.Range(0, spawnPoints.Length);

        Vector3 result = new Vector3(spawnPoints[index].position.x + UnityEngine.Random.Range(0, spawnOffset),
                                     spawnPoints[index].position.y,
                                     spawnPoints[index].position.z + UnityEngine.Random.Range(0, spawnOffset));
        return result;
    }
}
