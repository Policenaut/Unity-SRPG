using UnityEngine;
using System.Collections;

public class CameraHeading : MonoBehaviour {

    public void Start()
    {
        currentAngle = transform.eulerAngles;
        targetAngle = transform.eulerAngles;
    }

    Vector3 targetAngle;

    Vector3 currentAngle;

    bool lerp;

    public void Update()
    {
        currentAngle = transform.eulerAngles;
        if (Input.GetKeyDown("r"))
        {
            lerp = true;
            targetAngle.y += 90;
        }
        if (lerp)
        {
            currentAngle = new Vector3(
                currentAngle.x,
                Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
                currentAngle.z);

            transform.eulerAngles = currentAngle;
        }
    }
}
