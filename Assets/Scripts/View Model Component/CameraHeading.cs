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

    const int degTurn = 90;

    public void Update()
    {
        int threshold = (degTurn / 2); // How many degrees must pass before we are able to lerp again
        currentAngle = transform.eulerAngles;
        if (Input.GetKeyDown("r"))
        {
            lerp = true;
            if (targetAngle.y > 360)
                targetAngle.y -= 360;
            if ((targetAngle.y - currentAngle.y) < threshold)
                targetAngle.y += degTurn;
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
