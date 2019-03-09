using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Words
{
	public class MyButton : Button
	{
		public EventSystem eventSystem;		

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;			

			// Selection tracking
			if (IsInteractable() && navigation.mode != Navigation.Mode.None)
				eventSystem.SetSelectedGameObject(gameObject, eventData);

			base.OnPointerDown(eventData);
		}

		public override void OnSelect(BaseEventData eventData)
		{
			Debug.Log("OnSelect");
			base.OnSelect(eventData);
		}

		public override void Select()
		{
			Debug.Log("Selected " + name);
			if (eventSystem.alreadySelecting)
				return;

			eventSystem.SetSelectedGameObject(gameObject);
		}

		protected override void Awake()
		{
			base.Awake();

			eventSystem = GetComponent<EventSystemProvider>().eventSystem;
			Debug.Log(eventSystem.name);
		}

	}
}

