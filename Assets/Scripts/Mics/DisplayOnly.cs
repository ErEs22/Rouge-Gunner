using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 用于显示监视器面板的字段值，只能查看不能更改；用法：在字段前加上[DisplayOnly](属性前无效)
/// </summary>
public class DisplayOnly : PropertyAttribute
{

}
[CustomPropertyDrawer(typeof(DisplayOnly))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
