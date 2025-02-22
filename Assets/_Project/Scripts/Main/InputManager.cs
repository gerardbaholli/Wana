using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance { get; private set; }

    public event EventHandler<MouseEventArgs> OnMouseButtonDown;
    public event EventHandler<MouseEventArgs> OnMouseButtonUp;

    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindFirstObjectByType<Camera>();
            if (mainCamera == null)
            {
                Debug.LogError("❌ No camera found on scene!");
            }
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            OnMouseButtonDown?.Invoke(this, new MouseEventArgs(worldPosition));
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            OnMouseButtonUp?.Invoke(this, new MouseEventArgs(worldPosition));
        }

    }

    public Vector2 GetMouseWorldPosition()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

}

public class MouseEventArgs : EventArgs
{
    public Vector2 mouseWorldPosition { get; }

    public MouseEventArgs(Vector2 position)
    {
        mouseWorldPosition = position;
    }

}
