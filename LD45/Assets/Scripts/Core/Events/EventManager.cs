using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager {
	public static event EventController.MethodContainer OnPauseChanged;
	public void CallOnPauseChanged(EventData ob = null) => OnPauseChanged?.Invoke(ob);

	public static event EventController.MethodContainer OnGameSpeedChanged;
	public void CallOnGameSpeedChanged(EventData ob = null) => OnGameSpeedChanged?.Invoke(ob);
	//Scene Loader
	public static event EventController.MethodContainer OnSceneNeedLoad;
	public void CallOnSceneNeedLoad(EventData ob = null) => OnSceneNeedLoad?.Invoke(ob);

	public static event EventController.MethodContainer OnSceneLoadStart;
	public void CallOnSceneLoadStart(EventData ob = null) => OnSceneLoadStart?.Invoke(ob);

	public static event EventController.MethodContainer OnSceneLoadEnd;
	public void CallOnSceneLoadEnd(EventData ob = null) => OnSceneLoadEnd?.Invoke(ob);

    public static event EventController.MethodContainer OnMouseOverTip;
    public void CallOnMouseOverTip(EventData ob = null) => OnMouseOverTip?.Invoke(ob);

	public static event EventController.MethodContainer OnEquipmentChange;
	public void CallOnEquipmentChange(EventData ob = null) => OnEquipmentChange?.Invoke(ob);
}
