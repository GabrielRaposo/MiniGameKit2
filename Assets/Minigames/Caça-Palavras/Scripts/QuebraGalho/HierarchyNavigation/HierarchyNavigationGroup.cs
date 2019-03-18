using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

namespace Words
{
	[ExecuteInEditMode]
	//[RequireComponent(typeof(Selectable))]
	public class HierarchyNavigationGroup : MonoBehaviour, ISubmitHandler
	{
		public EventSystem eventSystem;
		public enum Mode { Vertical, Horizontal }
		public Mode mode;

		[Tooltip("GameObject selected when OnCancel is called from a child element of this hierarchy. Default is self.")]
		public Selectable childrenExitTarget;

		private List<Selectable> hierarchyNavigationElements = new List<Selectable>();

		public void UpdateHierarchyNavigationElements()
		{
			foreach (Selectable sel in hierarchyNavigationElements)
				sel.navigation = Navigation.defaultNavigation;
			hierarchyNavigationElements.Clear();

			for (int i = 0; i < transform.childCount; i++)
			{
				Selectable sel;
				if ((sel = transform.GetChild(i).GetComponent<Selectable>()) != null)
				{
					if (sel.GetComponent<HierarchyNavigationElement>() != null && sel.enabled)
						hierarchyNavigationElements.Add(sel);
				}
			}
		}

		public void OrganizeHierarchyNavigation()
		{
			for (int i = 0; i < hierarchyNavigationElements.Count; i++)
			{
				HierarchyNavigationElement ele = hierarchyNavigationElements[i].GetComponent<HierarchyNavigationElement>();
				Navigation nav = new Navigation();
				nav.mode = Navigation.Mode.Explicit;

				if (i + 1 < hierarchyNavigationElements.Count)
				{
					if (mode == Mode.Vertical)
					{
						if (ele.OverrideNextTarget)
							nav.selectOnDown = ele.nextTarget;
						else
							nav.selectOnDown = hierarchyNavigationElements[i + 1];
						

					}
					else
						if (ele.OverrideNextTarget)
							nav.selectOnRight = ele.nextTarget;
						else
							nav.selectOnRight = hierarchyNavigationElements[i + 1];
				}

				if (i > 0)
				{
					if (mode == Mode.Vertical)
						if (ele.OverridePreviousTarget)
							nav.selectOnUp = ele.previousTarget;
						else
							nav.selectOnUp = hierarchyNavigationElements[i - 1];
					else
						if (ele.OverridePreviousTarget)
							nav.selectOnLeft = ele.previousTarget;
						else
							nav.selectOnLeft = hierarchyNavigationElements[i - 1];
				}

				if (i == 0)
				{
					if (mode == Mode.Vertical)
					{
						if (ele.OverridePreviousTarget)
							nav.selectOnUp = ele.previousTarget;
						if (ele.OverrideNextTarget)
							nav.selectOnDown = ele.nextTarget;
					}
					else
					{
						if (ele.OverridePreviousTarget)
							nav.selectOnLeft = ele.previousTarget;
						if (ele.OverrideNextTarget)
							nav.selectOnRight = ele.nextTarget;
					}

				}

				hierarchyNavigationElements[i].navigation = nav;
			}


		}

		void OnEnable()
		{
			if(childrenExitTarget == null)
				childrenExitTarget = GetComponent<Selectable>();
			if (eventSystem == null)
				eventSystem = GetComponentInParent<EventSystem>();
			if (eventSystem == null)
				eventSystem = EventSystem.current;
			UpdateHierarchyNavigationElements();
			OrganizeHierarchyNavigation();
			UpdateChildrenExits();
		}

		private void UpdateChildrenExits()
		{
			foreach (Selectable sel in hierarchyNavigationElements)
			{
				HierarchyNavigationElement ele = sel.GetComponent<HierarchyNavigationElement>();
				if (ele != null && !ele.OverrideExitTarget)
					ele.exitTarget = childrenExitTarget;
			}
		}

		public void OnTransformChildrenChanged()
		{
			UpdateHierarchyNavigationElements();
			OrganizeHierarchyNavigation();
			UpdateChildrenExits();
		}

		public void OnSubmit(BaseEventData eventData)
		{
			if (hierarchyNavigationElements.Count > 0)
				eventSystem.SetSelectedGameObject(hierarchyNavigationElements[0].gameObject);
		}
	}
}