using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(Audio_Attribute))]
public class Audio_Attribute_Editor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Audio_Attribute audio_attribute = (Audio_Attribute)attribute;

        int index = new List<string>(audio_attribute.Names).IndexOf(property.stringValue);
        index = EditorGUI.Popup(position, property.displayName, index, audio_attribute.Names);
        if( index != -1 )
        {
            property.stringValue = audio_attribute.Names[index];
        }
    }
}
