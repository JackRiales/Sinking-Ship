using UnityEngine;
using System.Collections;

public class LERPTools {

	public static IEnumerator VectorLerp(Vector3 v1, Vector2 v2, float speed) {
		float startTime = Time.time;
		float length = Vector3.Distance(v1, v2);
		bool done = false;
		while (!done) {
			if (Vector3.Distance(v1, v2) <= 0) {
				done = true;
			}
			float lengthCovered = (Time.time - startTime) * speed;
			float fracLength = lengthCovered / length;
			v1 = Vector3.Lerp(v1, v2, fracLength);
			yield return null;
		}
	}
}
