using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private RotatableMovableObject[] controllableObjects;
    [SerializeField] private InputActionReference switchAction;
    
    private int currentObjectIndex = 0;

    private void Awake()
    {
        // 初始化所有对象，第一个为激活状态
        for (int i = 0; i < controllableObjects.Length; i++)
        {
            controllableObjects[i].SetActive(i == 0);
        }
    }

    private void OnEnable()
    {
        switchAction.action.Enable();
        switchAction.action.performed += OnSwitchObject;
    }

    private void OnDisable()
    {
        switchAction.action.performed -= OnSwitchObject;
        switchAction.action.Disable();
    }

    private void OnSwitchObject(InputAction.CallbackContext context)
    {
        // 取消当前对象的激活状态
        controllableObjects[currentObjectIndex].SetActive(false);
        
        // 切换到下一个对象
        currentObjectIndex = (currentObjectIndex + 1) % controllableObjects.Length;
        
        // 激活新对象
        controllableObjects[currentObjectIndex].SetActive(true);
    }
}