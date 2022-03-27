using UnityEngine;
using UnityEngine.InputSystem;

namespace CatGame.Movement
{
    public class CatJumpSelect : MonoBehaviour
    {
        [SerializeField]
        private float _radius;

        [SerializeField]
        private float _cursorAllowance;

        [SerializeField]
        private WipCursor _cursorPrefab;
        private WipCursor _cursorInstance;

        //when you hold a direction, how long does it take to start continuous movement?
        [SerializeField]
        private float _cursorMoveWait;
        private float _moveWaitCounter;
        private bool _keysAreDown = false;
        private bool _continuousMove = false;

        //when the cursor moves continuosly, how much time should elapse between subsequent moves?
        [SerializeField]
        private float _cursorMoveSpacing;
        private float _moveSpacingCounter;

        private Keyboard _keyboard;
        private Camera _mainCam;

        void Start()
        {
            _keyboard = Keyboard.current;
            _mainCam = Camera.main;
            _cursorInstance = Instantiate(_cursorPrefab, transform.position, _cursorPrefab.transform.rotation);
            Vector3 startPosition = _cursorInstance.transform.position;
            startPosition += transform.forward * _radius;
            startPosition.x = Mathf.Floor(_cursorInstance.transform.position.x) + SurfaceManager.GRID_OFFSET;
            startPosition.z = Mathf.Floor(_cursorInstance.transform.position.z) + SurfaceManager.GRID_OFFSET;
            _cursorInstance.transform.position = startPosition;
        }

        private Vector3 GetTranslatedCoordMod(int upDownMod, int leftRightMod)
        {
            //did longform to step through the logic. didn't compress because GAMEJAM
            if (Mathf.Abs(_mainCam.transform.forward.x) > Mathf.Abs(_mainCam.transform.forward.z))
            {
                //camera along global x axis
                if (_mainCam.transform.forward.x >= 0)
                {
                    //facing global right
                    return new Vector3(upDownMod, 0, leftRightMod * -1);
                }
                else
                {
                    //facing global left
                    return new Vector3(upDownMod * -1, 0, leftRightMod);
                }
            }
            else
            {
                //camera along global z axis
                if (_mainCam.transform.forward.z >= 0)
                {
                    //facing global forward
                    return new Vector3(leftRightMod, 0, upDownMod);
                }
                else
                {
                    //facing global backward
                    return new Vector3(leftRightMod * -1, 0, upDownMod * -1);
                }
            }
        }

        private void FixedUpdate()
        {
            //TODO:replace this key stuff with proper input code, using movement axis floats
            int upDownMod = 0;
            if (_keyboard.iKey.isPressed != _keyboard.kKey.isPressed)
            {
                upDownMod = _keyboard.iKey.isPressed ? 1 : -1;
            }

            int leftRightMod = 0;
            if (_keyboard.jKey.isPressed != _keyboard.lKey.isPressed)
            {
                leftRightMod = _keyboard.lKey.isPressed ? 1 : -1;
            }

            //no inputs. skip the rest
            if (upDownMod == 0 && leftRightMod == 0)
            {
                _keysAreDown = false;
                _continuousMove = false;
                _moveSpacingCounter = 0f;
                _moveWaitCounter = 0f;
                return;
            }

            bool triggerMove = false;

            if (_continuousMove)
            {
                //already in continuous move mode. check spacing time
                _moveSpacingCounter -= Time.deltaTime;
                if (_moveSpacingCounter <= 0f)
                {
                    //fire another move and reset the held move counter
                    triggerMove = true;
                    _moveSpacingCounter = _cursorMoveSpacing;
                }
            }
            else if (_keysAreDown)
            {
                //keys already down. check delay time
                _moveWaitCounter -= Time.fixedDeltaTime;
                if (_moveWaitCounter <= 0f)
                {
                    //we are now in continuous move mode. trigger a move and start the spacing counter for the first time
                    _continuousMove = true;
                    triggerMove = true;
                    _moveSpacingCounter = _cursorMoveSpacing;
                }
            }
            else
            {
                _keysAreDown = true;
                _moveWaitCounter = _cursorMoveWait;
                triggerMove = true;
            }

            //bail if we are not supposed to move
            if (!triggerMove)
            {
                return;
            }

            Vector3 newPosition = _cursorInstance.transform.position + GetTranslatedCoordMod(upDownMod, leftRightMod);
            //TODO: move this to init when we no longer want to mess with radius the fly for testing
            float detectRange = _radius + _cursorAllowance;
            if (Vector3.Distance(transform.position, newPosition) >= detectRange)
            {
                //new position is not good. bail
                return;
            }
            float safeY;
            if (!SurfaceManager.TryGetHeight(newPosition.x, newPosition.z, out safeY))
            {
                //no safe spot! BAIL
                //TODO: handle pits by snapping to closest safe target
                return;
            }
            newPosition.y = safeY;
            _cursorInstance.transform.position = newPosition;
        }
    }

}
