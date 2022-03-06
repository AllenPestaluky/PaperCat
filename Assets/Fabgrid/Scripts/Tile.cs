using UnityEngine;

namespace Fabgrid
{
    [System.Serializable]
    public class Tile
    {
        public GameObject prefab;
        public Vector3 offset;
        public Vector3 size;
        public SizeCalculationOption sizeCalculationOption;
        [HideInInspector]
        public Texture2D thumbnail;
        public Category category = null;

        public GameObject prefabInstance
        {
            get
            {
                if(_prefabInstance == null && prefab != null)
                {
                    _prefabInstance = GameObject.Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
                    _prefabInstance.hideFlags = HideFlags.HideAndDontSave;
                    _prefabInstance.SetActive(false);
                }
                return _prefabInstance;
            }
        }
        private GameObject _prefabInstance = null; // Instantiated prefab instance to facilitate making sure all our properties are initialized

        public Bounds GetWorldBounds(Vector3 position, Quaternion rotation, Tilemap3D tilemap)
        {
            if (position.IsInfinity() || prefab == null)
                return new Bounds(Vector3.zero, Vector3.one);

            // PaperCat: Size was not serialized. Needs to be calculated
            // TODO: Would prefer some migration code instead.
            if(size.sqrMagnitude == 0.0f)
            {
                var bounds = FabgridUtility.GetTileWorldBounds(prefabInstance, sizeCalculationOption, tilemap);
                size = bounds.size;
                offset = bounds.center;
            }

            return new Bounds(prefabInstance.transform.rotation * offset + position, rotation * size);
        }

        public Vector3 GetCenterToSurfaceVector(Vector3 position, Quaternion rotation, Vector3 direction, Tilemap3D tilemap)
        {
            if (position.IsInfinity()) return Vector3.negativeInfinity;

            var worldBounds = GetWorldBounds(position, rotation, tilemap);
            var closestPoint = worldBounds.ClosestPoint(worldBounds.center + (direction.normalized * float.MaxValue));
            return worldBounds.center - closestPoint;
        }

        public Vector3 GetOffset(Vector3 position, Quaternion rotation, Tilemap3D tilemap)
        {
            var center = GetWorldBounds(position, rotation, tilemap).center;
            return position - center;
        }

        public float GetDistanceThroughTile(Vector3 position, Quaternion rotation, Vector3 direction)
        {
            if (position.IsInfinity()) return Mathf.NegativeInfinity;

            var copy = GameObject.Instantiate(prefab, position, rotation);

            var renderers = copy.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return 0f;

            var meshTransform = renderers[0].transform;

            var meshCollider = meshTransform.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = true;

            var size = meshCollider.bounds.size.magnitude;

            var upperPoint = meshCollider.ClosestPoint(meshCollider.transform.position + (direction * size));
            var lowerPoint = meshCollider.ClosestPoint(meshCollider.transform.position - (direction * size));
            var distance = Vector3.Distance(upperPoint, lowerPoint);

            Object.DestroyImmediate(copy);

            return distance;
        }
    }
}