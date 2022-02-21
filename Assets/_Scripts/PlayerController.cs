using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//[SerializeField] private float joystickHorizontalValue;
	//[SerializeField] private float joystickVerticalValue;
	[SerializeField] private float playerMaxSpeed = 5;
	[SerializeField] private float forceValue = 10;
	[SerializeField] private LayerMask enemyLayerMask;

	private DynamicJoystick joystick;
	private JoyButton joybutton;
	private bool jump;
	private float scaleAppendValue;
	private Rigidbody rigidbody;
	private Vector3 targetSpeed;
	private Vector3 targetScale;

	private void Start()
	{
		joystick = FindObjectOfType<DynamicJoystick>();
		joybutton = FindObjectOfType<JoyButton>();
		rigidbody = GetComponent<Rigidbody>();
		//Physics.gravity = new Vector3(0, -30, 0);
	}

	private void FixedUpdate()
	{
		//joystickHorizontalValue = joystick.Horizontal;
		//joystickVerticalValue = joystick.Vertical;

		//TEMEL HAREKET KODU
		targetSpeed = new Vector3(joystick.Horizontal * playerMaxSpeed, rigidbody.velocity.y, joystick.Vertical * playerMaxSpeed);
		rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, targetSpeed, Time.fixedDeltaTime * 2);//Yavaşça veriyoruz hızı. FixedUpdate olduğu için fixedDeltaTime kullanılır
		if (Mathf.Abs(joystick.Horizontal) > 0.05f || Mathf.Abs(joystick.Vertical) > 0.05f)
		{
			scaleAppendValue += Time.fixedDeltaTime / 20;
		}
		targetScale = Vector3.one + Vector3.one * scaleAppendValue;
		transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.fixedDeltaTime * 5);//Daha yumuşak küçülme ve büyüme 

		////HAVAYA ZIPLAMA KODU
		//if (!jump && joybutton.Pressed) //havada değilse ve butona basılmışsa
		//{
		//	jump = true;
		//	rigidbody.velocity += Vector3.up * 5f;
		//}
		//else if (jump && !joybutton.Pressed)
		//{
		//	jump = false;
		//}
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			Force();
		}
	}

	private void Force()
	{
		if (scaleAppendValue > 0.05f)
		{
			Collider[] enemyColliders = Physics.OverlapSphere(transform.position, 5, enemyLayerMask);//Görünmez bir sphere yani top oluşturur bizim belirlediğimiz çapta içinde kalan düşmanları bize verir bir collider dizisi halinde
			float minDistance = float.MaxValue;
			Transform foundEnemy = null;
			foreach (Collider enemy in enemyColliders)
			{
				if (Vector3.Distance(enemy.transform.position, transform.position) < minDistance)
				{
					minDistance = Vector3.Distance(enemy.transform.position, transform.position);
					foundEnemy = enemy.transform;
				}
			}
			if (foundEnemy != null)
			{
				Vector3 targetDirection = foundEnemy.transform.position - transform.position;
				foundEnemy.GetComponent<Rigidbody>().velocity = targetDirection.normalized * forceValue * scaleAppendValue;
				scaleAppendValue = 0;
			}
		}
	}
}
