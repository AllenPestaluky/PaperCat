using System.Collections;
using UnityEngine;

namespace CatGame.Movement
{
    public class SurfaceManager : MonoBehaviour
    {

        [SerializeField]
        private Transform _positiveTransform;

        [SerializeField]
        private Transform _negativeTransform;

        //TODO: may need to nest an array inside 2d array for multiple platforms on same 
        private static int _xCount;
        private static int _xOffset;
        private static int _zOffset;
        private static float[,] _heightMap;
        private static bool _mapFilled = false;

        public const float GRID_OFFSET = 0.5f;
        private const int LAYER_MASK = 8;

        //debuggery
        private bool _drawCastLines = false;

        private void Awake()
        {
            if (_mapFilled)
            {
                Debug.LogError("Hey. What the heck, dude(tte)!? You seem to have ended up with more than 1 SurfaceManager in the same scene. Please don't");
            }
            if (_positiveTransform == null || _negativeTransform == null)
            {
                Debug.LogError("One of both of the tranforms (_positiveTransform, _negativeTransform) were null!");
                return;
            }
            if (_positiveTransform.position.x < _negativeTransform.position.x)
            {
                Debug.LogError("_positiveTransform has global x position less than _negativeTransform's (swap them along x axis)");
                return;
            }
            if (_positiveTransform.position.y < _negativeTransform.position.y)
            {
                Debug.LogError("_positiveTransform has global y position less than _negativeTransform's (swap them along y axis)");
                return;
            }
            if (_positiveTransform.position.z < _negativeTransform.position.z)
            {
                Debug.LogError("_positiveTransform has global z position less than _negativeTransform's (swap them along z axis)");
                return;
            }
        }

        //we cannot start raycasting around until the first fixed update
        private IEnumerator Start()
        {
            yield return new WaitForFixedUpdate();
            yield return null;

            _mapFilled = true;

            _xCount = Mathf.CeilToInt(_positiveTransform.position.x - _negativeTransform.position.x);
            int zCount = Mathf.CeilToInt(_positiveTransform.position.z - _negativeTransform.position.z);

            _xOffset = Mathf.FloorToInt(_negativeTransform.position.x);
            _zOffset = Mathf.FloorToInt(_negativeTransform.position.z);
            _heightMap = new float[_xCount, zCount];

            //iterate through level (in bounds of markers)
            for (int xIndex = 0; xIndex < _xCount; xIndex++)
            {
                int x = xIndex + _xOffset;
                for (int zIndex = 0; zIndex < zCount; zIndex++)
                {
                    int z = zIndex + _zOffset;
                    //first, use this point for distance calculation, ignoring y
                    Vector3 lineStart = new Vector3(x + GRID_OFFSET, _positiveTransform.position.y, z + GRID_OFFSET);
                    Vector3 lineEnd = new Vector3(x + GRID_OFFSET, _negativeTransform.position.y, z + GRID_OFFSET);
                    //default to NaN (no tile. it is the abyss)
                    float height;
                    if (_drawCastLines)
                    {
                        Debug.DrawLine(lineStart, lineEnd, Color.magenta, 20f, false);
                    }
                    //TODO: test only Ground layer
                    if (Physics.Linecast(lineStart, lineEnd, out RaycastHit hit))
                    {
                        height = hit.point.y;
                    }
                    else
                    {
                        height = float.NaN;
                    }
                    _heightMap[xIndex, zIndex] = height;
                }
            }
        }

        public static bool TryGetHeight(float x, float z, out float goodHeight)
        {
            goodHeight = float.NaN;
            int xIndex = Mathf.FloorToInt(x) - _xOffset;
            int zIndex = Mathf.FloorToInt(z) - _zOffset;
            if (xIndex > 0 && xIndex < _xCount & zIndex > 0 && zIndex < _heightMap.Length / _xCount)
            {
                goodHeight = _heightMap[Mathf.FloorToInt(x) - _xOffset, Mathf.FloorToInt(z) - _zOffset];
            }
            return !float.IsNaN(goodHeight);
        }

    }
}