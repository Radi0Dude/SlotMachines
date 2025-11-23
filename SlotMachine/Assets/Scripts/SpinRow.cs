using UnityEngine;
using UnityEngine.EventSystems;

public class SpinRow : MonoBehaviour, IPointerClickHandler
{

	public GridHolder gridHolder;
	public int rowIndex = -1;
	public int columnIndex = -1;


	public void OnPointerClick(PointerEventData eventData)
	{
		if(rowIndex == -1) 
		{ 
			gridHolder.ColSpin(columnIndex);
			return; 
		}
		gridHolder.RowSpin(rowIndex);

	}
}
