using UnityEngine;
using System.Collections;
using System;

public static class MapGenerator
{
	
	private static string imageFolder = "\\data\\_egimages\\";
	
	private static bool _randomwalk_saveImage = true;
	public static ArrayTexture2D generate_RandomWalk(int width, int height, float initialValue, int ptrX, int ptrY, int steps, float increment, bool longerCorridors) {
		ArrayTexture2D sampler = new ArrayTexture2D(width, height, initialValue);
		
		// var a;
		int _steps = steps;
		ptrX = Util.Clamp(ptrX, 0, width - 1);
		ptrY = Util.Clamp(ptrY, 0, height - 1);
		int _ptrX = ptrX;
		int _ptrY = ptrY;
		/* if (_randomwalk_saveImage){
			a = new BitmapData(width, height, true, Convert.colorFloat(initialValue, initialValue, initialValue, 1));
		} */
		
		int last = Util.RandomInt(4);
		
		int[] directionAvailable = new int[4];
		
		while (steps > 0) {
			sampler.Add(ptrX, ptrY, increment);
			// if (_randomwalk_saveImage) a.setPixel(ptrX, ptrY, Convert.colorFloat(sampler.data[ptrX, ptrY], sampler.data[ptrX, ptrY], sampler.data[ptrX, ptrY], 1));
			int selection;
			
			bool lastAvailable = false;
			
			int ptr = 0;
			if (ptrY + 1 < height) {
				directionAvailable[ptr++] = 0;
				if (last == 0) lastAvailable = true;
			}
			if (ptrY - 1 >= 0) {
				directionAvailable[ptr++] = 1;
				if (last == 1) lastAvailable = true;
			}
			if (ptrX + 1 < width) {
				directionAvailable[ptr++] = 2;
				if (last == 2) lastAvailable = true;
			}
			if (ptrX - 1 >= 0) {
				directionAvailable[ptr++] = 3;
				if (last == 3) lastAvailable = true;
			}
				
			if (longerCorridors && lastAvailable) {
				selection = Util.RandomInt(ptr + 1);
				if (selection == 0) selection = last;
				else selection = directionAvailable[selection - 1];
			}
			else selection = directionAvailable[Util.RandomInt(ptr)];
			
			switch (selection){
				case 0:
					ptrY += 1; 
					break;
				case 1:
					ptrY -= 1; 
					break;
				case 2:
					ptrX += 1; 
					break;
				case 3:
					ptrX -= 1; 
					break;
			}
			last = selection;
			--steps;
		}
		/* if (_randomwalk_saveImage) {
			var now = Date.now();
			var name = Convert.string(now.getMonth())+Convert.string(now.getDate())+Convert.string(now.getHours())+Convert.string(now.getMinutes())+Convert.string(now.getSeconds()) + " " + "initv-" + initialValue+"ptrx-" + _ptrX + "ptry-" + _ptrY + "steps-" + _steps + "incr-" + increment + "lc-" + longerCorridors;
			FileManager.setOutput(FileManager.execPath + imageFolder +"Random_Walk\\"+name+".png");
			FileManager.output = a.encode(a.rect, new PNGEncoderOptions(false));
			FileManager.closeHandles();
		} */
		return sampler;
	}
	

	private static bool _diamondsquare_saveImage = true;
	private static void _diamondsquare_diamond(ArrayTexture2D sampler, int centerX, int centerY, int width, float randomness) {
		sampler.Set(centerX, centerY, (sampler.Get(centerX - width, centerY - width) + sampler.Get(centerX - width, centerY + width) + sampler.Get(centerX + width, centerY - width) + sampler.Get(centerX + width, centerY + width)) / 4 + randomness);
	}
	private static void _diamondsquare_square(ArrayTexture2D sampler, int centerX, int centerY, int width, float randomness) {
		sampler.Set(centerX, centerY, (sampler.GetAlternative(centerX - width, centerY) + sampler.GetAlternative(centerX, centerY - width) + sampler.GetAlternative(centerX + width, centerY) + sampler.GetAlternative(centerX, centerY + width)) / 4 + randomness);
	}
	public static ArrayTexture2D generate_DiamondSquare(int size, float initialMin, float initialMax, float randomMin, float randomMax, float randomFallOff) {
		size = Util.ClosestPow2(size - 1) + 1;
		ArrayTexture2D sampler       = new ArrayTexture2D(size, size, 0);
		sampler.Data[0, 0]           = Util.RandomFloat(initialMin, initialMax);
		sampler.Data[0, size-1]      = Util.RandomFloat(initialMin, initialMax);
		sampler.Data[size-1, 0]      = Util.RandomFloat(initialMin, initialMax);
		sampler.Data[size-1, size-1] = Util.RandomFloat(initialMin, initialMax);
		
		// var a;
		/* if (_diamondsquare_saveImage){
			a = new BitmapData(size, size, true);
			a.setPixel(0, 0, Convert.colorFloat(sampler.data[0, 0], sampler.data[0, 0], sampler.data[0, 0]));
			a.setPixel(0, size-1, Convert.colorFloat(sampler.data[0, size-1], sampler.data[0, size-1], sampler.data[0, size-1]));
			a.setPixel(size-1, 0, Convert.colorFloat(sampler.data[size-1, 0], sampler.data[size-1, 0], sampler.data[size-1, 0]));
			a.setPixel(size-1, size-1, Convert.colorFloat(sampler.data[size-1, size-1], sampler.data[size-1, size-1], sampler.data[size-1, size-1]));
		} */
		
		int _size = 2;
		while (_size < size) {
			int step;
			int width;
			//diamond
			step = (size-1) / (_size-1);
			width = step / 2;
			for (int i = width, bound = size-1 - width; i <= bound; i += step){
				for (int j = width; j <= bound; j += step){
					_diamondsquare_diamond(sampler, i, j, width, Util.RandomFloat(randomMin, randomMax));
					// if (_diamondsquare_saveImage) a.setPixel(i, j, Convert.colorFloat(sampler.data[i, j], sampler.data[i, j], sampler.data[i, j]));
				}
			}
			//square
			step = (size-1) / (_size-1);
			width = step / 2;
			for (int i = 0, k = 1; i <= size-1; i += width, ++k){
				for (int j = (k & 1) * width, bound = size-1 - j; j <= bound; j += step){
					_diamondsquare_square(sampler, i, j, width, Util.RandomFloat(randomMin, randomMax));
					// if (_diamondsquare_saveImage) a.setPixel(i, j, Convert.colorFloat(sampler.data[i, j], sampler.data[i, j], sampler.data[i, j]));
				}
			}
			_size = _size * 2 - 1;
			randomMin *= randomFallOff;
			randomMax *= randomFallOff;
		}
		/* if (_diamondsquare_saveImage) {
			var now = Date.now();
			var name = Convert.string(now.getMonth())+Convert.string(now.getDate())+Convert.string(now.getHours())+Convert.string(now.getMinutes())+Convert.string(now.getSeconds()) + " " + "ivtl-" + Math.round(sampler.data[0, 0] * 100) * 0.01 + "ivtr-" + Math.round(sampler.data[0, size-1] * 100) * 0.01 + "ivbl-" + Math.round(sampler.data[size-1, 0] * 100) * 0.01 + "ivbr-" + Math.round(sampler.data[size-1, size-1] * 100) * 0.01 + "imi-" + Math.round(initialMin * 100) * 0.01 + "ima-" + Math.round(initialMax * 100) * 0.01 + "rmi-" + Math.round(randomMin * 100) * 0.01 + "rma-" + Math.round(randomMax * 100) * 0.01 + "rf-" + Math.round(randomFallOff * 100) * 0.01;
			FileManager.setOutput(FileManager.execPath + imageFolder +"Diamond_Square\\"+name+".png");
			FileManager.output = a.encode(a.rect, new PNGEncoderOptions(false));
			FileManager.closeHandles();
		} */
		return sampler;
	}
	
	
	private static bool _perlinnoise_saveImage = true;
	public static ArrayTexture2D generate_PerlinNoise(int size, int layers, float amp, float ampFallOff) {
		size = Util.ClosestPow2(size);
		ArrayTexture2D sampler = new ArrayTexture2D(size, size, 0.5f);
		
		amp /= 2;
		while (layers > 0) {
			//generate temporary sampler
			//noise generation rand(-amp, amp)
			//use linear sampling
			// TODO
			if (size == 1) break;
			size = (int)Math.Round((float)size / 2);
			amp *= ampFallOff;
			layers--;
		}
		
		
		//add parameters into file name
		
		//test linear sampling using bigger bitmap size
		
		/* if (_perlinnoise_saveImage) {
			size *= 2; /////////////
			var a = new BitmapData(size, size, true);
			var i, j;
			i = 0; while (i < size) {
				j = 0; while (j < size) {
					var d = sampler.texture2DLinear(new Vec2DConst(i / size, j / size));
					a.setPixel(i, j, Convert.colorFloat(d, d, d));
					j++;
				}
				i++;
			}
			var now = Date.now();
			var name = Convert.string(now.getMonth())+Convert.string(now.getDate())+Convert.string(now.getHours())+Convert.string(now.getMinutes())+Convert.string(now.getSeconds());
			FileManager.setOutput(FileManager.execPath + imageFolder +"Perlin_Noise\\"+name+".png");
			FileManager.output = a.encode(a.rect, new PNGEncoderOptions(false));
			FileManager.closeHandles();
		} */
		return sampler;
	}
	

	public static ArrayTexture2D generate_RandomFill(int width, int height, float wallProbability)
	{
		ArrayTexture2D sampler = new ArrayTexture2D(width, height, 0.0f);
		
		// int mapMiddle = 0;
		
		for (int row = 0; row < width; row++) {
			for (int column = 0; column < height; column++) {
				// If coordinants lie on the the edge of the map (creates a border)
				if(column == 0) sampler.Data[row, column] = 1.0f;
				else if (row == 0) sampler.Data[row, column] = 1.0f;
				else if (column == width-1) sampler.Data[row, column] = 1.0f;
				else if (row == width-1) sampler.Data[row, column] = 1.0f;

				// Else, fill with a wall a random percent of the time
				else {
					// mapMiddle = (height / 2);

					// if(row == mapMiddle) sampler.Data[row, column] = 0;
					// else 
						sampler.Data[row, column] = Util.RandomFloat(0.0f, 1.0f) < wallProbability ? 1.0f : 0.0f;
				}
			}
		}
		
		return sampler;
	}
	
	public static ArrayTexture2D generate_CaveCA(int width, int height, float initialWallProbability, int iterations)
	{
		ArrayTexture2D sampler1 = generate_RandomFill(width, height, initialWallProbability);
		ArrayTexture2D sampler2 = new ArrayTexture2D(width, height, 0.0f);
		ArrayTexture2D src = sampler1;
		ArrayTexture2D dest = sampler2;
		
		for (int i = 0; i < iterations; ++i) {
			for(int x = 0; x <= width-1; ++x) {
				for(int y = 0; y <= height-1; ++y) {
					dest.Data[x, y] = _caveCA_placeWallLogic(src, width, height, x, y) ? 1.0f : 0.0f;
				}
			}
			ArrayTexture2D temp = src;
			src = dest;
			dest = temp;
		}
		
		return src;
	}

	private static bool _caveCA_placeWallLogic(ArrayTexture2D sampler, int width, int height, int x, int y)
	{
		int scopeX = 1, scopeY = 1;
		
		int startX = x - scopeX;
		int startY = y - scopeY;
		int endX = x + scopeX;
		int endY = y + scopeY;

		int iX = startX;
		int iY = startY;

		int numWalls = 0;

		for (iY = startY; iY <= endY; iY++) {
			for (iX = startX; iX <= endX; iX++) {
				if (!(iX==x && iY==y)) {
					if (iX < 0 || iY < 0 || iX >= width || iY >= height || sampler.Data[iX, iY] == 1.0f) {
						numWalls += 1;
					}
				}
			}
		}

		if(sampler.Data[x,y] == 1) {
			if (numWalls >= 4) return true;
			if (numWalls < 2) return false;
		}
		else {
			if(numWalls>=5) {
				return true;
			}
		}
		return false;
	}

	// public delegate bool ExistsFunction<TData>(TData item);
	
	/* public static void evolveStatic_CaveCA<TData>(HexGrid<TData> grid1, int iterations, int param1, int param2, ExistsFunction<TData> existsFunc) {
		HexGrid<TData> grid2 = new HexGrid<TData>(grid1.FlatTop, 1.0f, false, 0, null, null);
		
		for (int i = 0; i < iterations; ++i) {
			int target = i & 1;
			HexGrid<TData> from;
			HexGrid<TData> to;
			if (target == 0) {
				from = grid1;
				to = grid2;
			}
			else {
				from = grid2;
				to = grid1;
			}
			foreach (CubicHexCoord hex in from.HexCoords()){
				int n = 0;
				// TODO after edge functions are added, implement this.
				[>var edges<GridIndex> = from.getEdges(from.gridIndex(ind));
				for (edge in edges) {
					if (from.get(edge) != null) n++;
				}
				if (from.data[ind.x, ind.y]) {
					if (n < param1) to.data[ind.x, ind.y] = null;
					else to.data[ind.x, ind.y] = true;
				}
				else {
					if (n < param2) to.data[ind.x, ind.y] = null;
					else to.data[ind.x, ind.y] = true;
				}<]
			}
		}
		
		// if (target == 0) grid1 = grid2;
	} */
	
}
