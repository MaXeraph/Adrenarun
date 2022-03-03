using UnityEngine;
public static class MathModule
{
    /**
     * Calculates the arc length of the quadratic in the form f(x) = ax^2 + bx + c from x = m to x = n.
     * The arc length of a function is the definite integral of (dy/dx)^2 + 1.
     */
    public static float calculateQuadraticArcLength(float a, float b, float c, float m, float n)
    {
        float[] integralCoefficients = new float[] {4 * a * a / 3, 4 * a * b / 2, b * b, 1};

        float Fm = 0f;
        float Fn = 0f;
        for (int i = 3; i > 0; i--)
        {
            Fm += integralCoefficients[i] * m;
            Fn += integralCoefficients[i] * n;
        }

        return Fm - Fn;
    }

    public static float findCubicRoot(float a, float b, float c, float d)
    {
	    // Change of variable to change form to a depressed cubic.
	    // t = x + b/3a.
	    float tc = b/(3*a);
	    float p = (3 * a * c - b * b)/(3 * a * a);
	    float q = (2 * b * b * b - 9 * a * b * c + 27 * a * a * d) / (27 * a * a * a);
	    
	    // Cardano's formula.
	    if (4 * p * p * p + 27 * q * q > 0) // There is one real root.
	    {
		    return Mathf.Pow(-1 * q / 2 + Mathf.Pow(q * q / 4 + p * p * p / 27, 1f / 2f), 1f / 3f) +
		           Mathf.Pow(-1 * q / 2 - Mathf.Pow(q * q / 4 + p * p * p / 27, 1f / 2f), 1f / 3f) - tc;
	    }

	    return -1;
    }

    public static float findNextX(float a, float b, float c, float curX, float arcLen)
    {
	    float[] integralCoefficients = new float[] {4 * a * a / 3, 4 * a * b / 2, b * b + 1, 0};
	    float FcurX = 1 / (4 * a) * (
		    (2 * a * curX + b) * Mathf.Pow(1 + (2 * a * curX + b) * (2 * a * curX + b), 1f / 2f)
		    + Mathf.Log(Mathf.Abs(2 * a * curX + b + Mathf.Pow(1 + (2 * a * curX + b) * (2 * a * curX + b), 1f / 2f)))
	    );
	    float d = -1 * (arcLen + Mathf.Pow(FcurX, 1f/2f));
	    return findCubicRoot(integralCoefficients[0], integralCoefficients[1], integralCoefficients[2], d);
    }

    /**
     * Return the normal of the plane defined by three points a, b, c.
     */
    public static Vector3 determinePlaneNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        return Vector3.Cross(b - a, c - a).normalized;
    }

    public static Vector3 planeXY = (new Vector3(0, 0, 1)).normalized;
    public static Vector3 convertToXY(Vector3 plane, Vector3 pointInPlane, float d)
    {
        float angle = Vector3.SignedAngle(plane, planeXY, Vector3.up);
        return Quaternion.Euler(0, angle, 0) * (pointInPlane - plane*(-1*d/Mathf.Pow(plane.x*plane.x + plane.y*plane.y + plane.z*plane.z, 1f/2f)));
    }

    public static Vector3 convertToPlane(Vector3 plane, Vector3 pointInPlaneXY, float d)
    {
        float angle = -1*Vector3.SignedAngle(plane, planeXY, Vector3.up);
        return (Quaternion.Euler(0, angle, 0) * pointInPlaneXY) + plane*(-1*d/Mathf.Pow(plane.x*plane.x + plane.y*plane.y + plane.z*plane.z, 1f/2f));
    }
    
	/**
	* Fill a matrix row first from an array of vectors.
	*/
	private static void fillMatrix(ref Matrix4x4 matrix, Vector3[] vectors) {
		for (int i = 0; i < vectors.Length; i++) {
			matrix[i, 0] = vectors[i].x * vectors[i].x;
			matrix[i, 1] = vectors[i].x;
			matrix[i, 2] = 1;
			matrix[i, 3] = vectors[i].y;
		}
	}

	private static void replaceMatrixCol(ref Matrix4x4 matrix, int col) {
		for (int i = 0; i < 3; i++) {
			matrix[i, col] = matrix[i, 3];
		}
	}

	private static float calculate3x3Det(Matrix4x4 matrix) {
		return matrix[0, 0] * calculate2x2Det(new float[2, 2] { {matrix[1, 1], matrix[1, 2]}, {matrix[2, 1], matrix[2, 2]} }) -
			matrix[0, 1] * calculate2x2Det(new float[2, 2] { {matrix[1, 0], matrix[1, 2]}, {matrix[2, 0], matrix[2, 2]} }) +
			matrix[0, 2] * calculate2x2Det(new float[2, 2] { {matrix[1, 0], matrix[1, 1]}, {matrix[2, 0], matrix[2, 1]} });
	}

	private static float calculate2x2Det(float[,] matrix) {
		return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
	}

	/**
	* Given three points in the XY plane, find the coefficient array [a, b, c] using Cramer's Rule.
	*/
	public static float[] calculateQuadratic(Vector3[] points) {
		Matrix4x4 m = new Matrix4x4();
		fillMatrix(ref m, points);
		Matrix4x4 ma = new Matrix4x4();
		fillMatrix(ref ma, points);
		replaceMatrixCol(ref ma, 0);
		Matrix4x4 mb = new Matrix4x4();
		fillMatrix(ref mb, points);
		replaceMatrixCol(ref mb, 1);
		Matrix4x4 mc = new Matrix4x4();
		fillMatrix(ref mc, points);
		replaceMatrixCol(ref mc, 2);
		float mDet = calculate3x3Det(m);
		float maDet = calculate3x3Det(ma);
		float mbDet = calculate3x3Det(mb);
		float mcDet = calculate3x3Det(mc);
		return new float[3] { maDet/mDet, mbDet/mDet, mcDet/mDet };
	}
}