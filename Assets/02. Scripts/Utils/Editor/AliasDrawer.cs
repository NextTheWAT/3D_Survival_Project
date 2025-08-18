﻿#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils.Attribute;

namespace Utils.Editor
{
    [CustomPropertyDrawer(typeof(AliasAttribute), true)]
    public class AliasDrawer : PropertyDrawer
    {
        private AliasAttribute _attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _attribute = attribute as AliasAttribute;

            var fieldType = fieldInfo.FieldType;
            var isArrayType = fieldType.IsArray;
            var isGenericType = fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>);
            if (isArrayType || isGenericType)
            {
                Debug.LogWarning("Can't draw label attribute because it's an array type.");

                EditorGUI.PropertyField(position, property, label);

                return;
            }

            EditorGUI.PropertyField(position, property, new GUIContent(_attribute.Text), true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}

#endif
