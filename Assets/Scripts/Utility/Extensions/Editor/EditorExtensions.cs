using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public static class EditorExtensions {

	// -- WHAT

	/// <summary>
	/// Returns the value of a serialized property of a specified name.
	/// </summary>
	public static T GetSerializedReferenceProperty<T> (this UnityEngine.Object obj, string propertyName)
	where T : UnityEngine.Object {
		SerializedObject serializedObject = new UnityEditor.SerializedObject (obj);
		SerializedProperty property = serializedObject.FindProperty (propertyName);
		return property.objectReferenceValue as T;
	}

	/// <summary>
	/// Assigns a serialized property of a specified name to the specified reference.
	/// </summary>
	public static void SetSerializedReferenceProperty (this UnityEngine.Object obj, string propertyName, UnityEngine.Object reference) {
		SerializedObject serializedObject = new UnityEditor.SerializedObject (obj);
		SerializedProperty property = serializedObject.FindProperty (propertyName);
		property.objectReferenceValue = reference;
		serializedObject.ApplyModifiedProperties ();
	}

	/// <summary>
	/// Assigns a serialized property of a specified name to the specified integer value.
	/// </summary>
	public static void SetSerializedIntProperty (this UnityEngine.Object obj, string propertyName, int value) {
		SerializedObject serializedObject = new UnityEditor.SerializedObject (obj);
		SerializedProperty property = serializedObject.FindProperty (propertyName);
		property.intValue = value;
		serializedObject.ApplyModifiedProperties ();
	}

	/// <summary>
	/// Assigns a serialized property of a specified name to the specified integer value.
	/// </summary>
	public static void SetSerializedFloatProperty (this UnityEngine.Object obj, string propertyName, float value) {
		SerializedObject serializedObject = new UnityEditor.SerializedObject (obj);
		SerializedProperty property = serializedObject.FindProperty (propertyName);
		property.floatValue = value;
		serializedObject.ApplyModifiedProperties ();
	}
}
