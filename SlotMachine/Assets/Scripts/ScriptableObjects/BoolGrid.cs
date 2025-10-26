using UnityEngine;

[CreateAssetMenu(fileName = "BoolGrid", menuName = "Scriptable Objects/BoolGrid")]
public class BoolGrid : ScriptableObject
{
	public int height = 5;
	public int width = 5;
	public bool[] cells;

	public void Initialize()
	{
		if(cells == null || cells.Length != width * height)
		{
			cells = new bool[width * height];
		}
	}

	public bool GetCell(int x, int y)
	{
		return cells[y * width + x];
	}
	public bool SetCell(int x, int y, bool value)
	{
		return cells[y * width + x] = value;
	}
}
