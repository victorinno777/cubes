using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class GlobalLogic : MonoBehaviour
{
	public GameObject cube;
	GameObject text, pusherPanel;
	Toggle pusherToggle;
	int globalScore;
	public int cubeCount;
	float cashTime = 0;
	Vector3 startPoint = new Vector3(0, 0.5f, -4.5f);
	public bool pusher;
	AdsPage adsPage;
	
	void Awake()
	{
		MobileAds.Initialize(initStatus => { });
	}
	
    void Start()
    {		
		adsPage = (AdsPage) ScriptableObject.CreateInstance("AdsPage");
	
		pusherPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
		text = GameObject.Find("Canvas").transform.GetChild(0).transform.GetChild(0).gameObject;
		pusherToggle = GameObject.Find("Canvas").transform.GetChild(2).gameObject.GetComponent<Toggle>();
		
        CreateCube(0, startPoint);
		
		pusherToggle.onValueChanged.AddListener(delegate {CheckPusher();});
    }
	
	void CheckPusher()
	{
		if (pusherToggle.isOn)
		{
			pusherPanel.SetActive(true);
			pusher = true;
		}
		else
		{
			pusherPanel.SetActive(false);
			pusher = false;
		}
	}
	
	public void Recharge()
	{
		cubeCount ++;
		if (cubeCount == 10)
		{
			cubeCount = 0;
			adsPage.ShowAd();
			adsPage.OnEnable();
		}
		
		StartCoroutine(RechargeTimer());
	}
	
	IEnumerator RechargeTimer()
    {
        yield return new WaitForSeconds(0.5f);
        int newNumber = Random.Range(0, 6);
		CreateCube((int) newNumber, startPoint);
    }
	
	GameObject CreateCube(int number, Vector3 startPosition)
	{
		GameObject cubeObj = Instantiate(cube, startPosition, Quaternion.identity);
		cubeObj.GetComponent<Cube>().number = number;
		return cubeObj;
	}
	
	public void Collapse(int number, ContactPoint[] contacts)
	{
		if (cashTime == Time.time)
		{
			GameObject cubeObj = CreateCube(number + 1, contacts[0].point);
			RefreshCount(number + 1);
			cubeObj.GetComponent<Cube>().active = false;
			cubeObj.GetComponent<Cube>().PushUp();
		}
		else
		{
			cashTime = Time.time;
		}
	}
	
	void RefreshCount(int toAdd)
	{
		globalScore += (int) Mathf.Pow(2, toAdd);
		text.GetComponent<Text>().text = globalScore.ToString();
	}
}

[System.Serializable]
public class Palette : Object 
{
	public static Color[] Set1 =
	{
		Color.red,
		new Color(0.08f, 0.69f, 0.13f, 1), // green
		new Color(0.73f, 0.9f, 0.13f, 1), // yellow
		new Color(0.2f, 0.23f, 0.92f, 1), // dark blue
		new Color(1, 0.34f, 0.76f, 1), // pink
		new Color(0.97f, 0.64f, 0.16f, 1), // orange
		new Color(0.57f, 0.31f, 0.17f, 1), // brown
		Color.black,
		new Color(0.4f, 0.87f, 0.67f, 1), // light green
		new Color(0.61f, 0.07f, 0.13f, 1), // dark red
		Color.blue,
		new Color(0.8f, 0.04f, 0.69f, 1) // violet
	};
	
	public static Color[] Set2 =
	{
		// another color palette
	};
}
