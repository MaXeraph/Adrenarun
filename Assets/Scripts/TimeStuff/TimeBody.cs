using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TimeBody : MonoBehaviour
{

	public static bool isRewinding = false;

	public static bool isRecording = true;

	public float recordTime = 20f;


	List<PointInTime> pointsInTime;

	Rigidbody rb;

	void Start()
	{
		pointsInTime = new List<PointInTime>();
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		//Temp to test rewind
		if (Input.GetKeyDown(KeyCode.Return))
			StartRewind();
	}

	void FixedUpdate()
	{
		if (isRewinding)
			Rewind();
		else if (isRecording)
			Record();
	}

	void Rewind()
	{
		if (pointsInTime.Count > 0)
		{
			PointInTime pointInTime = pointsInTime[0];
			transform.position = pointInTime.position;
			Camera.main.transform.rotation = pointInTime.rotation;
			pointsInTime.RemoveAt(0);
		}
		else
		{
			StopRewind();
		}

	}

	void Record()
	{
		if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
		{
			pointsInTime.RemoveAt(pointsInTime.Count - 1);
		}

		pointsInTime.Insert(0, new PointInTime(transform.position, Camera.main.transform.rotation));
	}

	public static void StartRewind()
	{
		Cursor.visible = false;
		DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 5f, 5).SetEase(Ease.InOutQuint);
		isRewinding = true;
	}

	public static void StopRewind()
	{
		Time.timeScale = 1f;
		isRewinding = false;
		isRecording = true;
		SpeedManager.updateSpeeds(1f);
		PlayerCentral.paused = false;
		Cursor.lockState = CursorLockMode.Locked;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}
}