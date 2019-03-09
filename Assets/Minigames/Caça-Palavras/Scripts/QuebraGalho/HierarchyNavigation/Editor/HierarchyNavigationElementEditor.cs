using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Words
{
	[CustomEditor(typeof(HierarchyNavigationElement))]
	public class HierarchyNavigationElementEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			HierarchyNavigationElement myTarget = (HierarchyNavigationElement)target;
			myTarget.OverrideExitTarget = EditorGUILayout.Toggle("Override Exit Target", myTarget.OverrideExitTarget);
			myTarget.OverrideNextTarget = EditorGUILayout.Toggle("Override Next Target", myTarget.OverrideNextTarget);
			myTarget.OverridePreviousTarget = EditorGUILayout.Toggle("Override Previous Target", myTarget.OverridePreviousTarget);

			if (myTarget.OverrideExitTarget)			
				myTarget.exitTarget = (Selectable)EditorGUILayout.ObjectField("Exit Target", myTarget.exitTarget, typeof(Selectable), false);			
			
			if (myTarget.OverrideNextTarget)
				myTarget.nextTarget = (Selectable)EditorGUILayout.ObjectField("Next Target", myTarget.nextTarget, typeof(Selectable), false);				
			
			if (myTarget.OverridePreviousTarget)
				myTarget.previousTarget = (Selectable)EditorGUILayout.ObjectField("Previous Target", myTarget.previousTarget, typeof(Selectable), false);				
		}
	}
}