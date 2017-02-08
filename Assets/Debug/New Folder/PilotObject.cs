using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Crappy hacker tool for debugging. Set the target to the thing you want to pilot. WASD to fly, space/E to ascend, lshift/Q to descend, hold right click to tilt camera, T to toggle.
/// </summary>
public class PilotObject : MonoBehaviour {

	[SerializeField] private bool useMainCamByDefault = true;

	[SerializeField] private GameObject target;
	[SerializeField] private float speed = 10;

	[SerializeField] private bool active = true;

	[SerializeField] private Text displayText;

	void Awake () {
		if (useMainCamByDefault) {
			target = Camera.main.gameObject;
		}

		if (active) {
			displayText.text = "Piloting: ON";
		}
		else {
			displayText.text = "Piloting: OFF";
		}
	}

	void Update () {
		if (active) {
			FlightControl ();
			if (Input.GetKeyDown (KeyCode.T)) {
				active = false;
				displayText.text = "Piloting: OFF";
			}
		}
		else {
			if (Input.GetKeyDown (KeyCode.T)) {
				active = true;
				displayText.text = "Piloting: ON";
			}
		}
	}

	private void FlightControl () {

		Vector3 tmp = target.transform.position;

		tmp += Input.GetAxis ("Horizontal") * target.transform.right * speed * Time.deltaTime;
		tmp += Input.GetAxis ("Vertical") * target.transform.forward * speed * Time.deltaTime;
		tmp += Input.GetAxis ("Ascend") * Vector3.up * speed * Time.deltaTime;

		target.transform.position = tmp;

		if (Input.GetMouseButton (1)) {

			//tmp = transform.localRotation.eulerAngles;
			tmp = Vector3.zero;
			tmp.x += -Input.GetAxis ("Mouse Y") * speed * 20 * Time.deltaTime;
			tmp.y += Input.GetAxis ("Mouse X") * speed * 20 * Time.deltaTime;

			target.transform.Rotate (tmp);

			tmp = target.transform.rotation.eulerAngles;
			tmp.z = 0;
			Quaternion qt = Quaternion.Euler (tmp);

			target.transform.rotation = Quaternion.Slerp (target.transform.rotation, qt, Time.deltaTime * 2);
			//target.transform.rotation = qt;
		}
	}
}
