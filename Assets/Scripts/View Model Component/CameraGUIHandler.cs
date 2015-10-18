using UnityEngine;
using System.Collections;

public class CameraGUIHandler : MonoBehaviour {
    [SerializeField] GameObject selectedCamera;

    public void RotateCamera()
    {
        selectedCamera.transform.Rotate(0, +90, 0);
    }

}
