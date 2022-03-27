using UnityEngine;
using UnityEngine.InputSystem;

namespace CatGame.Movement
{
    public class CatJumpSelect : MonoBehaviour
    {
        [SerializeField]
        private float _diameter;

        [SerializeField]
        private float _cursorAllowance;

        [SerializeField]
        private WipCursor _cursorPrefab;

        private WipCursor _cursorInstance;
        //these are relative to the cat's current position, but NOT rotation
        private float _relativeX;
        private float _relativeZ;

        private Keyboard _keyboard;

        void Start()
        {
            _keyboard = Keyboard.current;
            _relativeZ = _diameter * 0.25f;
            _cursorInstance = Instantiate(_cursorPrefab, CalculateCursorPosition(), _cursorPrefab.transform.rotation);
        }

        private Vector3 CalculateCursorPosition()
        {
            return new Vector3()
            {
                x = TubeOfJumpiness.SnapCoord(transform.position.x + _relativeX),
                y = 0f,
                z = TubeOfJumpiness.SnapCoord(transform.position.z + _relativeZ)
            };
        }

        private void FixedUpdate()
        {
            float detectRange = _diameter * 0.5f + _cursorAllowance;
            float lastGoodX = _relativeX;
            float lastGoodZ = _relativeZ;
            if (_keyboard.iKey.wasPressedThisFrame)
            {
                _relativeZ++;
            }
            if (_keyboard.kKey.wasPressedThisFrame)
            {
                _relativeZ--;
            }
            if (_keyboard.jKey.wasPressedThisFrame)
            {
                _relativeX--;
            }
            if (_keyboard.lKey.wasPressedThisFrame)
            {
                _relativeX++;
            }
            Vector3 newPosition = CalculateCursorPosition();
            if (Vector3.Distance(gameObject.transform.position, newPosition) >= detectRange)
            {
                //new position is not good
                _relativeX = lastGoodX;
                _relativeZ = lastGoodZ;
                newPosition = CalculateCursorPosition();
            }
            newPosition.y = gameObject.transform.position.y + 16;
            float safeY = transform.position.y;
            if (Physics.Raycast(newPosition, transform.TransformDirection(Vector3.down), out RaycastHit hit))
            {
                safeY = hit.point.y;
            }
            newPosition.y = safeY + 0.1f;
            _cursorInstance.transform.position = newPosition;
        }
    }

}
