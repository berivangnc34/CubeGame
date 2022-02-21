using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[HideInInspector] public bool stopCamera;

	[SerializeField] private Transform m_Target;
	[SerializeField] private float mHeight;
	[SerializeField] private float mDistance;
	[SerializeField] private float mAngle;
	[SerializeField] private float mSmooth;
	[SerializeField] private float appendDistance;
	[SerializeField] private float rotationSpeed = 5;
	[SerializeField] private bool valuesChangePermission = true;
	[SerializeField] private bool freezingRotation;

	private Vector3 refVelocity;
	private Vector3 worldPosition;
	private Vector3 rotatedVector;
	private Vector3 flatTargetPosition;
	private Vector3 finalPosition;
	private float targetHeight;
	private float targetDistance;
	private float targetAngle;
	private float targetSmooth;

	private void Awake()
	{
		ChangeCameraValue(mHeight, mDistance, mAngle, mSmooth);
		CameraControl();
	}

	private void FixedUpdate()//if (Rigidbody Follow) FixedUpdate else if (translate Follow) lateUpdate
	{
		if (stopCamera)
		{
			return;
		}
		if (valuesChangePermission)
		{
			mHeight = Mathf.Lerp(mHeight, targetHeight, Time.fixedDeltaTime * 2);
			mDistance = Mathf.Lerp(mDistance, targetDistance, Time.fixedDeltaTime * 2);
			mAngle = Mathf.LerpAngle(mAngle, targetAngle, Time.fixedDeltaTime * 2);
			mSmooth = Mathf.LerpAngle(mSmooth, targetSmooth, Time.fixedDeltaTime * 2);
		}
		CameraControl();
	}

	public void ChangeCameraValue(float newHeight, float newDistance, float newAngle, float newSmooth)
	{
		targetHeight = newHeight;
		targetDistance = newDistance;
		targetAngle = newAngle;
		targetSmooth = newSmooth;
	}

	private void CameraControl()
	{
		if (!m_Target)
		{
			return;
		}
		worldPosition = (Vector3.forward * -mDistance) + (Vector3.up * mHeight);
		rotatedVector = Quaternion.AngleAxis(mAngle, Vector3.up) * worldPosition;
		flatTargetPosition = m_Target.position + new Vector3(transform.forward.x, 0, transform.forward.z).normalized * appendDistance;
		finalPosition = flatTargetPosition + rotatedVector;
		transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, mSmooth);
		if (!freezingRotation)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(flatTargetPosition - transform.position), Time.fixedDeltaTime * rotationSpeed);
		}
	}
}
