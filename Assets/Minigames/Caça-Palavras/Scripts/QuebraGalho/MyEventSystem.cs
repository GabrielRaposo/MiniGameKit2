using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using System;

namespace Words
{	
		public class MyEventSystem : EventSystem
		{
			public MyInputModule inputModule;

			protected override void OnEnable()
			{
			}

			protected override void Update()
			{
				EventSystem originalCurrent = EventSystem.current;
				current = this; // in order to avoid reimplementing half of the EventSystem class, just temporarily assign this EventSystem to be the globally current one				

				if (inputModule != null)
				{
					inputModule.ActivateModule();
					inputModule.UpdateModule();
					inputModule.Process();
				}
				current = originalCurrent;
			}
		}	
}

