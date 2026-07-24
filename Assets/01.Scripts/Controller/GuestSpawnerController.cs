using System.Collections;
using System.Linq;
using UnityEngine;

public class GuestSpawnerController : MonoBehaviour
{
    [SerializeField] private GuestView _guestPrefab;
    [SerializeField] private float _spawnInterval = 5.0f;
    [SerializeField] private int _initialPoolSize = 3;
    [SerializeField] private Transform _spawnPoint;

    [Tooltip("소속 스테이지. 비워두면 부모에서 자동 탐색")]
    [SerializeField] private Stage _stage;

    private void Start()
    {
        if (_stage == null)
        {
            _stage = GetComponentInParent<Stage>();
        }

        if (_stage == null)
        {
            Debug.LogError("[GuestSpawnerController] 소속 Stage를 찾을 수 없습니다. 인스펙터에서 할당하거나 Stage 하위에 배치하세요.", this);
            return;
        }

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
        guest.Setup(_stage.Context);
    }

}
