using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager {
	//Global app events
	public static event EventController.MethodContainer OnApplicationStart;
	public void CallOnOnApplicationStart(EventData ob = null) => OnApplicationStart?.Invoke(ob);
	public static event EventController.MethodContainer OnApplicationExit;
	public void CallOnOnApplicationExit(EventData ob = null) => OnApplicationExit?.Invoke(ob);

	//Scene Loader
	public static event EventController.MethodContainer OnSceneNeedLoad;
	public void CallOnSceneNeedLoad(EventData ob = null) => OnSceneNeedLoad?.Invoke(ob);

	public static event EventController.MethodContainer OnSceneLoadStart;
	public void CallOnSceneLoadStart(EventData ob = null) => OnSceneLoadStart?.Invoke(ob);

	public static event EventController.MethodContainer OnSceneLoadEnd;
	public void CallOnSceneLoadEnd(EventData ob = null) => OnSceneLoadEnd?.Invoke(ob);

	//Settings
	public static event EventController.MethodContainer OnScreenResolutionChange;
	public void CallOnScreenResolutionChange(EventData ob = null) => OnScreenResolutionChange?.Invoke(ob);

    //Old LD 45
    public static event EventController.MethodContainer OnPauseChanged;
    public static void CallOnPauseChanged(EventData ob = null) => OnPauseChanged?.Invoke(ob);

    public static event EventController.MethodContainer OnGameSpeedChanged;
    public static void CallOnGameSpeedChanged(EventData ob = null) => OnGameSpeedChanged?.Invoke(ob);

    public static event EventController.MethodContainer OnBigMapShow;
    public static void CallOnBigMapShow(EventData ob = null) => OnBigMapShow?.Invoke(ob);

    public static event EventController.MethodContainer OnBigMapHide;
    public static void CallOnBigMapHide(EventData ob = null) => OnBigMapHide?.Invoke(ob);

    public static event EventController.MethodContainer OnMouseOverTip;
    public static void CallOnMouseOverTip(EventData ob = null) => OnMouseOverTip?.Invoke(ob);

    public static event EventController.MethodContainer OnEquipmentChange;
    public static void CallOnEquipmentChange(EventData ob = null) => OnEquipmentChange?.Invoke(ob);

    public static event EventController.MethodContainer OnBuildingCreation;
    public static void CallOnBuildingCreation(EventData ob = null) => OnBuildingCreation?.Invoke(ob);
}
