using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PrefabHelper
{
    //[MenuItem("Custom Scripts/PrefabHelper/Nest All Prefabs in Scene")]
    static void NestAllPrefabsInScene()
    {
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        foreach (GameObject obj in rootObjects)
        {
            GameObject prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(obj);

            if (prefabRoot != null)
            {
                GameObject newCopy = (GameObject)PrefabUtility.InstantiatePrefab(prefabRoot, obj.GetComponent<Transform>());
                PrefabUtility.UnpackPrefabInstance(newCopy, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
                newCopy.GetComponent<Transform>().localPosition = Vector3.zero;
            }

            foreach(Component component in obj.GetComponents<Component>())
            {
                if (component.GetType() != typeof(Transform))
                {
                    GameObject.DestroyImmediate(component);
                }
                else
                {
                    Transform transform = (Transform)component;
                    transform.localPosition = new Vector3((int)transform.localPosition.x, 0, (int)transform.localPosition.z); // snap to grid
                    transform.localScale = Vector3.one; 
                    transform.localRotation = Quaternion.identity;
                }
            }
            obj.tag = "Untagged";
            obj.layer = 0;
        }
    }

    //[MenuItem("Custom Scripts/PrefabHelper/Snap All Prefab Roots In Scene To Grid")]
    static void SnapAllRootsInSceneToGrid()
    {
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        foreach (GameObject obj in rootObjects)
        {
            GameObject prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            Transform transform = prefabRoot.transform;
            transform.localPosition = new Vector3((int)transform.localPosition.x, 0, (int)transform.localPosition.z); // snap to grid
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;

        }
    }
}