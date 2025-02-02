#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CustomAttributes 
{
    [CustomPropertyDrawer(typeof(StringDropdownAttribute))]
    public class StringDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as StringDropdownAttribute;
            
            if (property.propertyType == SerializedPropertyType.String)
            {
                int selectedIndex = GetIndex(property.stringValue, attr.options);
                
                selectedIndex = EditorGUI.Popup(
                    position,
                    label.text,
                    selectedIndex,
                    attr.options);
                
                if (selectedIndex >= 0)
                {
                    property.stringValue = attr.options[selectedIndex];
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }

        private int GetIndex(string value, string[] options)
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i] == value) return i;
            }
            return 0;
        }
    }
}
#endif 