using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics;

public class HelperFunctions : MonoBehaviour {
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetCurrentMethod() {
		StackTrace st = new StackTrace();
		StackFrame sf = st.GetFrame(1);

		return sf.GetMethod().Name;
	}

	public static bool GetEventWithChance(int percent) {
		int number = Random.Range(1, 101);
		return number <= percent;
	}

	public static void Shuffle<T>(IList<T> list) {
		int n = list.Count;
		while (n > 1) {
			n--;
			int rand = Random.Range(0, n + 1);
			T value = list[rand];
			list[rand] = list[n];
			list[n] = value;
		}
	}
}
