using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;

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
	public GameObject StartPanel;
	public GameObject WinPanel;

	[HideInInspector]
	public int turn = 1;
	GameObject board;
	Transform lastHighlight;

	[HideInInspector]
	[SerializeField]
	List<Vector5Int> largeData = new List<Vector5Int>();
	[SerializeField]
	List<Vector6Int> smallData = new List<Vector6Int>();

	// Use this for initialization
	void Start()
	{
		setup();
	}

	void setup()
	{
		foreach (Transform child in canvas.transform)
		{
			Destroy(child.gameObject);
		}
		largeData.Clear();
		smallData.Clear();
		turn = 1;
		GameObject startPanel = Instantiate(StartPanel, canvas.transform, false);

		Button two_button = startPanel.transform.GetChild(1).GetComponent<Button>();
		UnityAction two_action = new UnityAction(delegate { buildBoard(2); });
		two_button.onClick.AddListener(two_action);

		Button three_button = startPanel.transform.GetChild(2).GetComponent<Button>();
		UnityAction three_action = new UnityAction(delegate { buildBoard(3); });
		three_button.onClick.AddListener(three_action);

		Button four_button = startPanel.transform.GetChild(3).GetComponent<Button>();
		UnityAction four_action = new UnityAction(delegate { buildBoard(4); });
		four_button.onClick.AddListener(four_action);
	}

	// Update is called once per frame
	void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.R))
		{
			setup();
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			save();
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			loadBoard();
		}
#endif
#if UNITY_STANDALONE
		if (Input.GetKeyDown(KeyCode.Q) & Input.GetKey(KeyCode.LeftControl))
		{
			Application.Quit();
		}
		if (Input.GetKeyDown(KeyCode.R) & Input.GetKey(KeyCode.LeftControl))
		{
			setup();
		}
		if (Input.GetKeyDown(KeyCode.S) & Input.GetKey(KeyCode.LeftControl))
		{
			save();
		}
		if (Input.GetKeyDown(KeyCode.L) & Input.GetKey(KeyCode.LeftControl))
		{
			loadBoard();
		}
#endif
	}

	public void buildBoard(int size)
	{
		boardSize = size;
		Destroy(canvas.transform.GetChild(0).gameObject);
		board = canvas.GetComponent<boardBuilder>().buildBoard(size);
		setLargeAllValidity(true);
	}

	public void loadBoard()
	{
		boardSize = PlayerPrefs.GetInt("size");
		Destroy(canvas.transform.GetChild(0).gameObject);
		board = canvas.GetComponent<boardBuilder>().loadBoard(boardSize, loadLarge(), loadSmall());
		// Check for won squares
		foreach (Transform r1 in board.transform)
		{
			foreach (Transform s1 in r1)
			{
				if (s1.GetComponent<squareController>().wonBy != 0)
				{
					s1.GetComponent<squareController>().setWinState(s1.GetComponent<squareController>().wonBy);
					destroyChildren(s1.gameObject);
				}
			}
		}
	}

	public void squareClicked(GameObject square, PointerEventData data)
	{
		// Check validity and perform 'taking' action
		squareController controller = square.GetComponent<squareController>();
		if (square.GetComponent<squareController>().valid)
		{
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
				if (smallSquareWin != 0)
				{
					lastHighlight.GetComponent<squareController>().setWinState(smallSquareWin);
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
				if (largeSquareWin != 0)
				{
					win(largeSquareWin);
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

	public void squareHoverOff(GameObject square, PointerEventData data)
	{
		square.GetComponent<squareController>().squareHoverOff(square);
		Transform largeSquare = board.transform.GetChild(square.GetComponent<squareController>().row).GetChild(square.GetComponent<squareController>().column);
		if (largeSquare.GetComponent<squareController>().wonBy != 0)
		{
			if (largeSquare.GetComponent<squareController>().wonBy == 1)
			{
				largeSquare.GetComponent<Image>().color = oneHighlightSquareColour;
				//largeSquare.GetComponent<squareController>().currentColour = oneSquareColour;
			}
			else
			{
				largeSquare.GetComponent<Image>().color = twoHighlightSquareColour;
				//largeSquare.GetComponent<squareController>().currentColour = twoSquareColour;
			}

		}
		else if (largeSquare.GetComponent<squareController>().valid)
		{
			if (turn == 1)
			{
				largeSquare.GetComponent<Image>().color = oneSquareColour;
			}
			else
			{
				largeSquare.GetComponent<Image>().color = oneSquareColour;
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

	void win(int player)
	{
		GameObject winPanel = Instantiate(WinPanel, canvas.transform, false);
		winPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + player;
		if (player == 1)
		{
			winPanel.GetComponent<Image>().color = oneHighlightSquareColour;
		}
		else
		{
			winPanel.GetComponent<Image>().color = twoHighlightSquareColour;
		}
		Button button = winPanel.transform.GetChild(2).GetComponent<Button>();
		UnityAction continueAction = new UnityAction(setup);
		button.onClick.AddListener(continueAction);
	}

	public void save()
	{
		PlayerPrefs.SetInt("size", boardSize);
		PlayerPrefs.Save();
		largeData.Clear();
		smallData.Clear();
		foreach (Transform r1 in board.transform)
		{
			foreach (Transform s1 in r1)
			{
				largeData.Add(new Vector5Int(
					0,
					s1.GetComponent<squareController>().row,
					s1.GetComponent<squareController>().column,
					s1.GetComponent<squareController>().wonBy,
					s1.GetComponent<squareController>().valid ? 1 : 0
					));
			}
		}
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
							smallData.Add(new Vector6Int(
								s1.GetComponent<squareController>().row,
								s1.GetComponent<squareController>().column,
								s2.GetComponent<squareController>().row,
								s2.GetComponent<squareController>().column,
								s2.GetComponent<squareController>().wonBy,
								s2.GetComponent<squareController>().valid ? 1 : 0
							));
						}
					}
				}
			}
		}
		string destination = Application.persistentDataPath + "/large.dat";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, largeData);
		file.Close();

		destination = Application.persistentDataPath + "/small.dat";

		if (File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		bf.Serialize(file, smallData);
		file.Close();
	}

	public List<Vector5Int> loadLarge()
	{
		string destination = Application.persistentDataPath + "/large.dat";
		FileStream file;
		List<Vector5Int> largeSquares = new List<Vector5Int>();

		if (File.Exists(destination))
		{
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Open(destination, FileMode.Open);
			largeSquares = (List<Vector5Int>)bf.Deserialize(file);
			file.Close();
		}
		return largeSquares;
	}

	public List<Vector6Int> loadSmall()
	{
		string destination = Application.persistentDataPath + "/small.dat";
		FileStream file;
		List<Vector6Int> smallSquares = new List<Vector6Int>();

		if (File.Exists(destination))
		{
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Open(destination, FileMode.Open);
			smallSquares = (List<Vector6Int>)bf.Deserialize(file);
			file.Close();
		}
		return smallSquares;
	}
}
