using UnityEditor;
using UnityEngine;

namespace CatGame.Movement
{
    [ExecuteInEditMode]
    public class TubeOfJumpiness : MonoBehaviour
    {
        private const float GRID_OFFSET = 0.5f;

        [SerializeField]
        private int _diameter;

        [SerializeField]
        private float _cursorAllowance;

        [SerializeField]
        private WipCursor _cursorPrefab;

        [SerializeField]
        private Transform _cursorHolder;

        private void Update()
        {
            //gather cursors. remove them from holder
            WipCursor[] cursorArray = GetComponentsInChildren<WipCursor>();
            _cursorHolder.DetachChildren();
            //change the scale of the cylinder
            gameObject.transform.localScale = new Vector3(_diameter, 2f, _diameter);

            //update our pool of points
            RefreshPool(cursorArray);
        }

        private static float SnapCoord(float rawCoord)
        {
            return Mathf.Floor(rawCoord) + GRID_OFFSET;
        }

        private void RefreshPool(WipCursor[] oldCursorArray)
        {
            int newCursorCount = 0;
            int reusedCursorCount = 0;

            //iterate through possible points and see which are in range of the diameter
            float startX = SnapCoord(gameObject.transform.position.x - _diameter * 0.5f);
            float startZ = SnapCoord(gameObject.transform.position.z - _diameter * 0.5f);

            float detectRange = _diameter * 0.5f + _cursorAllowance;
            for (int x = 0; x <= _diameter; x++)
            {
                for (int z = 0; z <= _diameter; z++)
                {
                    Vector3 point = new Vector3(startX + x, gameObject.transform.position.y + 0.1f, startZ + z);
                    float distance = Vector3.Distance(gameObject.transform.position, point);
                    if (distance < detectRange)
                    {
                        if (newCursorCount < oldCursorArray.Length)
                        {
                            reusedCursorCount++;
                            WipCursor reusedCursor = oldCursorArray[newCursorCount];
                            reusedCursor.transform.SetParent(_cursorHolder, true);
                            reusedCursor.transform.position = point;
                        }
                        else
                        {
                            WipCursor newCursor = Instantiate(_cursorPrefab, point, this.transform.rotation);
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
