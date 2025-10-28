using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GridHolder : MonoBehaviour
{
	[SerializeField] private List<Grid> grids = new List<Grid>();

	[SerializeField]
	int amountOfSlots;

	[SerializeField]
	int heightMultiplayer;

	[SerializeField]
	GameObject textPrefab;

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
		canvasObj = FindAnyObjectByType<Canvas>().gameObject;
		playerData = FindAnyObjectByType<PlayerData>();
	}
	private void Start()
	{
		FirtsSlot();

	}
	void FirtsSlot()
	{
		for (int i = 0; i < amountOfSlots; i++)
		{
			grids.Add(new Grid());
			for (int j = 0; j < amountOfSlots; j++)
			{
				grids[i].cells.Add(new Cell());
				//grids[i].cells[j].num = UnityEngine.Random.Range(0, 9);
				grids[i].cells[j].num = 1;
				grids[i].cells[j].height = amountOfSlots - i * heightMultiplayer;
				GameObject text = Instantiate(textPrefab, new Vector3(j * heightMultiplayer + 100, grids[i].cells[j].height + 900), Quaternion.identity);
				text.transform.SetParent(canvasObj.transform);
				grids[i].cells[j].cellText = text.GetComponent<TMP_Text>();
				grids[i].cells[j].cellText.text = grids[i].cells[j].num.ToString();
			}
		}
		StartCoroutine(SpinLenght());
		CheckForBigWin();
	}
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
				grids[i].cells[j].num = UnityEngine.Random.Range(0, 9);
				grids[i].cells[j].cellText.text = grids[i].cells[j].num.ToString();
			}
		}
		StartCoroutine(SpinLenght());
		CheckForBigWin();
	}

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
					grids[i].cells[j].cellText.text = UnityEngine.Random.Range(0, 9).ToString();
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
