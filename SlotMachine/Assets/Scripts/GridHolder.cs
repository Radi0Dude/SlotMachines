using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using System.Runtime.ExceptionServices;

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


	private void Awake()
	{
		canvasObj = FindAnyObjectByType<Canvas>().gameObject;
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
				grids[i].cells[j].num = UnityEngine.Random.Range(0, 9);
				grids[i].cells[j].height = i * heightMultiplayer;
				GameObject text = Instantiate(textPrefab, new Vector3(j * heightMultiplayer + 100, grids[i].cells[j].height + 100), Quaternion.identity);
				text.transform.SetParent(canvasObj.transform);
				grids[i].cells[j].cellText = text.GetComponent<TMP_Text>();
				grids[i].cells[j].cellText.text = grids[i].cells[j].num.ToString();
			}
		}
	}
	public void MasterSlotButton()
	{
		for (int i = 0; i < amountOfSlots; i++)
		{
			for (int j = 0; j < amountOfSlots; j++)
			{
				grids[i].cells[j].num = UnityEngine.Random.Range(0, 9);
				grids[i].cells[j].cellText.text = grids[i].cells[j].num.ToString();
			}
		}
		CheckForWin();
	} 

	private void CheckForWin()
	{
		
	}

}


[Serializable]
class Grid
{
	public List<Cell> cells = new List<Cell>();
}
[Serializable]
class Cell
{
	public int num;
	public TMP_Text cellText;

	public int height;
}