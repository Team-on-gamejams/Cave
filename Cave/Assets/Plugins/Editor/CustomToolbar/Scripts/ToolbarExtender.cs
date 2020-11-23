﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityToolbarExtender
{
	[InitializeOnLoad]
	public static class ToolbarExtender
	{
		static int m_toolCount;
		static GUIStyle m_commandStyle = null;

		public static readonly List<Action> LeftToolbarGUI = new List<Action>();
		public static readonly List<Action> RightToolbarGUI = new List<Action>();

		static ToolbarExtender()
		{
			Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
			FieldInfo toolIcons = toolbarType.GetField("s_ShownToolIcons",
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			var array = ( Array ) toolIcons.GetValue( null );
			m_toolCount = array != null ? array.Length : 6;

			ToolbarCallback.OnToolbarGUI -= OnGUI;
			ToolbarCallback.OnToolbarGUI += OnGUI;
		}

		static void OnGUI()
		{
			// Create two containers, left and right
			// Screen is whole toolbar

			if (m_commandStyle == null)
			{
				m_commandStyle = new GUIStyle("CommandLeft");
			}

			var screenWidth = EditorGUIUtility.currentViewWidth;

			// Following calculations match code reflected from Toolbar.OldOnGUI()
			float playButtonsPosition = (screenWidth - 180) / 2;

			Rect leftRect = new Rect(0, 0, screenWidth, Screen.height);
			leftRect.xMin += 10; // Spacing left
			leftRect.xMin += 32 * m_toolCount; // Tool buttons
			leftRect.xMin += 20; // Spacing between tools and pivot
			leftRect.xMin += 64 * 2; // Pivot buttons
			leftRect.xMax = playButtonsPosition;

			Rect rightRect = new Rect(0, 0, screenWidth, Screen.height);
			rightRect.xMin = playButtonsPosition + 50;
			rightRect.xMin += m_commandStyle.fixedWidth * 3; // Play buttons
			rightRect.xMax = screenWidth;
			rightRect.xMax -= 10; // Spacing right
			rightRect.xMax -= 80; // Layout
			rightRect.xMax -= 10; // Spacing between layout and layers
			rightRect.xMax -= 80; // Layers
			rightRect.xMax -= 20; // Spacing between layers and account
			rightRect.xMax -= 80; // Account
			rightRect.xMax -= 10; // Spacing between account and cloud
			rightRect.xMax -= 32; // Cloud
			//rightRect.xMax -= 10; // Spacing between cloud and collab
			//rightRect.xMax -= 78; // Colab

			// Add spacing around existing controls
			leftRect.xMin -= 10;
			leftRect.xMax += 10;
			rightRect.xMin -= 20;
			rightRect.xMax += 10;

			// Add top and bottom margins
			leftRect.y = 5;
			leftRect.height = 24;
			rightRect.y = 5;
			rightRect.height = 24;

			if (leftRect.width > 0)
			{
				GUILayout.BeginArea(leftRect);
				GUILayout.BeginHorizontal();
				foreach (var handler in LeftToolbarGUI)
				{
					handler();
				}

				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}

			if (rightRect.width > 0)
			{
				GUILayout.BeginArea(rightRect);
				GUILayout.BeginHorizontal();
				foreach (var handler in RightToolbarGUI)
				{
					handler();
				}

				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
		}
	}
}