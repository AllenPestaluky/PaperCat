using System.Collections.Generic;
using UnityEngine;

namespace CatGame.Movement
{
    //ABANDONDED FOR NOW. Forgot it's a Game Jam. :P
    public class SurfaceManager : MonoBehaviour
    {

        [SerializeField]
        private Transform _positiveXZ;

        [SerializeField]
        private Transform _negativeXZ;

        private Dictionary<int, Dictionary<int, float>> _heightByZByX = new Dictionary<int, Dictionary<int, float>>();

        private const float GRID_OFFSET = 0.5f;

        private SurfaceManager _instance;

        private void Awake()
        {
            Debug.Assert(_positiveXZ != null, "SurfaceManager.PositiveXZ == null!");
            Debug.Assert(_negativeXZ != null, "SurfaceManager.NegativeXZ == null!");
            if (_positiveXZ != null && _negativeXZ != null)
            {
                _heightByZByX.Clear();

                //iterate through level (in bounds of markers)
                for (int x = Mathf.FloorToInt(_negativeXZ.position.x); x <= _positiveXZ.position.x; x++)
                {
                    Dictionary<int, float> heightByZ;
                    if (!_heightByZByX.TryGetValue(x, out heightByZ))
                    {
                        heightByZ = new Dictionary<int, float>();
                    }
                    for (int z = Mathf.FloorToInt(_negativeXZ.position.z); z <= _positiveXZ.position.z; z++)
                    {
                        //first, use this point for distance calculation, ignoring y
                        Vector3 rayOrigin = new Vector3(x, Mathf.Max(_negativeXZ.position.y, _positiveXZ.position.y), z);
                        if (Physics.Raycast(rayOrigin, transform.TransformDirection(Vector3.down), out RaycastHit hit))
                        {
                            if (heightByZ.ContainsKey(z))
                            {
                                heightByZ[z] = hit.point.y;
                            } else
                            {
                                heightByZ.Add(z, hit.point.y);
                            }
                        } else if (heightByZ.ContainsKey(z))
                        {
                            //remove float from map (there is a pit)
                            heightByZ.Remove(z);
                        }
                    }
                }
            }
        }

    }
}