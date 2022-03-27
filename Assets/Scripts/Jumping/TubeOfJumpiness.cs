using UnityEditor;
using UnityEngine;

namespace CatGame.Movement
{
    [ExecuteInEditMode]
    public class TubeOfJumpiness : MonoBehaviour
    {
        private const float GRID_OFFSET = 0.5f;

        [SerializeField]
        private float _diameter;

        [SerializeField]
        private float _cursorAllowance;

        [SerializeField]
        private WipCursor _cursorPrefab;

        [SerializeField]
        private Transform _cursorHolder;

        private float _lastDiameter;
        private float _lastCursorAllowance;

        private void Update()
        {
            if (_lastDiameter != _diameter || _lastCursorAllowance != _cursorAllowance || transform.hasChanged)
            {
                _lastDiameter = _diameter;
                _lastCursorAllowance = _cursorAllowance;

                //gather cursors. remove them from holder
                WipCursor[] cursorArray = GetComponentsInChildren<WipCursor>();
                _cursorHolder.DetachChildren();
                //change the scale of the cylinder
                gameObject.transform.localScale = new Vector3(_diameter, 2f, _diameter);

                //update our pool of points
                RefreshPool(cursorArray);
            }            
        }

        public static float SnapCoord(float rawCoord)
        {
            return Mathf.Floor(rawCoord) + GRID_OFFSET;
        }

        private void RefreshPool(WipCursor[] oldCursorArray)
        {
            //TODO: manage our cursors better. Map them by grid coordinates? So that if a cursor was already placed, we can reuse it without having to raycast again
            int newCursorCount = 0;
            int reusedCursorCount = 0;

            //iterate through possible points and see which are in range of the diameter
            float startX = SnapCoord(gameObject.transform.position.x - _diameter * 0.5f);
            float startZ = SnapCoord(gameObject.transform.position.z - _diameter * 0.5f);

            float detectRange = _diameter * 0.5f + _cursorAllowance;
            for (float x = 0; x <= _diameter; x++)
            {
                for (float z = 0; z <= _diameter; z++)
                {
                    //first, use this point for distance calculation, ignoring y
                    Vector3 allPurposePoint = new Vector3(startX + x, gameObject.transform.position.y, startZ + z);
                    float distance = Vector3.Distance(gameObject.transform.position, allPurposePoint);
                    //next, reuse the point for raycasting
                    allPurposePoint.y = gameObject.transform.position.y + 16;
                    if (distance < detectRange && Physics.Raycast(allPurposePoint, transform.TransformDirection(Vector3.down), out RaycastHit hit))
                    {
                        //finally, reuse the point for positioning the target on the ground's surface
                        allPurposePoint.y = hit.point.y + 0.1f;
                        if (newCursorCount < oldCursorArray.Length)
                        {
                            reusedCursorCount++;
                            WipCursor reusedCursor = oldCursorArray[newCursorCount];
                            reusedCursor.transform.SetParent(_cursorHolder, true);
                            reusedCursor.transform.position = allPurposePoint;
                        }
                        else
                        {
                            WipCursor newCursor = Instantiate(_cursorPrefab, allPurposePoint, this.transform.rotation);
                            newCursor.transform.SetParent(_cursorHolder, true);
                        }
                        newCursorCount++;
                    }
                }
            }

            UnityEditor.EditorApplication.delayCall += () =>
            {
                TrimCursorPool(reusedCursorCount, oldCursorArray);
            };
        }

        private void TrimCursorPool(int reusedCursorCount, WipCursor[] oldCursorArray)
        {
            for (int i = reusedCursorCount; i < oldCursorArray.Length; i++)
            {
                DestroyImmediate(oldCursorArray[i].gameObject);
            }

            EditorUtility.SetDirty(this);
        }
    }

}
