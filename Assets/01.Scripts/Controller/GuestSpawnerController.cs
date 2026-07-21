using System.Collections;
using System.Linq;
using UnityEngine;

public class GuestSpawnerController : MonoBehaviour
{
    [SerializeField] private GuestView _guestPrefab;
    [SerializeField] private float _spawnInterval = 5.0f;
    [SerializeField] private int _initialPoolSize = 3;
    [SerializeField] private Transform _spawnPoint;
    
    private void Start()
    {
        PoolManager.Instance.CreatePool(_guestPrefab, _initialPoolSize);
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        var wait = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            Spawn();
            yield return wait;
        }
    }

    private void Spawn()
    {
        GuestView guest = PoolManager.Instance.Get(_guestPrefab, _spawnPoint.position, Quaternion.identity);
        guest.Setup();
    }

}
