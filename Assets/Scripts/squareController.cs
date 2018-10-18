using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class squareController : MonoBehaviour
{

	public bool top;
	public int column;
	public int row;
	public Color currentColour;
	public int wonBy = 0;
	public bool valid = true;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void squareHoverOn(GameObject square)
	{
		if (gameManager.instance.turn == 1)
		{
			GetComponent<Image>().color = gameManager.instance.oneHighlightSquareColour;
		}
		else
		{
			GetComponent<Image>().color = gameManager.instance.twoHighlightSquareColour;
		}
	}

	public void squareHoverOff(GameObject square)
	{
		GetComponent<Image>().color = currentColour;
	}

	public void setEnabledState(bool state)
	{
		if (state)
		{
			EventTrigger eventTrigger = GetComponent<EventTrigger>();

			EventTrigger.Entry clickEntry = new EventTrigger.Entry();
			clickEntry.eventID = EventTriggerType.PointerClick;
			clickEntry.callback.AddListener((data) => { gameManager.instance.squareClicked(gameObject, (PointerEventData)data); });
			eventTrigger.triggers.Add(clickEntry);

			EventTrigger.Entry hoverOnEntry = new EventTrigger.Entry();
			hoverOnEntry.eventID = EventTriggerType.PointerEnter;
			hoverOnEntry.callback.AddListener((data) => { gameManager.instance.squareHoverOn(gameObject, (PointerEventData)data); });
			eventTrigger.triggers.Add(hoverOnEntry);

			EventTrigger.Entry hoverOffEntry = new EventTrigger.Entry();
			hoverOffEntry.eventID = EventTriggerType.PointerExit;
			hoverOffEntry.callback.AddListener((data) => { gameManager.instance.squareHoverOff(gameObject, (PointerEventData)data); });
			eventTrigger.triggers.Add(hoverOffEntry);

			currentColour = gameManager.instance.defaultSquareColour;
			GetComponent<Image>().color = currentColour;
		}
		else
		{
			Destroy(GetComponent<EventTrigger>());
			currentColour = Color.clear;
			GetComponent<Image>().color = currentColour;
		}
	}

	public void setLargeSquareValidity(bool state)
	{
		valid = state;
		if (state)
		{
			if (wonBy == 0)
			{
				if (gameManager.instance.turn == 1)
				{
					GetComponent<Image>().color = gameManager.instance.oneSquareColour;

				}
				else
				{
					GetComponent<Image>().color = gameManager.instance.twoSquareColour;
				}
			}
		}
		else
		{
			GetComponent<Image>().color = currentColour;
		}
	}

	public void setWinState(int winner)
	{
		wonBy = winner;
		if (winner == 1)
		{
			currentColour = gameManager.instance.oneSquareColour;
		}
		else if (winner == 2)
		{
			currentColour = gameManager.instance.twoSquareColour;
		}
		GetComponent<Image>().color = currentColour;
	}
}
