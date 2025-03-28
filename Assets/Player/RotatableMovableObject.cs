using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class RotatableMovableObject : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minXPosition = -5f; // 最小X位置
    [SerializeField] private float maxXPosition = 5f;  // 最大X位置
    [SerializeField] private float rotationSpeed = 180f; // 度/秒
    
    [Header("Input Settings")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference rotateAction;
    [SerializeField] private InputActionReference switchAction;

    private Rigidbody2D rb;
    private bool isActive = false;
    private float currentMoveInput;
    private float currentRotateInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (isActive)
        {
            EnableInput();
        }
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void FixedUpdate()
    {
        if (!isActive) return;
        
        // 处理持续移动
        HandleMovement();
        // 处理持续旋转
        HandleRotation();
    }

    public void SetActive(bool active)
    {
        isActive = active;
        
        if (active)
        {
            EnableInput();
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            DisableInput();
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void EnableInput()
    {
        moveAction.action.Enable();
        rotateAction.action.Enable();
        
        moveAction.action.performed += OnMovePerformed;
        moveAction.action.canceled += OnMoveCanceled;
        
        rotateAction.action.performed += OnRotatePerformed;
        rotateAction.action.canceled += OnRotateCanceled;
    }

    private void DisableInput()
    {
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;
        
        rotateAction.action.performed -= OnRotatePerformed;
        rotateAction.action.canceled -= OnRotateCanceled;
        
        moveAction.action.Disable();
        rotateAction.action.Disable();
        
        currentMoveInput = 0f;
        currentRotateInput = 0f;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        currentMoveInput = context.ReadValue<float>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        currentMoveInput = 0f;
    }

    private void OnRotatePerformed(InputAction.CallbackContext context)
    {
        currentRotateInput = context.ReadValue<float>();
    }

    private void OnRotateCanceled(InputAction.CallbackContext context)
    {
        currentRotateInput = 0f;
    }

    private void HandleMovement()
    {
        if (currentMoveInput == 0f) return;
        
        float moveAmount = currentMoveInput * moveSpeed * Time.fixedDeltaTime;
        float newX = Mathf.Clamp(rb.position.x + moveAmount, minXPosition, maxXPosition);
        
        rb.MovePosition(new Vector2(newX, rb.position.y));
    }

    private void HandleRotation()
    {
        if (currentRotateInput == 0f) return;
        
        float rotationAmount = -currentRotateInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);
    }

    private void OnDrawGizmosSelected()
    {
        // 绘制移动范围可视化
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position;
        center.x = (minXPosition + maxXPosition) / 2f;
        float width = maxXPosition - minXPosition;
        Gizmos.DrawWireCube(center, new Vector3(width, 1f, 1f));
    }
}