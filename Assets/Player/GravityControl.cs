using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityControl : MonoBehaviour
{
    [Header("Gravity Settings")]
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private InputActionReference toggleGravityAction;
    [SerializeField] private InputActionReference resetPositionAction;

    private Rigidbody2D rb;
    private bool gravityEnabled = false;
    private Vector2 originalPosition;
    private Quaternion originalRotation;
    private float originalGravityScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
        originalPosition = rb.position;
        originalRotation = rb.transform.rotation;
    }

    private void OnEnable()
    {
        toggleGravityAction.action.Enable();
        toggleGravityAction.action.performed += ToggleGravity;
        
        resetPositionAction.action.Enable();
        resetPositionAction.action.performed += ResetPosition;
    }

    private void OnDisable()
    {
        toggleGravityAction.action.performed -= ToggleGravity;
        toggleGravityAction.action.Disable();
        
        resetPositionAction.action.performed -= ResetPosition;
        resetPositionAction.action.Disable();
    }

    private void ToggleGravity(InputAction.CallbackContext context)
    {
        gravityEnabled = !gravityEnabled;

        if (gravityEnabled)
        {
            // 启用重力
            rb.gravityScale = gravityScale;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            // 禁用重力
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void ResetPosition(InputAction.CallbackContext context)
    {
        // 重置位置和旋转
        rb.position = originalPosition;
        rb.rotation = originalRotation.eulerAngles.z;
        
        // 重置物理状态
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        
        // 重置重力状态
        gravityEnabled = false;
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void ResetGravity()
    {
        gravityEnabled = false;
        rb.gravityScale = originalGravityScale;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}