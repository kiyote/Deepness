using UnityEngine;
using System;
using System.Collections;
using View.Map;

public class CameraBehaviour : MonoBehaviour {
	
	private const float Duration = 0.15f;  // The amount of time it takes to animate to the new zoom level
	private const float Delta = 1.0f; // The most the scale can change in a frame
	
	private float _oneToOne;
	private float _maxZoomIn;
	private float _maxZoomOut;
	private float _targetSize;

	public const float PixelsToUnits = (float)MapView.TileSize;

	void Start () {
		_oneToOne = Screen.height / PixelsToUnits / 2.0f;
		_maxZoomIn = _oneToOne * 2.0f;
		_maxZoomOut = _oneToOne / 2.0f;
		_targetSize = _oneToOne;

		GetComponent<Camera>().orthographicSize = _oneToOne;
		GetComponent<Camera>().UpdateOrthographicBounds();		
	}
	
	void Update() {
		bool dirty = false;
		
		// Check to see if the user has scrolled the wheel
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
		{
			_targetSize = Mathf.Clamp(_targetSize + Delta, _maxZoomOut, _maxZoomIn );
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
		{
			_targetSize = Mathf.Clamp(_targetSize - Delta, _maxZoomOut, _maxZoomIn );
		}
		
		// If the camera hasn't reached the target value yet we need to interpolate the current camera size and the target size
		if (Mathf.Abs(_targetSize - GetComponent<Camera>().orthographicSize) > 0.05f) {
			float newSize = Mathf.Lerp( Camera.main.orthographicSize, _targetSize, Time.deltaTime / Duration );
			GetComponent<Camera>().orthographicSize = newSize;
			dirty = true;
		} else if (_targetSize != GetComponent<Camera>().orthographicSize) {
			GetComponent<Camera>().orthographicSize = _targetSize;
			dirty = true;
		}
		
		float MouseX = Input.GetAxis("Mouse X");
		float MouseY = Input.GetAxis("Mouse Y");
		Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		position.z = 0.0f;

		if (Input.GetMouseButton(1)) {
			if ((MouseX != 0.0f) || (MouseY != 0.0f))
			{
				Vector3 newPos = new Vector3(- MouseX, - MouseY, 0) + GetComponent<Camera>().transform.position;
				
				Vector3 roundPos = new Vector3(RoundToNearestPixel(newPos.x, GetComponent<Camera>()), RoundToNearestPixel(newPos.y, GetComponent<Camera>()), -10.0f);
				if ((roundPos.x != transform.position.x) || (roundPos.y != transform.position.y))
				{
					transform.position = roundPos; 				
					dirty = true;
				}
			}
		}

		if (dirty)
		{
			GetComponent<Camera>().UpdateOrthographicBounds();
            MessageBus.Get.Publish<CameraUpdateEvent>(this, new CameraUpdateEvent(GetComponent<Camera>().OrthographicBounds()));
		}
	}
	
	private static float RoundToNearestPixel(float unityUnits, Camera viewingCamera)
	{
		float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
		valueInPixels = Mathf.Round(valueInPixels);
		float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));

		return adjustedUnityUnits;
	}
}
