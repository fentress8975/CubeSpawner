using System.Collections;
using UnityEngine;


public class SpawnManager : SingletonMono<SpawnManager>
{
    private const int POOL_SIZE_CORRECTION = 2;

    private float m_ObjectSpeed = 0;
    private float m_ObjectDistanceLimit = 0;
    private float m_SpawnCooldown = 0;
    private enum SpawnerState
    {
        Deactivated,
        Activated
    }
    [SerializeField] private SpawnerState m_State = SpawnerState.Deactivated;
    private float m_Delay = 0;

    [SerializeField] private Transform m_SpawnPosition;
    [SerializeField] private CubeBehaviour m_Prefab;

    private ObjectPool<CubeBehaviour> m_Pool;

    private void Start()
    {
        if (m_SpawnPosition == null)
        {
            m_SpawnPosition = transform;
        }
        m_Pool = new ObjectPool<CubeBehaviour>(m_Prefab, 1);
        UIManager.Instance.Initialize(m_ObjectSpeed, m_ObjectDistanceLimit, m_SpawnCooldown, m_Pool.CurrentPoolSize);
        SubcribeToUIActions();
        CalculatePoolSize();
        StartCoroutine(PoolSize());
    }

    private void SubcribeToUIActions()
    {
        UIManager.Instance.SpeedChange += UpdateSpeed;
        UIManager.Instance.DistanceChange += UpdateDistance;
        UIManager.Instance.SpawnRateChange += UpdateSpawnRate;
    }

    private void StartSpawner()
    {
        Debug.Log("Spawner Enabled");
        m_State = SpawnerState.Activated;
    }

    private void StopSpawner()
    {
        Debug.Log("Spawner Disabled");
        m_State = SpawnerState.Deactivated;
    }

    private void GetCubeFromPool()
    {
        var cube = m_Pool.GetObjectFromPool();
        cube.Initialize(m_ObjectSpeed, m_ObjectDistanceLimit);
        cube.transform.position = m_SpawnPosition.position;
        cube.iSKilled += ReturnCubeToPool;

    }

    private void ReturnCubeToPool(CubeBehaviour target)
    {
        m_Pool.ReturnToPool(target);
    }

    private void CalculatePoolSize()
    {
        if (isDataCorrect())
        {
            int size = Mathf.RoundToInt(m_ObjectDistanceLimit / m_ObjectSpeed / m_SpawnCooldown);
            Debug.Log("Pool size is " + (size + POOL_SIZE_CORRECTION));
            m_Pool.ChangePoolSize(size + POOL_SIZE_CORRECTION);
            StartSpawner();
        }
    }

    private bool isDataCorrect()
    {
        if (m_ObjectDistanceLimit > 0 && m_ObjectSpeed > 0 && m_SpawnCooldown > 0)
        {
            return true;
        }
        else
        {
            StopSpawner();
            return false;
        }
    }

    private void UpdateSpeed(float speed)
    {
        m_ObjectSpeed = speed;
        CalculatePoolSize();
    }
    private void UpdateDistance(float distance)
    {
        m_ObjectDistanceLimit = distance;
        CalculatePoolSize();
    }
    private void UpdateSpawnRate(float spawnRate)
    {
        m_SpawnCooldown = spawnRate;
        m_Delay = m_SpawnCooldown;
        CalculatePoolSize();
    }

    private IEnumerator PoolSize()
    {
        while (true)
        {
            UIManager.Instance.ChangePoolSizeText(m_Pool.CurrentPoolSize);
            yield return new WaitForSeconds(2);
        }
    }

    private void WaitForDelay()
    {
        if (m_Delay > 0)
        {
            m_Delay -= Time.fixedDeltaTime;

            if (m_Delay <= 0)
            {
                GetCubeFromPool();
                m_Delay = m_SpawnCooldown;
            }
        }
    }

    private void FixedUpdate()
    {
        switch (m_State)
        {
            case SpawnerState.Deactivated:

                break;
            case SpawnerState.Activated:
                if (isDataCorrect())
                {
                    WaitForDelay();
                }
                break;
            default:
                break;
        }
    }
}