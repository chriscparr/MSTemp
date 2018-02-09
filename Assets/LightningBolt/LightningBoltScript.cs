using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightningBoltScript : MonoBehaviour
{
	[Tooltip("The game object where the lightning will emit from. If null, StartPosition is used.")]
	public GameObject StartObject;
	
	[Tooltip("The start position where the lightning will emit from. This is in world space if StartObject is null, otherwise this is offset from StartObject position.")]
	public Vector3 StartPosition;
	
	[Tooltip("The game object where the lightning will end at. If null, EndPosition is used.")]
	public GameObject EndObject;
	
	[Tooltip("The end position where the lightning will end at. This is in world space if EndObject is null, otherwise this is offset from EndObject position.")]
	public Vector3 EndPosition;
	
	[Range(0f, 8f), Tooltip("How manu generations? Higher numbers create more line segments.")]
	public int Generations = 6;
	
	[Range(0.01f, 1f), Tooltip("How long each bolt should last before creating a new bolt. In ManualMode, the bolt will simply disappear after this amount of seconds.")]
	public float Duration = 0.05f;
	
	private float timer;
	
	[Range(0f, 1f), Tooltip("How chaotic should the lightning be? (0-1)")]
	public float ChaosFactor = 0.15f;
	
	[Tooltip("In manual mode, the trigger method must be called to create a bolt")]
	public bool ManualMode;
	
	[Range(1f, 64f), Tooltip("The number of rows in the texture. Used for animation.")]
	public int Rows = 1;
	
	[Range(1f, 64f), Tooltip("The number of columns in the texture. Used for animation.")]
	public int Columns = 1;
	
	[Tooltip("The animation mode for the lightning")]
	public WrapMode LightningBoltAnimationMode = WrapMode.PingPong;

	
	[HideInInspector]
	[NonSerialized]
	public System.Random RandomGenerator = new System.Random();
	
	private LineRenderer lineRenderer;
	
	private List<KeyValuePair<Vector3, Vector3>> segments = new List<KeyValuePair<Vector3, Vector3>>();
	
	private int startIndex;
	
	private Vector2 size;
	
	private Vector2[] offsets;
	
	private int animationOffsetIndex;
	
	private int animationPingPongDirection = 1;
	
	private void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
	{
		if (directionNormalized == Vector3.zero)
		{
			side = Vector3.right;
		}
		else
		{
			float x = directionNormalized.x;
			float y = directionNormalized.y;
			float z = directionNormalized.z;
			float num = Mathf.Abs(x);
			float num2 = Mathf.Abs(y);
			float num3 = Mathf.Abs(z);
			float num4;
			float num5;
			float num6;
			if (num >= num2 && num2 >= num3)
			{
				num4 = 1f;
				num5 = 1f;
				num6 = -(y * num4 + z * num5) / x;
			}
			else if (num2 >= num3)
			{
				num6 = 1f;
				num5 = 1f;
				num4 = -(x * num6 + z * num5) / y;
			}
			else
			{
				num6 = 1f;
				num4 = 1f;
				num5 = -(x * num6 + y * num4) / z;
			}
			Vector3 vector = new Vector3(num6, num4, num5);
			side = vector.normalized;
		}
	}
	
	private void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount)
	{
		if (generation < 0 || generation > 8)
		{
			return;
		}
		this.segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));
		if (generation == 0)
		{
			return;
		}
		if (offsetAmount <= 0f)
		{
			offsetAmount = (end - start).magnitude * this.ChaosFactor;
		}
		while (generation-- > 0)
		{
			int num = this.startIndex;
			this.startIndex = this.segments.Count;
			for (int i = num; i < this.startIndex; i++)
			{
				start = this.segments[i].Key;
				end = this.segments[i].Value;
				Vector3 vector = (start + end) * 0.5f;
				Vector3 b;
				this.RandomVector(ref start, ref end, offsetAmount, out b);
				vector += b;
				this.segments.Add(new KeyValuePair<Vector3, Vector3>(start, vector));
				this.segments.Add(new KeyValuePair<Vector3, Vector3>(vector, end));
			}
			offsetAmount *= 0.5f;
		}
	}
	
	public void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result)
	{
		if (Camera.main.orthographic)
		{
			end.z = start.z;
			Vector3 normalized = (end - start).normalized;
			Vector3 a = new Vector3(-normalized.y, normalized.x, 0f);
			float d = (float)this.RandomGenerator.NextDouble() * offsetAmount * 2f - offsetAmount;
			result = a * d;
		}
		else
		{
			Vector3 normalized2 = (end - start).normalized;
			Vector3 point;
			this.GetPerpendicularVector(ref normalized2, out point);
			float d2 = ((float)this.RandomGenerator.NextDouble() + 0.1f) * offsetAmount;
			float angle = (float)this.RandomGenerator.NextDouble() * 360f;
			result = Quaternion.AngleAxis(angle, normalized2) * point * d2;
		}
	}
	
	private void SelectOffsetFromAnimationMode()
	{
		if (LightningBoltAnimationMode == WrapMode.Default)
		{
			this.lineRenderer.material.mainTextureOffset = this.offsets[0];
			return;
		}
		int num;
		if (LightningBoltAnimationMode == WrapMode.PingPong)
		{
			num = this.animationOffsetIndex;
			this.animationOffsetIndex += this.animationPingPongDirection;
			if (this.animationOffsetIndex >= this.offsets.Length)
			{
				this.animationOffsetIndex = this.offsets.Length - 2;
				this.animationPingPongDirection = -1;
			}
			else if (this.animationOffsetIndex < 0)
			{
				this.animationOffsetIndex = 1;
				this.animationPingPongDirection = 1;
			}
		}
		else if (LightningBoltAnimationMode == WrapMode.Loop)
		{
			num = this.animationOffsetIndex++;
			if (this.animationOffsetIndex >= this.offsets.Length)
			{
				this.animationOffsetIndex = 0;
			}
		}
		else
		{
			num = this.RandomGenerator.Next(0, this.offsets.Length);
		}
		if (num >= 0 && num < this.offsets.Length)
		{
			this.lineRenderer.material.mainTextureOffset = this.offsets[num];
		}
		else
		{
			this.lineRenderer.material.mainTextureOffset = this.offsets[0];
		}
	}
	
	private void UpdateLineRenderer()
	{
		int num = this.segments.Count - this.startIndex + 1;
		this.lineRenderer.SetVertexCount(num);
		if (num < 1)
		{
			return;
		}
		int num2 = 0;
		this.lineRenderer.SetPosition(num2++, this.segments[this.startIndex].Key);
		for (int i = this.startIndex; i < this.segments.Count; i++)
		{
			this.lineRenderer.SetPosition(num2++, this.segments[i].Value);
		}
		this.segments.Clear();
		this.SelectOffsetFromAnimationMode();
	}
	
	private void Start()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		this.lineRenderer.SetVertexCount(0);
		this.UpdateFromMaterialChange();
	}
	
	private void Update()
	{
		if (this.timer <= 0f)
		{
			if (this.ManualMode)
			{
				this.timer = this.Duration;
				this.lineRenderer.SetVertexCount(0);
			}
			else
			{
				this.Trigger();
			}
		}
		this.timer -= Time.deltaTime;
	}
	
	public void Trigger()
	{
		this.timer = this.Duration + Mathf.Min(0f, this.timer);
		Vector3 start;
		if (this.StartObject == null)
		{
			start = this.StartPosition;
		}
		else
		{
			start = this.StartObject.transform.position + this.StartPosition;
		}
		Vector3 end;
		if (this.EndObject == null)
		{
			end = this.EndPosition;
		}
		else
		{
			end = this.EndObject.transform.position + this.EndPosition;
		}
		this.startIndex = 0;
		this.GenerateLightningBolt(start, end, this.Generations, this.Generations, 0f);
		this.UpdateLineRenderer();
	}
	
	public void UpdateFromMaterialChange()
	{
		this.size = new Vector2(1f / (float)this.Columns, 1f / (float)this.Rows);
		this.lineRenderer.material.mainTextureScale = this.size;
		this.offsets = new Vector2[this.Rows * this.Columns];
		for (int i = 0; i < this.Rows; i++)
		{
			for (int j = 0; j < this.Columns; j++)
			{
				this.offsets[j + i * this.Columns] = new Vector2((float)j / (float)this.Columns, (float)i / (float)this.Rows);
			}
		}
	}
}
