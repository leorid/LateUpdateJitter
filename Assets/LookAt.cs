using UnityEngine;

public class LookAt : MonoBehaviour
{
	Camera _cam;
	[SerializeField] Transform _transform;

	// Start is called before the first frame update
	void Start()
	{
		_cam = Camera.main;
		if (!_transform) _transform = transform;
	}

	// Update is called once per frame
	void LateUpdate()
	{
		Ray ray = _cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
		RaycastHit hit;
		Vector3 lookAtPoint = ray.GetPoint(100);
		if (Physics.Raycast(ray, out hit, 100))
		{
			lookAtPoint = hit.point;
		}

		_transform.LookAt(lookAtPoint, Vector3.up);
	}
}
