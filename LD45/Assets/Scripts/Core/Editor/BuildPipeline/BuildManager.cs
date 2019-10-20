using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Ionic.Zip;

using Debug = UnityEngine.Debug;

//TODO: publish to itch.io via butler
public static class BuildManager {
	//TODO: move to settings that can be changed in editor;
	const string butlerRelativePath = @"Thirdparty/Editor/butler/butler.exe";
	static string[] channelNames = new string[] {
		"windows-32",
		"windows-64",
		"linux-universal",
		"osx-universal",
		"webgl",
	};


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

	// Для пабліша на itch.io не треба zip. Він все одно розархівується, а потім заархівується по новому, так як треба itch
	// Але я це оставив, щоб не робити архіви ручками, якщо щось треба отправить
	public static void BuildAll(bool needZip, bool needPush) {
		Debug.Log("Start building all");
		DateTime startTime = DateTime.Now;
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		List<string> buildsPath = new List<string>(5);
		buildsPath.Add("Builds/Cave_0.0.4.11_Windows");
		buildsPath.Add(BuildWindows(true, needZip));
		buildsPath.Add(BuildWindowsX64(true, needZip));
		buildsPath.Add(BuildLinux(true, needZip));
		buildsPath.Add(BuildOSX(true, needZip));
		buildsPath.Add(BuildWeb(true, needZip));

		EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroupBeforeStart, targetBeforeStart);
		Debug.Log($"End building all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");

		if (needPush) {
			PushAll(buildsPath);
		}

		++LastBuildPatch;
	}

	public static void PushAll(List<string> buildsPath) {
		DateTime startTime = DateTime.Now;
		Debug.Log($"Start pushing all");

		StringBuilder command = new StringBuilder(256);
		for(byte i = 0; i < buildsPath.Count; ++i) {
			command.Append("\"");
			command.Append(Application.dataPath);
			command.Append("/");
			command.Append(butlerRelativePath);
			command.Append("\" ");

			command.Append("push \"");
			command.Append(Application.dataPath);
			command.Append("/../");
			command.Append(buildsPath[i]);
			command.Append("\" ");

			command.Append($"teamon/{PlayerSettings.productName}:{channelNames[i]} ");
			command.Append($"--userversion {PlayerSettings.bundleVersion}.{LastBuildPatch} ");

			Debug.Log(command.ToString());
			//Process.Start("CMD.exe", command.ToString());
			command.Clear();
		}

		Debug.Log($"End pushing all. Elapsed time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}");
	}

	public static string BuildWindows(bool isInBuildSequence, bool needZip) {
		return BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			needZip,
			$"_Windows/",
			$"_Windows/{PlayerSettings.productName}.exe"
		);
	}

	public static string BuildWindowsX64(bool isInBuildSequence, bool needZip) {
		return BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			needZip, 
			"_Windows64",
			$"_Windows64/{PlayerSettings.productName}.exe"
		);
	}

	public static string BuildLinux(bool isInBuildSequence, bool needZip) {
		return BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			needZip, 
			"_Linux",
			$"_Linux/{PlayerSettings.productName}.x86_64"
		);
	}

	public static string BuildOSX(bool isInBuildSequence, bool needZip) {
		return BaseBuild(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX,
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer,
			!isInBuildSequence,
			!isInBuildSequence,
			needZip,
			"_OSX",
			$"_OSX/{PlayerSettings.productName}"
		);
	}

	public static string BuildWeb(bool isInBuildSequence, bool needZip) {
		return BaseBuild(
			BuildTargetGroup.WebGL, BuildTarget.WebGL, 
			isInBuildSequence ? BuildOptions.None : BuildOptions.ShowBuiltPlayer, 
			!isInBuildSequence,
			!isInBuildSequence,
			needZip,
			"_Web",
			$"_Web"
		);
	}

	static string BaseBuild(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, BuildOptions buildOptions, bool needReturnBuildTarget, bool incrementPatch, bool needZip, string folderPath, string buildPath) {
		string basePath = $"Builds/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.{LastBuildPatch}";
		BuildTarget targetBeforeStart = EditorUserBuildSettings.activeBuildTarget;
		BuildTargetGroup targetGroupBeforeStart = BuildPipeline.GetBuildTargetGroup(targetBeforeStart);

		if (LastBundleVersion != PlayerSettings.bundleVersion) {
			LastBundleVersion = PlayerSettings.bundleVersion;
			LastBuildPatch = 0;
		}

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions {
			scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
			locationPathName = basePath + buildPath,
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

			if (needZip)
				Compress(basePath + folderPath);
		}
		else if (summary.result == BuildResult.Failed) {
			Debug.Log(
				$"{summary.platform} failed.   \t Time: {string.Format("{0:mm\\:ss}", summary.totalTime)}  \t Size: {summary.totalSize / 1048576}" + "\n" +
				$"Warnings: {summary.totalWarnings}" + "\n" +
				$"Errors:   {summary.totalErrors}"
			);
		}

		if (incrementPatch)
			++LastBuildPatch;

		if (needReturnBuildTarget)
			EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroupBeforeStart, targetBeforeStart);

		return basePath + folderPath;
	}

	public static void Compress(string dirPath) {
		using (ZipFile zip = new ZipFile()) {
			DateTime startTime = DateTime.Now;
			zip.AddDirectory(dirPath);
			zip.Save(dirPath + ".zip");

			long uncompresedSize = 0;
			long compresedSize = 0;
			foreach (var e in zip.Entries) {
				uncompresedSize += e.UncompressedSize;
				compresedSize += e.CompressedSize;
			}
			Debug.Log($"Make .ZIP.  \t\t\t Time: {string.Format("{0:mm\\:ss}", DateTime.Now - startTime)}  \t Size: {uncompresedSize / 1048576} - {compresedSize / 1048576}");
		}
	}
}
