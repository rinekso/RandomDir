using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class CameraScript : MonoBehaviour
{
    public WebCamTexture webCameraTexture;
    RectTransform currentRect;
    public RectTransform buttonPanel;
    public RawImage preview;
    public enum Mode{
        Boomerang,Selfie,Casual
    }
    Mode currentMode;
    WebCamDevice[] devices;
    int currentDevice = 0;
    public GameObject countDown;
    public GameObject countDownDesc;
    public GameObject coverWhite;
    public PhotoboothControll PC;
    // Start is called before the first frame update
    void Start()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            Init();
        }else{
            Init();
        }
        #endif
    }

    public void Init()
    {
        webCameraTexture = new WebCamTexture();
        devices = WebCamTexture.devices;
        LoadWebCamTexture();
        currentMode = Mode.Casual;
        currentRect = transform.parent.GetComponent<RectTransform>();
    }

    void OnDestroy()
    {
        webCameraTexture.Stop();
    }
    public void SwitchCam(){
        webCameraTexture.Stop();
        if(currentDevice+1 < devices.Length){
            currentDevice+=1;
        }else{
            currentDevice = 0;
        }
        LoadWebCamTexture();
    }
    void LoadWebCamTexture(){
        webCameraTexture.deviceName = devices[currentDevice].name;
        print(devices[currentDevice].name);
        webCameraTexture.Play();

        // change as user rotates iPhone or Android:

        int cwNeeded = webCameraTexture.videoRotationAngle;
        // Unity helpfully returns the _clockwise_ twist needed
        // guess nobody at Unity noticed their product works in counterclockwise:
        int ccwNeeded = -cwNeeded;

        // IF the image needs to be mirrored, it seems that it
        // ALSO needs to be spun. Strange: but true.
        if ( webCameraTexture.videoVerticallyMirrored ) ccwNeeded += 180;

        // you'll be using a UI RawImage, so simply spin the RectTransform
        GetComponent<RectTransform>().localEulerAngles = new Vector3(0f,0f,ccwNeeded);

        float videoRatio = (float)webCameraTexture.width/(float)webCameraTexture.height;

        // you'll be using an AspectRatioFitter on the Image, so simply set it
        GetComponent<AspectRatioFitter>().aspectRatio = videoRatio;

        // alert, the ONLY way to mirror a RAW image, is, the uvRect.
        // changing the scale is completely broken.
        if ( webCameraTexture.videoVerticallyMirrored )
            GetComponent<RawImage>().uvRect = new Rect(1,0,-1,1);  // means flip on vertical axis
        else
            GetComponent<RawImage>().uvRect = new Rect(0,0,1,1);  // means no flip
            
        // GetComponent<RectTransform>().sizeDelta = new Vector2(webCameraTexture.width,webCameraTexture.height);
        GetComponent<RawImage>().texture = webCameraTexture;
    }
    public void ChangeMode(int mode){
        if(mode == 0){
            currentMode = Mode.Selfie;
        }else if(mode == 1){
            currentMode = Mode.Boomerang;
        }else if(mode == 2){
            currentMode = Mode.Casual;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Texture2D[] tempTexture = new Texture2D[20];
    
    
    
    public void GalleryScene(){
        Application.LoadLevel(1);
    }
}
