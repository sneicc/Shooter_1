using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Camera _camera;
    private PlayerInput _playerInput;
    private InputAction _walkAction;
    private CharacterController _characterController;  

    public float WalkSpeed;
    public float SprintSpeed;
    public float JumpHeight;
    public float AirSpeed;
    public float Gravity;
    public float GroundCheckDistance = 0.05f;
    public float GroundCheckDistanceInAir = 0.01f;
    public float JumpGroundingPreventionTime;
    public LayerMask GroundCheckLayers;

    [Range(0f, 100f)]
    public float MouseVerticalSensitivity;
    [Range(0f, 100f)]
    public float MouseHorizontalSensitivity;

    private float _pitch;
    private Vector3 _groundNormal;
    private float _lastTimeJumped;
    //private float _characterSpeed;

#if DEBUG
    private Vector3 _idealDirection;

#endif

    public bool IsGrounded { get; private set; } 

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _playerInput = GetComponent<PlayerInput>();
        _walkAction = _playerInput.actions.FindAction("Movement");
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        GroundCheck();

        Rotate();
        Walk();        
    }

    void GroundCheck()
    {
        // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
        float chosenGroundCheckDistance =
            IsGrounded ? (_characterController.skinWidth + GroundCheckDistance) : GroundCheckDistanceInAir;

        // reset values before the ground check
        IsGrounded = false;
        _groundNormal = Vector3.up;

        // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
        if (Time.time >= _lastTimeJumped + JumpGroundingPreventionTime)
        {
            // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(_characterController.height),
                _characterController.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, GroundCheckLayers,
                QueryTriggerInteraction.Ignore))
            {
                // storing the upward direction for the surface found
                _groundNormal = hit.normal;

                // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                // and if the slope angle is lower than the character controller's limit
                if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                    IsNormalUnderSlopeLimit(_groundNormal))
                {
                    IsGrounded = true;

                    // handle snapping to the ground
                    if (hit.distance > _characterController.skinWidth)
                    {
                        _characterController.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
    }

    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= _characterController.slopeLimit;
    }

    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * _characterController.radius);
    }

    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - _characterController.radius));
    }

    private Vector3 ApplyGravity(Vector3 direction)
    {
        direction.y = _characterController.velocity.y - Gravity;
        return direction;
    }

    private void Walk()
    {
        Vector2 readedDirection = _walkAction.ReadValue<Vector2>();
        Vector3 direction = transform.rotation.normalized * new Vector3(readedDirection.x, 0, readedDirection.y);

        if (IsGrounded) 
        {
            direction *= WalkSpeed;
            direction = GetDirectionReorientedOnSlope(direction.normalized, _groundNormal) * direction.magnitude;
        }
        else
        {
            direction *= AirSpeed;
            direction.y += _characterController.velocity.y - Gravity;
        }

#if DEBUG
        _idealDirection = direction;
#endif

        _characterController.Move(direction * Time.deltaTime);  

        if (_characterController.velocity.magnitude > 1 && IsGrounded) _animator.SetBool("IsWalking", true);
        else
        {
            _animator.StopPlayback();
            _animator.SetBool("IsWalking", false);
        }
    }

    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        Vector3 correctDirection = Vector3.Cross(slopeNormal, directionRight);
        return correctDirection.normalized;
    }

    private void Rotate()
    {
        float inputX = Input.GetAxis("Mouse X");
        float inputY = Input.GetAxis("Mouse Y");
        
        _pitch -= inputY * MouseVerticalSensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -89, 89);
        _camera.transform.localEulerAngles = Vector3.right * _pitch;

        transform.Rotate(Vector3.up * inputX * MouseHorizontalSensitivity * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (IsGrounded)
            {
                _lastTimeJumped = Time.time;
                _characterController.Move((Vector3.up * JumpHeight + _characterController.velocity) * Time.deltaTime);
            }
        }
    }

#if DEBUG
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_characterController.transform.position, _characterController.velocity + _characterController.transform.position); // real character direction
            Gizmos.DrawLine(GetCapsuleBottomHemisphere(), GetCapsuleBottomHemisphere() + _groundNormal); // normal to ground

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _idealDirection);// ideal character directon          
        }
    }
#endif
}
