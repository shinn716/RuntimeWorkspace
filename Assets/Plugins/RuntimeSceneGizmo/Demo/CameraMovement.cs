using UnityEngine;

namespace RuntimeSceneGizmo
{
	public class CameraMovement : MonoBehaviour
	{
		[SerializeField]
		private float sensitivity = 0.5f;

		private Vector3 prevMousePos;
		private Transform mainCamParent;
		private Camera mainCam;

		private float fov = 60;
		private float size = 50;

		private void Awake()
		{
			mainCam = Camera.main;
			mainCamParent = Camera.main.transform.parent;
		}

		private void Update()
		{
			//size = Mathf.Clamp(size, .2f, 100);
			//fov = Mathf.Clamp(fov, .05f, 100);

			if (Input.GetMouseButtonDown(1))
				prevMousePos = Input.mousePosition;
			else if (Input.GetMouseButton(1))
			{
				Vector3 mousePos = Input.mousePosition;
				Vector2 deltaPos = (mousePos - prevMousePos) * sensitivity;

				Vector3 rot = mainCamParent.localEulerAngles;
				while (rot.x > 180f)
					rot.x -= 360f;
				while (rot.x < -180f)
					rot.x += 360f;

				rot.x = Mathf.Clamp(rot.x - deltaPos.y, -89.8f, 89.8f);
				rot.y += deltaPos.x;
				rot.z = 0f;

				mainCamParent.localEulerAngles = rot;
				prevMousePos = mousePos;
			}
			else if (Input.GetMouseButton(2))
				Pan(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
			else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				if (mainCam.orthographic)
					mainCam.orthographicSize = OrthographicZoomIO(++size * .1f);
				else
					mainCam.fieldOfView = PerspectiveZoomIO(++fov);
				//mainCamParent.transform.GetChild(0).Translate(new Vector3(0, 0, .5f));
			}
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				if (mainCam.orthographic)
					mainCam.orthographicSize = OrthographicZoomIO(--size * .1f);
				else
					mainCam.fieldOfView = PerspectiveZoomIO(--fov);
				//mainCamParent.transform.GetChild(0).Translate(-new Vector3(0, 0, .5f));
			}
		}

		private void Pan(float right, float up)
		{
			transform.Translate(sensitivity * 2 * right * Vector3.left);
			transform.Translate(sensitivity * 2 * up * Vector3.up, Space.World);
		}

		private float OrthographicZoomIO(float value)
        {
			return value = value < .1f ? .1f : value;
        }

		private float PerspectiveZoomIO(float value)
		{
			value = value < .01f ? .01f : value;
			return value;
		}
	}
}