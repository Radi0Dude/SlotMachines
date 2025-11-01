using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//Manages the grids and slot machine logic
public class GridHolder : MonoBehaviour
{

	
	[SerializeField] private List<Grid> grids = new List<Grid>();

	[SerializeField]
	int amountOfSlots;

	[SerializeField]
	int heightMultiplier;

	[SerializeField]
	GameObject textPrefab;
	[SerializeField]
	GameObject canvasObj;
	[SerializeField]
	boolHolder bools;
	[SerializeField]
	List<BoolGrid> boolGrids = new List<BoolGrid>();

	[SerializeField]
	PlayerData playerData;

	[SerializeField]
	float slotTime;

	public bool isRunning = true;

	private void Awake()
	{

		playerData = playerData ?? FindFirstObjectByType<PlayerData>();
		canvasObj = canvasObj ?? FindFirstObjectByType<Canvas>().gameObject;
	}
	private void Start()
	{
		FirtSlot();

	}
	/// <summary>
	/// Initializes the first slot grid
	/// </summary>
	void FirtSlot()
	{
		for (int i = 0; i < amountOfSlots; i++)
		{
			grids.Add(new Grid());
			for (int j = 0; j < amountOfSlots; j++)
			{
				grids[i].cells.Add(new Cell());
				//grids[i].cells[j].num = UnityEngine.Random.Range(1, 10);
				grids[i].cells[j].num = 1;
				grids[i].cells[j].height = amountOfSlots - i * heightMultiplier;
				GameObject text = Instantiate(textPrefab, new Vector3(j * heightMultiplier + 100, grids[i].cells[j].height + 900), Quaternion.identity);
				text.transform.SetParent(canvasObj.transform);
				grids[i].cells[j].cellText = text.GetComponent<TMP_Text>();
				grids[i].cells[j].cellText.text = grids[i].cells[j].num.ToString();
			}
		}
		StartCoroutine(SpinLenght());
		CheckForBigWin();
	}
	/// <summary>
	/// Handles the main slot button press
	/// </summary>
	public void MasterSlotButton()
	{
		if (isRunning)
		{
			return;
		}
		for (int i = 0; i < amountOfSlots; i++)
		{
			for (int j = 0; j < amountOfSlots; j++)
			{
				grids[i].cells[j].num = UnityEngine.Random.Range(1, 10);
				grids[i].cells[j].cellText.text = grids[i].cells[j].num.ToString();
			}
		}
		StartCoroutine(SpinLenght());
		CheckForBigWin();
	}
	/// <summary>
	/// Checks for the biggest win based on the bool grids
	/// </summary>

	private void CheckForBigWin()
	{
		List<int> winningNums = grids.SelectMany(g => g.cells).Select(c => c.num).ToList();
		BiggestWin biggestWin = new BiggestWin();
		Debug.Log("Winning Numbers: " + string.Join(", ", winningNums));
		foreach (var grid in boolGrids)
		{
			List<int> nums = new List<int>();
			int previousCount = -1;

			for (int i = 0; i < grid.cells.Length; i++)
			{

				if (grid.cells[i] == true)
				{
					nums.Add(i);
				}
			}
			for (int i = 0; i < nums.Count; i++)
			{
				if (i == 0)
				{
					previousCount = winningNums[nums[i]];
					Debug.Log("Starting with: " + previousCount);
					continue;
				}
				int current = winningNums[nums[i]];
				if (current != previousCount)
				{
					Debug.Log("You lost");

					ProceduralWin();
					return;
				}


				previousCount = current;
			}
			Debug.Log("You won!");
			if (grid.score > biggestWin.score)
			{
				biggestWin.score = grid.score;
				biggestWin.winName = grid.name;
			}


		}

		StartCoroutine(Win(biggestWin));
	}
	/// <summary>
	/// Handles the spinning of the slot machine
	/// </summary>
	IEnumerator SpinLenght()
	{
		float t = 0;
		isRunning = true;
		while (t < slotTime)
		{

			t += Time.deltaTime;
			for (int i = 0; i < amountOfSlots; i++)
			{
				for (int j = 0; j < amountOfSlots; j++)
				{
					grids[i].cells[j].cellText.text = UnityEngine.Random.Range(1, 10).ToString();
				}
			}
			yield return new WaitForEndOfFrame();
		}
		for (int i = 0; i < amountOfSlots; i++)
		{
			for (int j = 0; j < amountOfSlots; j++)
			{
				grids[i].cells[j].cellText.text = grids[i].cells[j].num.ToString();
			}
		}
		isRunning = false;
	}
	/// <summary>
	/// Handles the win process
	/// </summary>
	private IEnumerator Win(BiggestWin bigWin)
	{
		yield return new WaitForSeconds(0.1f);
		Debug.Log(isRunning + " waiting for the slot to end");
		while (isRunning)
		{
			yield return new WaitForEndOfFrame();
		}
		Debug.Log("You won a " + bigWin.winName + " for " + bigWin.score + " credits!");
		yield return new WaitForSeconds(0.5f);

		playerData.AddCredits(bigWin.score);
		//Check for the biggest win is more important not every win
	}
	/// <summary>
	/// Procedurally checks for wins if no bool grid matched
	/// </summary>
	private void ProceduralWin()
	{
		//Check for Jackpot any number


		//for (int i = 0; i < grids.Count; i++)
		//{
		//	for (int j = 0; j < grids[i].cells.Count; j++)
		//	{
		//		numToCheckAgainst = grids[i].cells[0].num;
		//		if (grids[i].cells[j].num != numToCheckAgainst)
		//		{
		//			Debug.Log("No Jackpot");
		//			break;
		//		}
		//	}
		//}
		//Collumn check
		int collumsAmountWon = 0;
		int numToCheckAgainst = -1;

		for (int i = 0; i < grids.Count; i++)
		{
			if (grids[i].cells.All(c => c.num == grids[i].cells[0].num))
			{
				collumsAmountWon++;
			}
			int amountInRow = 0;
			numToCheckAgainst = grids[0].cells[i].num;
			for (int j = 0; j < grids[i].cells.Count; j++)
			{

				if (grids[j].cells[i].num == numToCheckAgainst)
				{
					amountInRow++;
				}

				if (j == grids.Count - 1 && amountInRow == grids.Count)
				{
					collumsAmountWon++;
				}
			}
		}
		if (collumsAmountWon >= grids.Count)
		{
			Debug.Log("You won a Jackpot for any number!");
		}
		else if (collumsAmountWon > 0)
		{
			Debug.Log("You won a smaller prize for " + collumsAmountWon + " collumns!");
		}
		else
		{
			Debug.Log("No win");
		}
	}
}

[Serializable]
class Grid
{
	public List<Cell> cells = new List<Cell>();
}
struct BiggestWin
{
	public string winName;
	public int score;
}
[Serializable]
class Cell
{
	public int num;
	public TMP_Text cellText;

	public int height;
}
[Serializable]
public class boolHolder
{
	public bool[] bools;
}
