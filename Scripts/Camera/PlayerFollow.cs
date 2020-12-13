using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
	public float smoothSpeed = 0.125f;
	public Vector3 offset;

	public Transform target;

	private void FixedUpdate()
	{
		Vector3 desiredPosition = new Vector3(target.position.x, 0f, 0f) + offset;
		//Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition,ref vel, smoothSpeed,MaxSpeed);

		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;
		transform.LookAt(target);
	}

}