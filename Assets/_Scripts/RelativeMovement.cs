using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    public const string Jump = nameof(Jump);

    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _rotationSpeed = 15f;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _duration = 0.8f;
    [SerializeField] private AnimationCurve _yJumpCurve;
    [SerializeField] private CharacterAnimator _animator;

    private float _expiredTime = 0f;
    private bool _isJumping = false;

    private Vector3 _movement = Vector3.zero;
    private Transform _transform;
    private Transform _camera;
    private CharacterController _characterController;

    private void Awake()
    {
        _transform = transform;
        _camera = Camera.main.transform;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        TryMove();
    }

    private void TryMove()
    {
        float horizontal = Input.GetAxis(Axis.Horizontal);
        float vertical = Input.GetAxis(Axis.Vertical);

        if (horizontal != 0 || vertical != 0)
        {
            _movement = new(horizontal, 0f, vertical);
            _movement *= _speed;
            _movement = Vector3.ClampMagnitude(_movement, _speed);

            _animator.EnableRunning();

            Quaternion temporary = _camera.rotation;
            _camera.eulerAngles = new(0, _camera.eulerAngles.y, 0);
            _movement = _camera.TransformDirection(_movement);
            _camera.rotation = temporary;

            var direction = Quaternion.LookRotation(_movement);
            _transform.rotation = Quaternion.Lerp(
                _transform.rotation, direction, _rotationSpeed * Time.deltaTime);
        }
        else
        {
            _animator.DisableRunning();
        }

        TryJump();
        SetUpMovementVector();
    }

    private void TryJump()
    {
        if (Input.GetButtonDown(Jump))
        {
            _isJumping = true;
        }

        if (_isJumping)
        {
            _expiredTime += Time.deltaTime;

            if (_expiredTime > _duration)
            {
                _animator.DisableJumping();

                _isJumping = false;
                _expiredTime = 0f;
            }

            _animator.EnableJumping();

            float progress = _expiredTime / _duration;

            _movement.y = _yJumpCurve.Evaluate(progress);
            _movement.y *= _jumpForce;
        }
        else
        {
            _animator.DisableJumping();
        }
    }

    private void SetUpMovementVector()
    {
        _movement.y += Physics.gravity.y;
        _movement *= Time.deltaTime;
        _characterController.Move(_movement);
    }
}
