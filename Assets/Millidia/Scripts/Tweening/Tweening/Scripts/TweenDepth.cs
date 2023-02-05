//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the UGUI width and hegith
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Depth")]
public class TweenDepth : UITweener
{
	public float from;
	public float to;

	Camera mTrans;
	float hv;

	/// <summary>
	/// Interpolate the position, scale, and rotation.
	/// </summary>

	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (true)
		{
			if (mTrans == null)
			{
				mTrans = GetComponent<Camera> (); 
			}

			if (true)
			{
				mTrans.depth = from * (1f - factor) + to * factor;

			}
			else
			{

			}
		}
	}

	/// <summary>
	/// Start the tweening operation from the current position/rotation/scale to the target transform.
	/// </summary>

	static public TweenDepth Begin(GameObject go, float duration, float to) { return Begin(go, duration, to); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenDepth Begin(GameObject go, float duration, float from, float to)
	{
		TweenDepth comp = UITweener.Begin<TweenDepth>(go, duration);
		comp.from = from;
		comp.to = to;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
