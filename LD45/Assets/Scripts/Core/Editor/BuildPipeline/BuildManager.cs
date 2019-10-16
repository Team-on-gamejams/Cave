using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

//TODO: archivate build
//TODO: publish to itch.io
public static class BuildManager {
	public static string LastBundleVersion {
		get {
			if (_LastBundleVersion == null)
				_LastBundleVersion = PlayerPrefs.GetString("BuildManager.LastBundleVersion", "0.0.0.0");
			return _LastBundleVersion;
		}
		set {
			_LastBundleVersion = value;
			PlayerPrefs.SetString("BuildManager.LastBundleVersion", _LastBundleVersion);
			PlayerPrefs.Save();
		}
	}
	public static int LastBuildPatch {
		get {
			if (_LastBuildPatch == -1)
				_LastBuildPatch = PlayerPrefs.GetInt("BuildManager.LastBuildPatch", 0);
			return _LastBuildPatch;
		}
		set {
			_LastBuildPatch = value;
			PlayerPrefs.SetInt("BuildManager.LastBuildPatch", _LastBuildPatch);
			PlayerPrefs.Save();
		}
	}
	static string _LastBundleVersion = null;
	static int _LastBuildPatch = -1;

	public static void BuildAll() {
		Debug.Log("Start building all");
		DateTime startTime = DateTime.Now;
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		BuildWindows(true);
		BuildWindowsX64(true);
		BuildLinux(true);
		BuildOSX(true);
		//TODO: incoment after web build fix
		//BuildWeb(true);

		++LastBuildPatch;

		EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroupBeforeStart, targetBeforeStart);

		Debug.Log($"End building all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");
	}

	public static void BuildWindows(bool isInBuildSequence) {
		BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			$"_Windows/{PlayerSettings.productName}.exe"
		);
	}

	public static void BuildWindowsX64(bool isInBuildSequence) {
		BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			$"_Windows64/{PlayerSettings.productName}.exe"
		);
	}

	public static void BuildLinux(bool isInBuildSequence) {
		BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			$"_Linux/{PlayerSettings.productName}.x86_64"
		);
	}

	public static void BuildOSX(bool isInBuildSequence) {
		BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			$"_OSX/{PlayerSettings.productName}"
		);
	}

	public static void BuildWeb(bool isInBuildSequence) {
		BaseBuild(
			BuildTargetGroup.WebGL, BuildTarget.WebGL, 
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer, 
			!isInBuildSequence,
			!isInBuildSequence,
			$"_Web"
		);
	}

	static void BaseBuild(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions, bool needReturnBuildTarget, bool incrementPatch, string buildName) {
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		if (LastBundleVersion != PlayerSettings.bundleVersion) {
			LastBundleVersion = PlayerSettings.bundleVersion;
			LastBuildPatch = 0;
		}

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions {
			scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
			locationPathName = $"Builds/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.{LastBuildPatch}" + buildName,
			targetGroup = buildTargetGroup,
			target = buildTarget,
			options = buildOptions,
		};


		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		//TODO: Зробити вивід гарнішим. Щоб виглядало по типу таблиці.
		//Зараз \t не вирівнює його, коли summary.platform дуже різних довжин, наприклад StandaloneWindows та StandaloneOSX
		if (summary.result == BuildResult.Succeeded) {
			Debug.Log($"{summary.platform} succeeded.  \t Time: {string.Format("{0:mm\\:ss}", summary.totalTime)}  \t Size: {summary.totalSize / 1048576f}");

		}
		else if (summary.result == BuildResult.Failed) {
			Debug.Log(
				$"{summary.platform} failed.   \t Time: {string.Format("{0:mm\\:ss}", summary.totalTime)}  \t Size: {summary.totalSize / 1048576f}" + "\n" +
				$"Warnings: {summary.totalWarnings}" + "\n" +
				$"Errors:   {summary.totalErrors}"
			);
		}

		if (incrementPatch)
			++LastBuildPatch;

		if (needReturnBuildTarget)
			EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroupBeforeStart, targetBeforeStart);
	}
}
