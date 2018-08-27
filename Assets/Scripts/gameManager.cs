using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class gameManager : MonoBehaviour
{
	[HideInInspector]
	public static gameManager instance;

	void Awake()
	{
		instance = this;
	}

	public int boardSize = 3;
	public Color defaultSquareColour;
	public Color oneSquareColour;
	public Color twoSquareColour;

	public Color oneHighlightSquareColour;
	public Color twoHighlightSquareColour;

	public GameObject canvas;

	public int turn = 1;
	GameObject board;
	Transform lastHighlight;

	// Use this for initialization
	void Start()
	{
		board = canvas.GetComponent<boardBuilder>().buildBoard(boardSize);
		setLargeAllValidity(true);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void squareClicked(GameObject square, PointerEventData data)
	{
		// Check validity and perform 'taking' action
		squareController controller = square.GetComponent<squareController>();
		if (square.GetComponent<squareController>().valid)
		{
			if (lastHighlight && board.transform.GetChild(controller.row).GetChild(controller.column).GetComponent<squareController>().wonBy == 0)
			{
				lastHighlight.GetComponent<squareController>().currentColour = Color.clear;
				lastHighlight.GetComponent<Image>().color = Color.clear;
			}
			if (turn == 1)
			{
				turn++;
				controller.setWinState(1);
			}
			else
			{
				turn--;
				controller.setWinState(2);
			}
			// Move highlight to new large square
			foreach (Transform row in board.transform)
			{
				foreach (Transform largeSquare in row)
				{
					if (square.transform.IsChildOf(largeSquare))
					{
						lastHighlight = largeSquare;
					}
				}
			}
			// Check winning conditions
			if (lastHighlight)
			{
				//print("checking for win at (" + lastHighlight.GetComponent<squareController>().row + ", " + lastHighlight.GetComponent<squareController>().column + ")";

				// Check win within a large square
				List<List<int>> squareStateList = new List<List<int>>();
				for (int row = 0; row < lastHighlight.GetChild(0).childCount; row++)
				{
					List<int> sublist = new List<int>();
					for (int child = 0; child < lastHighlight.GetChild(0).GetChild(row).childCount; child++)
					{
						sublist.Add(lastHighlight.GetChild(0).GetChild(row).GetChild(child).GetComponent<squareController>().wonBy);
					}
					squareStateList.Add(sublist);
				}
				int smallSquareWin = checkWin(squareStateList);
				if (smallSquareWin == 1)
				{
					lastHighlight.GetComponent<squareController>().setWinState(1);
					destroyChildren(lastHighlight.gameObject);
				}
				else if (smallSquareWin == 2)
				{
					lastHighlight.GetComponent<squareController>().setWinState(2);
					destroyChildren(lastHighlight.gameObject);
				}
				// Check win for whole board
				List<List<int>> largeSquareStateList = new List<List<int>>();
				for (int row = 0; row < board.transform.childCount; row++)
				{
					List<int> sublist = new List<int>();
					for (int child = 0; child < board.transform.GetChild(row).childCount; child++)
					{
						sublist.Add(board.transform.GetChild(row).GetChild(child).GetComponent<squareController>().wonBy);
					}
					largeSquareStateList.Add(sublist);
				}
				int largeSquareWin = checkWin(largeSquareStateList);
				// Handle win - TEMP
				if (largeSquareWin == 1)
				{
					print("Player one wins!");
				}
				else if (largeSquareWin == 2)
				{
					print("Player two wins!");
				}
			}
			// Do highlight updating
			// If new square is different
			if (lastHighlight != board.transform.GetChild(square.GetComponent<squareController>().row).GetChild(square.GetComponent<squareController>().column))
			{
				// Set current large square to invalid and get new square, setting it to valid
				lastHighlight.GetComponent<squareController>().setLargeSquareValidity(false);
				lastHighlight = board.transform.GetChild(square.GetComponent<squareController>().row).GetChild(square.GetComponent<squareController>().column);
				lastHighlight.GetComponent<squareController>().setLargeSquareValidity(true);
			} // Otherwise can stay as-is
			  // Update valid squares
			  // If the square isn't won
			if (lastHighlight.GetComponent<squareController>().wonBy == 0)
			{
				// Set only new square's subsquares as valid - if they aren't won yet
				setSmallAllValidity(false);
				setLargeAllValidity(false);
				lastHighlight.GetComponent<squareController>().setLargeSquareValidity(true);
				foreach (Transform row in lastHighlight.GetChild(0))
				{
					foreach (Transform child in row)
					{
						if (child.GetComponent<squareController>().wonBy == 0)
						{
							child.GetComponent<squareController>().valid = true;
						}
					}
				}
			}
			else
			{
				// Otherwise everything is valid
				setSmallAllValidity(true);
				setLargeAllValidity(true);
				// Disable last taken square
				//lastHighlight.GetComponent<squareController>().setLargeSquareValidity(false);
				lastHighlight.GetComponent<Image>().color = lastHighlight.GetComponent<squareController>().currentColour;
			}
		}
	}

	public void squareHoverOn(GameObject square, PointerEventData data)
	{
		squareController controller = square.GetComponent<squareController>();
		Transform largeSquare = board.transform.GetChild(controller.row).GetChild(controller.column);
		if (controller.valid)
		{
			if (controller.wonBy == 0)
			{
				controller.squareHoverOn(square);
				if (largeSquare.GetComponent<squareController>().wonBy == 0)
				{
					if (turn == 1)
					{
						largeSquare.GetComponent<Image>().color = twoSquareColour;
					}
					else
					{
						largeSquare.GetComponent<Image>().color = oneSquareColour;
					}
				}
				else
				{
					if (turn == 1)
					{
						largeSquare.GetComponent<Image>().color = twoHighlightSquareColour;
					}
					else
					{
						largeSquare.GetComponent<Image>().color = oneHighlightSquareColour;
					}
				}
			}
		}
	}

	public void squareHoverOff(GameObject square, PointerEventData data)
	{
		square.GetComponent<squareController>().squareHoverOff(square);
		Transform largeSquare = board.transform.GetChild(square.GetComponent<squareController>().row).GetChild(square.GetComponent<squareController>().column);
		if (largeSquare.GetComponent<squareController>().wonBy != 0)
		{
			if (largeSquare.GetComponent<squareController>().wonBy == 1)
			{
				largeSquare.GetComponent<Image>().color = oneSquareColour;
				largeSquare.GetComponent<squareController>().currentColour = oneSquareColour;
			}
			else
			{
				largeSquare.GetComponent<Image>().color = twoSquareColour;
				largeSquare.GetComponent<squareController>().currentColour = twoSquareColour;
			}

		}
		else if (largeSquare.GetComponent<squareController>().valid)
		{
			if (turn == 1)
			{
				largeSquare.GetComponent<Image>().color = oneHighlightSquareColour;
			}
			else
			{
				largeSquare.GetComponent<Image>().color = twoHighlightSquareColour;
			}
		}
		else
		{
			largeSquare.GetComponent<Image>().color = Color.clear;
		}
	}

	public void destroyChildren(GameObject parent)
	{
		foreach (Transform child in parent.transform)
		{
			Destroy(child.gameObject);
		}
	}

	public void setSmallAllValidity(bool state)
	{
		foreach (Transform r1 in board.transform)
		{
			foreach (Transform s1 in r1)
			{
				if (s1.childCount > 0)
				{
					foreach (Transform r2 in s1.GetChild(0))
					{
						foreach (Transform s2 in r2)
						{
							if (s2.GetComponent<squareController>().wonBy == 0)
							{
								s2.GetComponent<squareController>().valid = state;
							}
						}
					}
				}
			}
		}
	}
	public void setLargeAllValidity(bool state)
	{
		foreach (Transform row in board.transform)
		{
			foreach (Transform largeSquare in row)
			{
				largeSquare.GetComponent<squareController>().setLargeSquareValidity(state);
			}
		}
	}

	int checkWin(List<List<int>> list)
	{
		foreach (List<int> row in list)
		{
			if (checkRow(row) == 1)
			{
				return 1;
			}
			else if (checkRow(row) == 2)
			{
				return 2;
			}
		}

		for (int y = 0; y < list.Count; y++)
		{
			List<int> columnList = new List<int>();
			for (int x = 0; x < list.Count; x++)
			{
				columnList.Add(list[x][y]);
			}
			if (checkRow(columnList) == 1)
			{
				return 1;
			}
			else if (checkRow(columnList) == 2)
			{
				return 2;
			}
		}

		for (int diagonal = 0; diagonal < list.Count; diagonal++)
		{
			List<int> diagonalList = new List<int>();
			for (int square = 0; square < list.Count; square++)
			{
				diagonalList.Add(list[square][square]);
			}
			if (checkRow(diagonalList) == 1)
			{
				return 1;
			}
			else if (checkRow(diagonalList) == 2)
			{
				return 2;
			}
		}

		for (int reverseDiagonal = 0; reverseDiagonal < list.Count; reverseDiagonal++)
		{
			List<int> reverseDiagonalList = new List<int>();
			int y = list.Count - 1;
			for (int x = 0; x < list.Count; x++)
			{
				reverseDiagonalList.Add(list[x][y]);
				y--;
			}
			if (checkRow(reverseDiagonalList) == 1)
			{
				return 1;
			}
			else if (checkRow(reverseDiagonalList) == 2)
			{
				return 2;
			}
		}

		return 0;
	}
	int checkRow(List<int> row)
	{
		bool playerOne = true;
		bool playerTwo = true;
		foreach (int square in row)
		{
			if (square == 0)
			{
				playerOne = false;
				playerTwo = false;
			}
			else if (square == 1)
			{
				playerTwo = false;
			}
			else if (square == 2)
			{
				playerOne = false;
			}
		}
		if (playerOne)
		{
			return 1;
		}
		else if (playerTwo)
		{
			return 2;
		}
		else
		{
			return 0;
		}
	}
}
