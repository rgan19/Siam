using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	private static int timeSinceStart = 0;
	public static int totalTime = 60;
	public Text countdownText;
	private static int hour;
	private static int min;
	public bool isTimeUp = false;
	private static string initialTime = "08:00";
	// Use this for initialization
	void Start()
	{
		StartCoroutine("LoseTime");

	}

	// Update is called once per frame
	void Update()
	{
		countdownText.text = display();

		if (totalTime - timeSinceStart <= 0)
		{
			StopCoroutine("LoseTime");
			countdownText.text = "Times Up!";
			isTimeUp = true;
		}
	}

	IEnumerator LoseTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			timeSinceStart++;
		}
	}

	static string display() {
		
		hour = timeSinceStart / 6 + 8;
		min = timeSinceStart % 6;
		if (hour.ToString ().Length == 2)
			return hour + ":" + min+"0";
		else
			return "0" + hour + ":" + min+ "0";

	}
	public void resetTimer() {
		timeSinceStart = 0;
		countdownText.text = initialTime;
		isTimeUp = false;
	}

}