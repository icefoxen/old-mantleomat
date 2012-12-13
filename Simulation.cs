using System;
using System.Drawing;

namespace Mantleomat {
	public class Simulation {
		double[,] NextStep;
		double[,] LastStep;
		long Width;
		long Height;
		
		double BottomTemp;
		double TopTemp;
		
		double Time;
		public Simulation(long width, long height, double bottomTemp, double topTemp) {
			NextStep = new double[width, height];
			LastStep = new double[width, height];
			Width = width;
			Height = height;
			InitArrays(0);
			
			BottomTemp = bottomTemp;
			TopTemp = topTemp;
			
			Time = 0;
		}
		
		// We really don't have memset()???  Or some way to initialize an array...
		// It's probably hiding somewhere.
		private void InitArrays(double val) {
			for(int i = 0; i < Width; i++) {
				for(int j = 0; j < Height; j++) {
					NextStep[i, j] = val;
					LastStep[i, j] = val;
				}
			}
		}
		
		// This calculates what the temperature/properties/whatever of a point should be, 
		// based on the LastStep state.
		// It wraps around in the X direction; if you go left of point 0 you end up at point
		// Width-1.
		public double CalcPoint(long x, long y, double stepSize) {
			
			double val = 0;
			long posLeft, posRight, posUp, posDown;
			
			if(x == 0) {
				posLeft = Width - 1;
				posRight = 1;
			} else if(x == Width - 1) {
				posLeft = x - 1;
				posRight = 0;
			} else {
				posLeft = x - 1;
				posRight = x + 1;
			}
			
			if(y == 0) {
				return BottomTemp;
			} else if(y == Height - 1) {
				return TopTemp;
			}
			posUp = y + 1;
			posDown = y - 1;
			
			val += LastStep[posLeft, posDown];
			val += LastStep[posLeft, y];
			val += LastStep[posLeft, posUp];
			val += LastStep[x, posDown];
			val += LastStep[x, y];
			val += LastStep[x, posUp];
			val += LastStep[posRight, posDown];
			val += LastStep[posRight, y];
			val += LastStep[posRight, posUp];
			
			val /= 9;
			
			
			return val;
		}
		
		public void Step(double stepSize) {
			for(int i = 0; i < Width; i++) {
				for(int j = 0; j < Height; j++) {
					NextStep[i, j] = CalcPoint(i, j, stepSize);
				}
			}
			// Flip ze arrays
			double[,] temp;
			temp = LastStep;
			LastStep = NextStep;
			NextStep = temp;
			
			Time += stepSize;
		}
		
		public double[,] RunSteps(long count, double stepSize) {
			for(double i = 0; i < count; i += stepSize) {
				Step(stepSize);
			}
			return LastStep;
		}
		
		public void WriteToFile(string filename) {
			Bitmap b = new Bitmap((Int32)Width, (Int32)Height);
			double maxTemp = Math.Max(BottomTemp, TopTemp);
			for(int i = 0; i < Width; i++) {
				for(int j = 0; j < Height; j++) {
					double normalizedTemp = LastStep[i,j] / maxTemp;
					int color = (int) (normalizedTemp * 255);
					//Console.WriteLine(color);
					Color c = Color.FromArgb(color, color, color);
					//Color c = System.Drawing.Color.Tomato;
					
					b.SetPixel(i, j, c);
				}
			}
			b.Save(filename);
		}
	}
}

