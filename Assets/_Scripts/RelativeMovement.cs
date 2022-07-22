using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _rotationSpeed = 15f;
    [SerializeField] private float _jumpForce;
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

            Quaternion temporary = _camera.rotation;
            _camera.eulerAngles = new Vector3(0, _camera.eulerAngles.y, 0);
            movement = _camera.TransformDirection(movement);
            _camera.rotation = temporary;

            var direction = Quaternion.LookRotation(movement);
            _transform.rotation = Quaternion.Lerp(
                _transform.rotation, direction, _rotationSpeed * Time.deltaTime);
        }

        if (_characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                movement.y = _jumpForce;
            }
            else
            {
                movement.y += -1.5f;
            }
        }
        else
        {
            movement.y += Physics.gravity.y;
        }

        movement *= Time.deltaTime;
        _characterController.Move(movement);
    }
}
