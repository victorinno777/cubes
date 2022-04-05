using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cube : MonoBehaviour
{
	bool isPressed, pusher, yellowScale = true, redScale;
	GameObject pusherScaleYellow, pusherScaleRed;
	public int number;
	float force = 1000, angle = 0, bottom = 0;
	public bool active = true;
	Touch touch;
	Rigidbody rigidbody;
	
    public void Start()
    {
		GameObject pusherPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
		pusherScaleYellow = pusherPanel.transform.GetChild(0).gameObject;
		pusherScaleRed = pusherPanel.transform.GetChild(1).gameObject;
		rigidbody = GetComponent<Rigidbody>();
        SetCube();
    }
	
	void SetCube()
	{
		int powNumber = (int) Mathf.Pow(2, number + 1);
		for (int i = 0; i < transform.childCount; i ++)
		{
			transform.GetChild(i).gameObject.GetComponent<TextMeshPro>().text = powNumber.ToString();
			transform.GetChild(i).gameObject.GetComponent<TextMeshPro>().fontSize = 1.2f * (7 - powNumber.ToString().Length);
		}
		GetComponent<MeshRenderer>().materials[0].color = Palette.Set1[number];
	}
	
	void Update()
    {
		SetAndPush();
	}
	
	void SetAndPush()
	{
		if (active && Input.touchCount > 0)
		{
			touch = Input.GetTouch(0);
			pusher = Camera.main.GetComponent<GlobalLogic>().pusher;
			if (pusher)
			{
				if (yellowScale)
				{
					if (bottom < 45)
					{
						bottom += 0.8f;
						pusherScaleYellow.GetComponent<RectTransform>().sizeDelta = new Vector2(33, bottom * 4);
					}
					else
					{
						yellowScale = false;
						redScale = true;
						bottom = 0;
						pusherScaleYellow.GetComponent<RectTransform>().sizeDelta = new Vector2(33, 0);
					}
				}
				
				if (redScale)
				{
					if (angle < 45)
					{
						angle += 1f;
						pusherScaleRed.GetComponent<RectTransform>().sizeDelta = new Vector2(33, angle * 4);
					}
				}
				
			}
				
			if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
			{
				Ray raycast = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit hit;
				if (Physics.Raycast(raycast, out hit))
				{
					if (hit.collider.tag == "Cube")
					{
						Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.transform.position.z * -1));
						transform.position = new Vector3(touchPos.x, transform.position.y, transform.position.z);
						isPressed = true;
					}
				}
			}
			if (Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				RaycastHit hit;
				if (Physics.Raycast(raycast, out hit))
				{
					if (isPressed)
					{
						active = false;
						isPressed = false;
						rigidbody.constraints = RigidbodyConstraints.None;
						float correctForce = force - (angle * 8);
						rigidbody.AddForce(0, Mathf.Sin(Mathf.Deg2Rad * angle) * correctForce, Mathf.Cos(Mathf.Deg2Rad * angle) * correctForce);
						
						bottom = 0;
						angle = 0;
						pusherScaleYellow.GetComponent<RectTransform>().sizeDelta = new Vector2(33, 0);
						pusherScaleRed.GetComponent<RectTransform>().sizeDelta = new Vector2(33, 0);
						
						Camera.main.GetComponent<GlobalLogic>().Recharge();
					}
				}
			}
		}
	}
	
	public void PushUp()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.AddForce(0, 300, 0);
		
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "Cube")
		{
			return;
		}
		
		int collisionNumber = collision.gameObject.GetComponent<Cube>().number;
		if (collisionNumber == number)
		{
			Camera.main.GetComponent<GlobalLogic>().Collapse(number, collision.contacts);
			Destroy(gameObject);
		}
	}
}