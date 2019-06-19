using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
	[SerializeField] bool _useInitialOffset = true;
	[SerializeField] bool _cursorVisible = true;
	[SerializeField] Vector3 _originOffset;
	float _originalCamOffsetDistance;
	public float camOffsetDistance;
	[SerializeField] Vector2 _rotationSpeed = Vector2.one;
	[SerializeField] Vector2 _rotationLimitVertical = new Vector2(-70, 85);
	[SerializeField] Transform _target;
	Transform _origin;


	// Start is called before the first frame update
	void Start()
	{
		_origin = transform.parent;

		if (!_target)
		{
			_target = _origin;
		}


		if (_useInitialOffset)
		{
			_originOffset = _target.position - _origin.position;
		}
		if (camOffsetDistance == 0)
		{
			camOffsetDistance = (transform.position - _origin.position).magnitude;
		}
		_originalCamOffsetDistance = camOffsetDistance;

		SetCursorVisibility();
	}


	// Update is called once per frame
	public void LateUpdate()
	{
		ChangeCursorVisibility();

		// keep position
		_origin.position = _target.position + _originOffset;

		// do not rotate when the cursor is visible
		if (!_cursorVisible)
		{
			OrbitCameraRotation();
		}
		ManageDistance();
	}

	void ChangeCursorVisibility()
	{
		if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetMouseButtonDown(2))
		{
			_cursorVisible = !_cursorVisible;
			SetCursorVisibility();
		}
	}

	private float rotationVertical = 0f;

	void OrbitCameraRotation()
	{
		float hAxis = Input.GetAxis("Mouse X");
		float vAxis = -Input.GetAxis("Mouse Y");

		_origin.Rotate(Vector3.up, hAxis * _rotationSpeed.x, Space.World);

		rotationVertical += vAxis * _rotationSpeed.y;
		rotationVertical = ClampAngle(rotationVertical, _rotationLimitVertical.x, _rotationLimitVertical.y);

		_origin.localEulerAngles = new Vector3(rotationVertical, _origin.localEulerAngles.y, _origin.localEulerAngles.z);
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}

	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

		angleX = Mathf.Clamp(angleX, _rotationLimitVertical.x, _rotationLimitVertical.y);

		q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}

	void ManageDistance()
	{
		Vector3 originToCamVector = transform.position - _origin.position;
		if (Vector3.Dot(originToCamVector, transform.forward) > 0)
		{
			originToCamVector *= -1;
		}
		transform.position = _origin.position + originToCamVector.normalized * camOffsetDistance;
	}
	
	void SetCursorVisibility()
	{
		Cursor.visible = _cursorVisible;
		Cursor.lockState = _cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
	}
}