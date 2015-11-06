using UnityEngine;
using System.Collections;

public class CameraPitch : MonoBehaviour {

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

        if (Input.GetKeyDown("e"))
        {
            lerp = true;
            if (currentAngle.x < 35)
            {
                targetAngle.x += 10;
            } else
            {
                targetAngle.x -= 10;
            }
        }
        if (lerp)
        {
            currentAngle = new Vector3(
                Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
                Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
                Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));

            transform.eulerAngles = currentAngle;
        }
    }
}
