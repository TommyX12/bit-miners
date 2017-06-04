﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

static public class Util
{
	
	public const float PI2 = Mathf.PI * 2.0f;
	
	private static Stack<Random.State> _randomSeedStack = new Stack<Random.State>();
	
	static public void PushRandomSeed(int seed)
	{
		_randomSeedStack.Push(Random.state);
		Random.InitState(seed);
	}
	
	static public void PopRandomSeed()
	{
		if (_randomSeedStack.Count == 0) return;
		Random.state = _randomSeedStack.Pop();
	}
	
	static public float[] ColorToFloatArray(Color color)
	{
		return new float[]{color.r, color.g, color.b, color.a};
	}
	
	static public float RotationDist(float start, float end, bool useRadians)
	{
		float cap = useRadians ? PI2 * 2.0f : 360.0f;
		float dif = (end - start) % cap;
		if (dif != dif % (cap / 2)) {
			dif = (dif < 0) ? dif + cap : dif - cap;
		}
		return dif;
	}
	
	static public float GetAngle(Vector2 target, Vector2 origin, bool radian)
	{
		float dx = -(target.x - origin.x);  
		float dy = -(target.y - origin.y);
		return radian ? Mathf.Atan2(dy, dx) : Mathf.Atan2(dy, dx) * 180.0f / Mathf.PI;
	}
	
	static public string AlphaNumericOnly(string str)
	{
		return Regex.Replace(str, "[^A-Za-z0-9_]", "");
	}
	
	static public int StringHash(string str)
	{
		int hash = 5381;
		foreach (char chr in str) hash = ((hash << 5) + hash) + Convert.ToInt32(chr);
		return hash;
	}

	static public Component MakeChild(Transform parentTransform, Component childTemplate)
	{
		Component newObject = (Component)UnityEngine.Object.Instantiate(childTemplate, parentTransform.position, parentTransform.rotation); // Instantiate
		newObject.transform.parent = parentTransform; // Set as child
		newObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		
		return newObject;
	}
	
	static public void DestroyAllChildren(Transform parentTransform)
	{
		foreach(Transform child in parentTransform){
			GameObject.Destroy(child.gameObject);
		}
	}
	
	static public GameObject MakeChild(Transform parentTransform, GameObject childTemplate)
	{
		GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(childTemplate, parentTransform.position, parentTransform.rotation); // Instantiate
		newObject.transform.parent = parentTransform; // Set as child
		newObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		
		return newObject;
	}
	
	static public Vector2 CameraToLocal(Camera camera, Vector2 cameraPoint, Transform transform)
	{
		Vector3 globalPoint = cameraPoint;
		globalPoint.z = transform.position.z - camera.transform.position.z;
		return transform.InverseTransformPoint(camera.ScreenToWorldPoint(globalPoint));
	}
	
	static public float Distance(float dx, float dy)
	{
		return (float)Math.Sqrt(dx*dx + dy*dy);
	}
	
	static public int Mod(int i, int n) {
		return (i % n + n) % n;
	}
	
	static public float Clamp(float val, float lower, float higher)
	{
		return val < lower ? lower : (val > higher ? higher : val);
	}
	
	static public int Clamp(int val, int lower, int higher)
	{
		return val < lower ? lower : (val > higher ? higher : val);
	}
	
	static public float Map(float x, float in_min, float in_max, float out_min, float out_max, bool clamped)
	{
		float result = (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		if (clamped) return Clamp(result, out_min, out_max);
		return result;
	}
	
	static public int ClosestPow2(int v)
	{
		int p = 2;
		while (p < v) p = p << 1;
		return p;
	}
	
	static public float RandomFloat(float min, float max) 
	{
		return Random.Range(min, max);
	}
	
	static public bool RandomChance(float chance)
	{
		return Random.Range(0.0f, 0.99999999f) < chance;
	}
	
	static public int RandomInt(int min, int max) 
	{
		return Random.Range(min, max);
	}
	
	static public T GetRandomElement<T>(List<T> list)
	{
		return list[RandomInt(list.Count)];
	}
	
	static public int RandomSelectChance(float[] chanceArray)
	{
		if (chanceArray.Length == 0) return -1;
		
		int options = chanceArray.Length - 1;
		int pointer = 0;
		float total = 0.0f;
		foreach (float i in chanceArray) total += i;
		float rand = RandomFloat(0.0f, total);
		while (pointer < options-1){
			float chance = chanceArray[pointer];
			if (rand < chance) break;
			rand -= chance;
			total -= chance;
			++pointer;
		}
		return pointer; 
	}
	
	static public T RandomSelectChance<T>(Dictionary<T, float> optionChanceDictionary)
	{
		if (optionChanceDictionary.Count == 0) return default(T);
		
		float total = 0.0f;
		foreach (float i in optionChanceDictionary.Values) total += i;
		float rand = RandomFloat(0.0f, total);
		T result = default(T);
		foreach (T option in optionChanceDictionary.Keys) {
			result = option;
			float chance = optionChanceDictionary[option];
			if (rand < chance) break;
			rand -= chance;
			total -= chance;
		}
		return result; 
	}
	
	static public int RandomInt(int max) 
	{
		return Random.Range(0, max);
	}
	
	static public Vector2 Vec2FromAngle(float angle, float magnitude)
	{
		return new Vector2(
				(float)Math.Cos(angle) * magnitude,
				(float)Math.Sin(angle) * magnitude
				);
	}
	
	static public Color Float4ToColor(float[] floats)
	{
		return new Color(floats[0], floats[1], floats[2], floats[3]);
	}
	
	static public void SetUIScreenPos(Canvas canvas, Camera camera, Vector2 screenPos, Transform elementTransform)
	{
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, camera, out pos);
		elementTransform.position = canvas.transform.TransformPoint(pos);
	}
}