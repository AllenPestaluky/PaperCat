using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CatDeath : MonoBehaviour
{
    [SerializeField]
    private float timeToDie = 0.75f;

    [SerializeField]
    private float timeToFade = 0.25f;

    [SerializeField]
    private Image deathScreen = null;

    private Transform previousCheckpoint;

    private bool isDying = false;
    private float dyingTimer = 0;

    public bool GetIsDying()
    {
        return isDying;
    }

    public void StartDying()
    {
        isDying = true;
    }

    public void UpdateCheckpoint(Transform newCheckpoint)
    {
        previousCheckpoint = newCheckpoint;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDying)
        {
            dyingTimer += Time.deltaTime;
            UpdateDeathScreen();

            if (dyingTimer > timeToDie)
            {
                FinishDying();
            }
        }
    }

    private void UpdateDeathScreen()
    {
        float fadeAmount = dyingTimer / timeToFade;

        deathScreen.color = new Color(deathScreen.color.r, deathScreen.color.g, deathScreen.color.b, fadeAmount);
    }

    private void FinishDying()
    {
        //TODO HACK restarting the scene completely for now
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        /*
         dyingTimer = 0;
         transform.position = previousCheckpoint.position;
         */
    }
}
