using UnityEngine;

public class CustomMovementInput : CameraMovementInputBase
{
    [SerializeField] private LayerMask _clickableMask;
    [SerializeField] private float _mouseSensitivity = 1;
    [SerializeField] private float _keyboardSensitivity = 1;


    private bool _dragEnabled;
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();

        _camera = GetComponent<Camera>();
    }

    protected override ICameraMovementHandler CreateMovementHandler()
    {
        return new SmoothCameraMovementHandler(_properties);
    }

    protected override Vector3 ReadInputDelta()
    {
        Vector3 mouseInputDelta = ReadMouseDelta();
        Vector3 keyboardInputDelta = ReadKeyboardInputDelta();

        return (mouseInputDelta + keyboardInputDelta) * _camera.fieldOfView;
    }

    private Vector3 ReadMouseDelta()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsClickedOnGround())
            {
                _dragEnabled = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _dragEnabled = false;
        }

        if (_dragEnabled && Input.GetMouseButton(0))
        {
            return Input.mousePositionDelta * _mouseSensitivity;
        }

        return Vector3.zero;
    }

    private Vector3 ReadKeyboardInputDelta()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        return new Vector3(-horizontal, -vertical, 0) * _keyboardSensitivity;
    }

    private bool IsClickedOnGround()
    {
        Vector3 pointerScreenPosition = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(pointerScreenPosition);
        bool result = Physics.Raycast(ray, out _, float.MaxValue, _clickableMask.value);

        return result;
    }
}
