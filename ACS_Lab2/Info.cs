namespace ACS_Lab2
{
	public class Info
	{
		public Info(string cPUName, string type, string opt, int launchCount, double averageTime, double time, double dispersion, double absver, int matrixSize)
		{
			CPUName = cPUName ?? throw new ArgumentNullException(nameof(cPUName));
			Type = type ?? throw new ArgumentNullException(nameof(type));
			Opt = opt ?? throw new ArgumentNullException(nameof(opt));
			LNum = launchCount;
			AverageTime = averageTime;
			Time = time;
			Dispersion = dispersion;
			AbsErr = absver;
			MatrixSize = matrixSize;

			Task = "Matrix";
			Timer = "StopWatch";
		}

		public string CPUName { get; set; }
		public string Task { get; }
		public string Type { get; set; }
		public string Opt { get; set; }
		public int InsCount => (LNum * MatrixSize * MatrixSize);
		public string Timer { get; }
		public double Time { get; set; }
		public int LNum { get; set; }
		public double AverageTime { get; set; }
		public double AbsErr { get; set; }
		public double RelErr => (AbsErr / AverageTime) * 10;

		private double Dispersion { get; set; }
		private int MatrixSize { get; set; }
	}
}
