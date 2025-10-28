using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

	[SerializeField]
	private int credits = 0;
	[SerializeField]
	private TMP_Text creditsText;

	GridHolder gridHolder;
	private void Awake()
	{
		gridHolder = FindAnyObjectByType<GridHolder>();
		UpdateCredits();
	}

	public void AddCredits(int amount)
	{
		credits += amount;
		UpdateCredits();
	}

	public void RemoveCredits(int amount)
	{
		credits -= amount;
		UpdateCredits();
	}
	public void ClickSlot(int prize)
	{
		if(gridHolder.isRunning)
			return;
		gridHolder.MasterSlotButton();
		RemoveCredits(prize);
	}

	public void UpdateCredits()
	{
		if (int.TryParse(creditsText.text.ToString(), out int prevCredit))
		{
			if (prevCredit != credits)
			{
				StartCoroutine(Lerp(prevCredit, credits));
			}
		}
		else
		{
			creditsText.text = credits.ToString();
		}

	}

	IEnumerator Lerp(int startNum, int endNum)
	{
		float duration = 0;
		while (true)
		{
			duration += Time.deltaTime;
			int newCredit = (int)Mathf.Lerp(startNum, endNum, duration);
			creditsText.text = newCredit.ToString();
			yield return new WaitForEndOfFrame();
		}

	}
}
