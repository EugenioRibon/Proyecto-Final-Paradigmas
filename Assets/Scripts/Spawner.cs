using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float minTime = 2f;
    public float maxTime = 4f;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        Invoke(nameof(Spawn), minTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (gameManager != null)
        {
            float speedFactor = 1f - (gameManager.level * 1f);  // Reduce el tiempo según el nivel
            minTime = Mathf.Max(0.5f, 2f - (gameManager.level * 0.2f));  
            maxTime = Mathf.Max(0.5f, 4f - (gameManager.level * 0.2f));  
        }
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);

        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }
}
