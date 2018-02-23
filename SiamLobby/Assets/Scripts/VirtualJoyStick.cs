using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {
	private Image bgImg, joystickImg;
	public Vector3 inputVector;

	public virtual void OnDrag(PointerEventData eventData)
	{
		Vector2 pos;
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
		{
			pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
			pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);
			inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
			inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

			joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVector.z * (bgImg.rectTransform.sizeDelta.y / 3));
			Debug.Log(inputVector);
		}

	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		OnDrag(eventData);

	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		inputVector = Vector3.zero;
		joystickImg.rectTransform.anchoredPosition = Vector3.zero;
	}

	// Use this for initialization
	void Start () {
		bgImg = GetComponent<Image>();
		joystickImg = transform.GetChild(0).GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () {

	}

	public float Horizontal()
	{
		if (inputVector.x != 0)
			return inputVector.x;
		else
			return Input.GetAxis("Horizontal");
	}

	public float Vertical()
	{
		if (inputVector.z != 0)
			return inputVector.z;
		else
			return Input.GetAxis ("Vertical");
	}

}