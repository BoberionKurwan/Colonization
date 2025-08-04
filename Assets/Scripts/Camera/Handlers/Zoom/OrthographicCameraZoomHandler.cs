using UnityEngine;

public class OrthographicCameraZoomHandler : IZoomHandler
{
    private readonly Camera _camera;
    private readonly ZoomProperties _properties;
    private float _orthoSize;
    private float _velocity;

    public OrthographicCameraZoomHandler(Camera camera, ZoomProperties properties)
    {
        _camera = camera;
        _properties = properties;
        _orthoSize = _camera.orthographicSize;
    }

    public void Zoom(float inputDelta)
    {
        float inputDeltaWithSpeed = inputDelta * _properties._zoomSpeed;

        _orthoSize = Mathf.Clamp(_orthoSize - inputDeltaWithSpeed, _properties._zoomMin, _properties._zoomMax);

        float newOrthoSize = Mathf.SmoothDamp(
            _camera.orthographicSize,
            _orthoSize,
            ref _velocity,
            _properties._smoothness);

        _camera.orthographicSize = newOrthoSize;
    }
}
