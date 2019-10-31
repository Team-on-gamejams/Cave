using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Ionic.Zip;

//TODO: publish to itch.io via butler
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

	public static void BuildAll(bool needZip) {
		Debug.Log("Start building all");
		DateTime startTime = DateTime.Now;
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		BuildWindows(true, needZip);
		BuildWindowsX64(true, needZip);
		BuildLinux(true, needZip);
		BuildOSX(true, needZip);
		BuildWeb(true, needZip);

		++LastBuildPatch;

		EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroupBeforeStart, targetBeforeStart);

		Debug.Log($"End building all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");
	}

	public static void BuildWindows(bool isInBuildSequence, bool needZip) {
		BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			needZip ? "_Windows" : null,
			$"_Windows/{PlayerSettings.productName}.exe"
		);
	}

	public static void BuildWindowsX64(bool isInBuildSequence, bool needZip) {
		BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			needZip ? "_Windows64" : null,
			$"_Windows64/{PlayerSettings.productName}.exe"
		);
	}

	public static void BuildLinux(bool isInBuildSequence, bool needZip) {
		BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			needZip ? "_Linux" : null,
			$"_Linux/{PlayerSettings.productName}.x86_64"
		);
	}

	public static void BuildOSX(bool isInBuildSequence, bool needZip) {
		BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			needZip ? "_OSX" : null,
			$"_OSX/{PlayerSettings.productName}"
		);
	}

	public static void BuildWeb(bool isInBuildSequence, bool needZip) {
		BaseBuild(
			BuildTargetGroup.WebGL, BuildTarget.WebGL, 
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer, 
			!isInBuildSequence,
			!isInBuildSequence,
			needZip ? "_Web" : null,
			$"_Web"
		);
	}

	static void BaseBuild(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions, bool needReturnBuildTarget, bool incrementPatch, string zipPath, string buildName) {
		string basePath = $"Builds/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.{LastBuildPatch}";
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		if (LastBundleVersion != PlayerSettings.bundleVersion) {
			LastBundleVersion = PlayerSettings.bundleVersion;
			LastBuildPatch = 0;
		}

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions {
			scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
			locationPathName = basePath + buildName,
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

			if (zipPath != null && zipPath != "")
				Compress(basePath + zipPath);
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

	public static void Compress(string dirPath) {
		using (ZipFile zip = new ZipFile()) {
			zip.AddDirectory(dirPath, $"{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.{LastBuildPatch}");
			zip.Save(dirPath + ".zip");
		}
	}
}
