using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    
    private Vector3 _velocity; 
    private void Start()
    {
        _velocity = Vector3.zero;
    }

    private void Update()
    {
        Vector3 targetPos = new Vector3(_followTarget.position.x, _followTarget.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, 0.2f);
    }
}
