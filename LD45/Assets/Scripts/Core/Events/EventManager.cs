using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager {
	public static event EventController.MethodContainer OnExampleEvent;
	public void CallOnExampleEvent(EventData ob = null) => OnExampleEvent?.Invoke(ob);

}
