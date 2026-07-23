using System;
using System.Collections;
using UnityEngine;

// 시작 위치에서 목표 지점으로 포물선을 그리며 날아가는 범용 연출 오브젝트.
// 비주얼(스프라이트)은 프리팹이 결정하므로 돈, 아이템 등 무엇이든 날릴 수 있다.
// 콜라이더 없이 연출만 담당하며, 도착 시 콜백을 호출하고 풀로 돌아간다
public class ProjectileEffect : MonoBehaviour, IPoolable
{
    [SerializeField] private float _flyDuration = 0.3f;
    [SerializeField] private float _arcHeight = 1.0f;

    public void Play(Vector3 target, Action onArrived)
    {
        StartCoroutine(FlyRoutine(target, onArrived));
    }

    private IEnumerator FlyRoutine(Vector3 target, Action onArrived)
    {
        Vector3 start = transform.position;
        Vector3 controlPoint = (start + target) * 0.5f + Vector3.up * _arcHeight;

        float elapsed = 0f;
        while (elapsed < _flyDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _flyDuration);

            Vector3 a = Vector3.Lerp(start, controlPoint, t);
            Vector3 b = Vector3.Lerp(controlPoint, target, t);
            transform.position = Vector3.Lerp(a, b, t);

            yield return null;
        }

        onArrived?.Invoke();
        PoolManager.Instance.Return(this);
    }

    public void OnSpawn() { }

    public void OnDespawn()
    {
        StopAllCoroutines();
    }
}
