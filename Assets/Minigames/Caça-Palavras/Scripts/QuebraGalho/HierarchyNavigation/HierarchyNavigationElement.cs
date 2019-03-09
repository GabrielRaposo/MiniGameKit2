using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


namespace Words
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Selectable))]
	public class HierarchyNavigationElement : MonoBehaviour, ICancelHandler
	{
		private EventSystem eventSystem;
		private HierarchyNavigationGroup group;

		public bool OverrideExitTarget { get { return overrideExitTarget; } set { overrideExitTarget = value; if(group != null) group.OrganizeHierarchyNavigation(); } }
		public bool OverridePreviousTarget { get { return overridePreviousTarget; } set { overridePreviousTarget = value; if (group != null) group.OrganizeHierarchyNavigation(); } }
		public bool OverrideNextTarget { get { return overrideNextTarget; } set { overrideNextTarget = value; if (group != null) group.OrganizeHierarchyNavigation(); } }


		private bool overrideExitTarget = false;
		private bool overridePreviousTarget = false;
		private bool overrideNextTarget = false;
		public Selectable exitTarget;
		public Selectable previousTarget;
		public Selectable nextTarget;

		public HierarchyNavigationGroup Group { get { return group; } }

		void OnEnable()
		{
			if (transform.parent != null)
				group = transform.parent.GetComponent<HierarchyNavigationGroup>();
			if (group != null)
			{
				eventSystem = group.eventSystem;
				if (!overrideExitTarget)
					exitTarget = group.childrenExitTarget;
			}
		}

		void OnTransformParentChanged()
		{
			OnEnable();
		}

		public void OnCancel(BaseEventData eventData)
		{
			if (!overrideExitTarget)
			{
				if (group == null)
					OnEnable();
				if (group != null)
					if (group.childrenExitTarget != null)
						eventSystem.SetSelectedGameObject(group.childrenExitTarget.gameObject);
			}
			else
			{
				if (exitTarget != null)
					eventSystem.SetSelectedGameObject(exitTarget.gameObject);
			}
		}
	}
}