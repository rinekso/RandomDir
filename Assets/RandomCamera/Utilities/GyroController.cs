using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroController : MonoBehaviour
{
	public bool gyroActive;
	private bool gyroEnabled;
	private Gyroscope gyro;

	private GameObject cameraContainer;
	private Quaternion rot;
	public Quaternion targetRotation;

    private float prevDir = 0;
	public float rotAzm;
	// Use this for initialization
	private void Start () {
		gyroActive = true;
		cameraContainer = new GameObject ("Camera Container");
		cameraContainer.transform.position = transform.position;
		transform.SetParent (cameraContainer.transform);

		gyroEnabled = EnabledGyro ();
	}

	private bool EnabledGyro(){
		if (SystemInfo.supportsGyroscope) {
			gyro = Input.gyro;
			gyro.enabled = true;

			cameraContainer.transform.rotation = Quaternion.Euler (90f, 90f, 90f);
			rot = new Quaternion (0, 0, 1, 0);

			return true;
		}
		return false;
	}
	
	// Update is called once per frame
	private void Update () {
        if (gyroEnabled && gyroActive) {
			Quaternion gyroAttitude = gyro.attitude;
			//gyroAttitude.y = gyroTest.y - gyro.attitude.y;
			transform.localRotation = gyroAttitude * rot;
		}else{
	        transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation , 5 * 1 * Time.deltaTime); 
		}
		float azm = transform.rotation.eulerAngles.y;
		float alt = transform.rotation.eulerAngles.x;
		rotAzm = azm;
		// print(azm+","+(alt));
	}
}
