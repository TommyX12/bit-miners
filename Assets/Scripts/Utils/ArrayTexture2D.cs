using UnityEngine;
using System.Collections;
using System;

public class ArrayTexture2D
{
	public float[,] Data{get; set;}
	public int Width{get; set;}
	public int Height{get; set;}

	public ArrayTexture2D(int width, int height, float initialVal){
		Data = new float[width, height];
		Width = width;
		Height = height;
		for (int i = 0; i < width; ++i){
			for (int j = 0; j < height; ++j){
				Data[i, j] = initialVal;
			}
		}
	}
	
	public float Texture2DNearest(Vector2 pos) {
		int x = (int)Math.Round(pos.x * Width);
		int y = (int)Math.Round(pos.y * Height);
		return Get(x, y);
	}
	
	public float Texture2DLinear(Vector2 pos) {
		float x = pos.x * Width;
		float y = pos.y * Height;
		return GetLinear(x, y);
	}
	
	public float Get(int x, int y) {
		return Data[Util.Mod(x, Width), Util.Mod(y, Height)];
	}
	
	public float GetAlternative(int x, int y) {
		if (x < 0 || x >= Width) x = Util.Mod(x, Width-1);
		if (y < 0 || y >= Height) y = Util.Mod(y, Height-1);
		return Data[x, y];
	}
	
	public float GetLinear(float u, float v) {
		//sample 4 times and average
		u -= 0.5f;
		v -= 0.5f;
		int x = (int)Math.Floor(u);
		int y = (int)Math.Floor(v);
		float u_ratio = u - x;
		float v_ratio = v - y;
		float u_opposite = 1 - u_ratio;
		float v_opposite = 1 - v_ratio;
		return (Get(x, y) * u_opposite + Get(x+1, y) * u_ratio) * v_opposite + (Get(x, y+1) * u_opposite  + Get(x+1, y+1) * u_ratio) * v_ratio;
	}
	
	public void Set(int x, int y, float val) {
		Data[Util.Mod(x, Width), Util.Mod(y, Height)] = val;
	}
	
	public void Add(int x, int y, float val) {
		Data[Util.Mod(x, Width), Util.Mod(y, Height)] += val;
	}
	
	public void ReAverage(float maxShift)
	{
		maxShift = Math.Max(maxShift, 0.0f);
		// float min = 1.0f, max = 0.0f;
		float average = 0.0f;
		for (int i = 0; i < Width; ++i){
			for (int j = 0; j < Height; ++j){
				average += Data[i, j];
				// min = Math.Min(min, Data[i, j]);
				// max = Math.Max(max, Data[i, j]);
			}
		}
		// min = Math.Max(min, 0.0f);
		// max = Math.Min(max, 1.0f);
		average /= (Width * Height);
		
		// Debug.Log(average);
		
		if (average > 0.5f) {
			average = Math.Min(average, 0.5f + maxShift);
			for (int i = 0; i < Width; ++i){
				for (int j = 0; j < Height; ++j){
					Data[i, j] = Util.Map(Data[i, j], average, 1.0f, 0.5f, 1.0f, false);
				}
			}
		}
		else {
			average = Math.Max(average, 0.5f - maxShift);
			for (int i = 0; i < Width; ++i){
				for (int j = 0; j < Height; ++j){
					Data[i, j] = Util.Map(Data[i, j], 0.0f, average, 0.0f, 0.5f, false);
				}
			}
		}
		/* 
		average = 0.0f;
		for (int i = 0; i < Width; ++i){
			for (int j = 0; j < Height; ++j){
				average += Util.clamp(Data[i, j], 0.0f, 1.0f);
				// min = Math.Min(min, Data[i, j]);
				// max = Math.Max(max, Data[i, j]);
			}
		}
		// min = Math.Max(min, 0.0f);
		// max = Math.Min(max, 1.0f);
		average /= (Width * Height);
		
		Debug.Log(average); */
	}
	
}
