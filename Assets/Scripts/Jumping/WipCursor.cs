using UnityEngine;

namespace CatGame.Movement
{
    public class WipCursor : MonoBehaviour
    {
        [SerializeField]
        private Transform _arrow;

        [SerializeField]
        private float _maxScale;

        [SerializeField]
        private float _minDistance;

        [SerializeField]
        private float _maxDistance;

        public void RotateAndScale(Vector3 cameraFacing, float distance)
        {
            _arrow.rotation = Quaternion.LookRotation(cameraFacing, Vector3.up);
            float abbyNormal;
            if (distance >= _maxDistance)
            {
                abbyNormal = 1f;
            }
            else if (distance < _minDistance)
            {
                abbyNormal = 0f;
            }
            else
            {
                float range = _maxDistance - _minDistance;
                float workingDistance = distance - _minDistance;
                abbyNormal = Mathf.Clamp(workingDistance / range, 0f, 1f);
            }
            _arrow.localScale = Vector3.one * Mathf.Lerp(1.0f, _maxScale, abbyNormal);
        }
    }

}
