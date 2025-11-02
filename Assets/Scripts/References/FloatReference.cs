using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[Serializable]
public class FloatReference
{
    public bool useConstant;
    public float constant;
    public FloatVariable variable;

    public float value
    {
        get { return useConstant ? constant : variable.Value; }
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(FloatReference))]
public class FloatReferencePropertyDrawr : PropertyDrawer
{
    SerializedProperty useConstantProperty;
    SerializedProperty constantProperty;
    SerializedProperty variableProperty;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect folfoutPos = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(folfoutPos, property.isExpanded, label);

        if(property.isExpanded)
        {
            Rect useConstanctPos = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 1, position.width, EditorGUIUtility.singleLineHeight);
            useConstantProperty = property.FindPropertyRelative("useConstant");
            EditorGUI.PropertyField(useConstanctPos, useConstantProperty);

            bool useConstant = useConstantProperty.boolValue;

            if (useConstant)
            {
                Rect constantPos = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight);
                constantProperty = property.FindPropertyRelative("constant");
                EditorGUI.PropertyField(constantPos, constantProperty);
            }
            else
            {
                Rect variablePos = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight);
                variableProperty = property.FindPropertyRelative("variable");
                EditorGUI.PropertyField(variablePos, variableProperty);
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.isExpanded ? EditorGUIUtility.singleLineHeight * 3 : EditorGUIUtility.singleLineHeight;
    }
}
#endif