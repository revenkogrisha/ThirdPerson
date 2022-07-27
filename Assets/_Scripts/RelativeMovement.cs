using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _rotationSpeed = 15f;
    [SerializeField] private float _jumpForce;
    [SerializeField] private AnimationCurve _yJumpCurve;
    [SerializeField] private Animator _animator;

    private float _expiredTime = 0f;
    private float _duration = 0.8f;
    private bool _isJumping = false;

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
        Vector3 movement = Vector3.zero;
        Vector3 rotation = Vector3.zero;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (horizontalInput != 0 || verticalInput != 0)
        {
            movement.x = horizontalInput;
            movement.z = verticalInput;
            movement *= _speed;
            movement = Vector3.ClampMagnitude(movement, _speed);

            _animator.SetBool("Running", true);

            Quaternion temporary = _camera.rotation;
            _camera.eulerAngles = new Vector3(0, _camera.eulerAngles.y, 0);
            movement = _camera.TransformDirection(movement);
            _camera.rotation = temporary;

            var direction = Quaternion.LookRotation(movement);
            _transform.rotation = Quaternion.Lerp(
                _transform.rotation, direction, _rotationSpeed * Time.deltaTime);
        }
        else
        {
            _animator.SetBool("Running", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            _isJumping = true;
        }

        if (_isJumping)
        {
            _expiredTime += Time.deltaTime;

            if (_expiredTime > _duration)
            {
                _animator.SetBool("Jumped", false);

                _isJumping = false;
                _expiredTime = 0f;
            }

            _animator.SetBool("Jumped", true);

            float progress = _expiredTime / _duration;

            movement.y = _yJumpCurve.Evaluate(progress);
            movement.y *= _jumpForce;
        }
        else
        {
                _animator.SetBool("Jumped", false);
        }

        movement.y += Physics.gravity.y;
        movement *= Time.deltaTime;
        _characterController.Move(movement);
    }
}
