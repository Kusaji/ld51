using UnityEngine;
using System.Collections;
using System;

public class SmoothFunc : MonoBehaviour {

	public static float QuarterSmoothStep(float p_t){
		return (p_t * p_t * (3f - 2f * p_t)) * 0.25f + p_t * 0.75f;
	}
	public static float HalfSmoothStep(float p_t){
		return (p_t * p_t * (3f - 2f * p_t)) * 0.5f + p_t * 0.5f;
	}
	public static float SmoothStep(float p_t){
		return p_t * p_t * (3f - 2f * p_t);
	}
	public static float InverseSmoothStep(float p_t)
	{
		return p_t + (p_t - (p_t * p_t * (3f - 2f * p_t)));
	}
	public static float InverseSmoothStepDouble(float p_t)
	{
		return p_t + (p_t - (p_t * p_t * (4f - 3f * p_t)));
	}
	public static float DoubleSmoothStep(float p_t)
	{
		return p_t * p_t * p_t * (4f - 3f * p_t);
	}
	public static float SmoothStopDouble(float p_t){
		return 1f - Mathf.Pow(1f - Mathf.Clamp01(p_t), 4f);
	}
	public static float SmoothStop(float p_t){
		return 1f - Mathf.Pow(1f - Mathf.Clamp01(p_t), 2f);
	}
	public static float SmoothStopUltra(float p_t){
		return 1f - Mathf.Pow(1f - Mathf.Clamp01(p_t), 100f);
	}
	public static float SmoothStopVariable(float p_t, float p_factor){
		return 1f - Mathf.Pow(1f - Mathf.Clamp01(p_t), p_factor);
	}
	public static float SmoothStart(float p_t){
		return p_t * p_t;
	}
	public static float SmoothStartDouble(float p_t)
	{
		return p_t * p_t * p_t;
	}
	public static float SmoothStartTriple(float p_t){
		return p_t * p_t * p_t * p_t;
	}
	public static float SmoothStartVariable(float p_t, float p_p)
	{
		return Mathf.Pow(p_t, p_p);
	}
	public static float SinTween(float p_t){
		return 1f + Mathf.Sin (Mathf.Lerp (Mathf.PI, Mathf.PI * 2f, Mathf.Clamp01(p_t)));
	}
	public static float Damp(float source, float target, float smoothing, float dt){
		// Smoothing rate dictates the proportion of source remaining after one second
		return Mathf.Lerp (source, target, 1f - Mathf.Pow (smoothing, dt));
	}
	public static Vector3 Damp(Vector3 source, Vector3 target, float smoothing, float dt){
		return Vector3.Lerp (source, target, 1f - Mathf.Pow (smoothing, dt));
	}
	public static Quaternion Damp(Quaternion source, Quaternion target, float smoothing, float dt){
		return Quaternion.Lerp (source, target, 1f - Mathf.Pow (smoothing, dt));
	}
	public static Color Damp(Color source, Color target, float smoothing, float dt)
	{
		return Color.Lerp(source, target, 1f - Mathf.Pow(smoothing, dt));
	}

    internal static float SmoothStopVariable(float v, object chaseSmoothStopVar)
    {
        throw new NotImplementedException();
    }
}
