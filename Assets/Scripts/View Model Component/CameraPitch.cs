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
        currentAngle = transform.eulerAngles;
        if (Input.GetKeyDown("e"))
        {
            lerp = true;
            if (currentAngle.x < 40)
            {
                targetAngle.x = 45;
            } else
            {
                targetAngle.x = 35;
            }
        }
        if (lerp)
        {
            currentAngle = new Vector3(
                Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
                currentAngle.y,
                currentAngle.z);

            transform.eulerAngles = currentAngle;
        }
    }
}
