using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public bool loadScenesAdditively = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        int index = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            index = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            index = 5;
        }

        LoadSceneMode mode = loadScenesAdditively ? LoadSceneMode.Single : LoadSceneMode.Additive;
        HandleIntegerPress(index, mode);

    }

    void HandleIntegerPress(int index, LoadSceneMode mode)
    {
        bool loadScene = true;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i) == SceneManager.GetSceneByBuildIndex(index))
            {
                SceneManager.UnloadSceneAsync(index);
                loadScene = false;
            }
        }

        if (loadScene)
        {
            SceneManager.LoadScene(index, mode);
        }
    }

}
