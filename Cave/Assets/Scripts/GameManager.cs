using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using yaSingleton;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "GameManager", menuName = "Singletons/GameManager")]
public class GameManager : Singleton<GameManager> {
    public bool IsPaused {
        set {
            _IsPaused = value;
        }
        get {
            return _IsPaused;
        }
    }
    private bool _IsPaused;

    public Camera MainCamera => TemplateGameManager.Instance.Camera;
    public Player Player;

    protected override void Initialize() {
		base.Initialize();
	}

	protected override void Deinitialize() {
		base.Deinitialize();
	}
}
