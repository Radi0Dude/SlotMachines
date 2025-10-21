using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(boolHolder))]
public class PropertyDrawerBoolGrid : PropertyDrawer
{
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		var container = new VisualElement();

		var boolGridField = property.FindPropertyRelative("bools");

		if (boolGridField != null)
		{
			var propertyField = new PropertyField(boolGridField);
			container.Add(propertyField);
		}

		return container;
	}
}
