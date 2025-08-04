using UnityEngine;

public class PerspectiveCameraZoomHandler : IZoomHandler
{
    private readonly Camera _camera;
    private readonly ZoomProperties _properties;
    private float _fov;
    private float _velocity;

    public PerspectiveCameraZoomHandler(Camera camera, ZoomProperties properties)
    {
        _camera = camera;
        _properties = properties;
        _fov = _camera.fieldOfView;
    }

    public void Zoom(float inputDelta)
    {
        float inputDeltaWithSpeed = inputDelta * _properties._zoomSpeed;

        _fov = Mathf.Clamp(_fov - inputDeltaWithSpeed, _properties._zoomMin, _properties._zoomMax);

        float newFov = Mathf.SmoothDamp(
            _camera.fieldOfView,
            _fov,
            ref _velocity,
            _properties._smoothness);

        _camera.fieldOfView = newFov;
    }
}