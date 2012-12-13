using System;

namespace Mantleomat {
	class MainClass {
		public static void Main(string[] args) {
			Console.WriteLine("Running...");
			var a = new Simulation(100, 100, 100000, 0);
			a.RunSteps(100000, 10);
			/*
			foreach(var i in b) {
				Console.Out.WriteLine(i);
			}
			*/
			Console.WriteLine("Writing results...");
			a.WriteToFile("test.png");
			Console.WriteLine("Done.");
		}
	}
}

