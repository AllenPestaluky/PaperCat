using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCameraFree : MonoBehaviour
{
    [SerializeField]
    private float rotationPower = 3f;

    [SerializeField]
    private Transform followTarget = null;

    public void ResetCamera()
    {
        // Start freelook at 0,0,0 again
        followTarget.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void UpdateCamera(Vector2 lookInput)
    {
        // Based on https://www.youtube.com/watch?v=537B1kJp9YQ

        // Rotate the Follow Target transform based on the input
        followTarget.rotation *= Quaternion.AngleAxis(lookInput.x * rotationPower, Vector3.up);
        followTarget.rotation *= Quaternion.AngleAxis(lookInput.y * rotationPower, Vector3.right);

        // Clamp the Up/Down rotation
        var angles = followTarget.localEulerAngles;
        angles.z = 0;
        if (angles.x > 180 && angles.x < 340)
        {
            angles.x = 340;
        }
        else if (angles.x < 180 && angles.x > 40)
        {
            angles.x = 40;
        }
        followTarget.transform.localEulerAngles = angles;
    }
}   
