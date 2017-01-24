using UnityEngine;
using System.Collections;

public class Math : MonoBehaviour {

	public static Matrix4x4 MatrixLerp(Matrix4x4 a, Matrix4x4 b, float t) {
		//function to lerp between the two 4x4 matrices a and b by float t.
		Matrix4x4 ret = new Matrix4x4 ();
		//defining new matrix which we will write the interpolated results to.
		for (int i = 0; i < 16; i++) {
			//looping through all values in the 4x4 matrix
			ret [i] = Mathf.Lerp (a [i], b [i], t);
			//assigning the interpolation of values for matrices a and b to the ret matrix;
		}
		return ret;
		//returning the interpolated matrix
	}
}
