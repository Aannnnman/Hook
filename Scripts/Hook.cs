using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private LayerMask _whatCanToHook;
    [SerializeField] private float _hookingMaxDistance;
    [SerializeField] private float _stopHookingDistance;
    [SerializeField] private float _hookingSpeed;

    private Vector2 _hookDirection;
    private Vector3 _mousePosition;
    private RaycastHit2D _rayCastHit;
    private bool _isHooking;

    private void Awake()
    {
        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ReleaseHook();
        }
    }

    private void FixedUpdate()
    {
        Hooking();
    }
    private void ReleaseHook()
    {
        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        _hookDirection = (_mousePosition - transform.position).normalized;
        _lineRenderer.enabled = true;
        LineRendererStartPositionUpdate();
        CheckRayCastHit();
    }

    private void Hooking()
    {
        if (_isHooking)
        {
            LineRendererStartPositionUpdate();
            _rigidbody.linearVelocity = _hookDirection * _hookingSpeed;

            if (Vector2.Distance(transform.position, _rayCastHit.point) < _stopHookingDistance)
            {
                StopHooking();
            }
        }
    }

    private void StopHooking()
    {
        _isHooking = false;
        _lineRenderer.enabled = false;
        _rigidbody.linearVelocity = Vector2.zero;
    }

    private void LineRendererStartPositionUpdate()
    {
        _lineRenderer.SetPosition(0, transform.position);
    }

    private void CheckRayCastHit()
    {
        _rayCastHit = Physics2D.Raycast(transform.position, _hookDirection, _hookingMaxDistance, _whatCanToHook);

        if (_rayCastHit.collider != null)
        {
            _lineRenderer.SetPosition(1, _rayCastHit.point);
            _isHooking = true;
            return;
        }

        StopHooking();
    }
}
