using UnityEngine;

namespace InputReader
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private LayerMask _clickableMask;

        private Camera _camera;
        private MainBase.MainBase _choosenMainbase;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = _camera.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.TryGetComponent(out MainBase.MainBase mainbase))
                    {
                        _choosenMainbase = mainbase;
                    }
                    else if (IsClickedOnGround(out _))
                    {
                        _choosenMainbase = null;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (IsClickedOnGround(out Vector3 groundPosition) && _choosenMainbase != null)
                {
                    _choosenMainbase.SpawnFlag(groundPosition);
                }
            }
        }

        private bool IsClickedOnGround(out Vector3 groundPosition)
        {
            Vector3 pointerScreenPosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(pointerScreenPosition);
            bool result = Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _clickableMask.value);

            groundPosition = result ? hit.point : Vector3.zero;
            return result;
        }
    }
}