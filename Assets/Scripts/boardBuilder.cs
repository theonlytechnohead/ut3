using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boardBuilder : MonoBehaviour
{
	public GameObject boardPrefab;
	public GameObject rowPrefab;
	public GameObject squarePrefab;

	public GameObject board;

	// Use this for initialization
	void Start()
	{
		//createBoard(size, transform, true);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public GameObject buildBoard(int size)
	{
		createBoard(size, transform, true);
		return board;
	}

	public GameObject loadBoard(int size, List<Vector5Int> largeData, List<Vector6Int> smallData)
	{
		createBoard(size, transform, true);
		foreach (Vector5Int largeSquare in largeData)
		{
			bool valid = largeSquare.valid != 0;
			board.transform.GetChild(largeSquare.largeRow).GetChild(largeSquare.largeColumn).GetComponent<squareController>().setWinState(largeSquare.wonBy);
			board.transform.GetChild(largeSquare.largeRow).GetChild(largeSquare.largeColumn).GetComponent<squareController>().setLargeSquareValidity(valid);
		}
		foreach (Vector6Int smallSquare in smallData)
		{
			bool valid = smallSquare.valid != 0;
			board.transform.GetChild(smallSquare.largeRow).GetChild(smallSquare.largeColumn).GetChild(0).GetChild(smallSquare.smallRow).GetChild(smallSquare.smallColumn).GetComponent<squareController>().setWinState(smallSquare.wonBy);
			board.transform.GetChild(smallSquare.largeRow).GetChild(smallSquare.largeColumn).GetChild(0).GetChild(smallSquare.smallRow).GetChild(smallSquare.smallColumn).GetComponent<squareController>().valid = valid;
		}
		return board;
	}

	void createBoard(int size, Transform parent, bool top)
	{
		GameObject boardObject = Instantiate(boardPrefab, parent.position, Quaternion.identity, parent);
		if (!top)
		{
			boardObject.GetComponent<VerticalLayoutGroup>().spacing = 100f / (size * size);
			boardObject.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
			Destroy(boardObject.GetComponent<Image>());
		}
		else
		{
			board = boardObject;
		}
		for (int row = 0; row < size; row++)
		{
			GameObject rowObject = Instantiate(rowPrefab, boardObject.transform);
			if (!top)
			{
				rowObject.GetComponent<HorizontalLayoutGroup>().spacing = 100f / (size * size);
			}
			for (int column = 0; column < size; column++)
			{
				GameObject squareObject = Instantiate(squarePrefab, rowObject.transform);
				squareObject.GetComponent<squareController>().row = row;
				squareObject.GetComponent<squareController>().column = column;
				squareObject.GetComponent<squareController>().top = top;
				if (top)
				{
					squareObject.GetComponent<squareController>().setEnabledState(false);
					createBoard(size, squareObject.transform, false);
				}
				else
				{
					squareObject.GetComponent<squareController>().setEnabledState(true);
				}
			}
		}
	}
}
