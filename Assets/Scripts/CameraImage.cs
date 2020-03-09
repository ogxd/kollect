using UnityEngine;
using UnityEngine.UI;

public class CameraImage : MonoBehaviour {

    public WebCamTexture mCamera = null;

    void Start() {
        mCamera = new WebCamTexture();
        GetComponent<Image>().material.mainTexture = mCamera;
        mCamera.Play();
    }
}