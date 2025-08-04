using UnityEngine;

public class ScrollWheelCameraZoomController : CameraZoomController
{
    protected override float ReadInputDelta()
    {
        return Input.GetAxis("Mouse ScrollWheel");
    }
}
