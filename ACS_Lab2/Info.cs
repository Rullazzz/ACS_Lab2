namespace ACS_Lab2
{
	public class Info
	{
		public Info(string cPUName, string type, string opt, int launchCount, double averageTime, double time, double absver, double taskperf, int lnum, string task = "none", string timer = "StopWatch")
		{
			CPUName = cPUName ?? throw new ArgumentNullException(nameof(cPUName));
			Type = type ?? throw new ArgumentNullException(nameof(type));
			Opt = opt ?? throw new ArgumentNullException(nameof(opt));
			Task = task ?? throw new ArgumentNullException(nameof(task));
			Timer = timer ?? throw new ArgumentNullException(nameof(timer));
			InsCount = launchCount;
			LNum = lnum;
			AverageTime = averageTime;
			Time = time;
			AbsErr = absver;
			TaskPerf = taskperf;
		}

		public string CPUName { get; set; }
		public string Task { get; set; }
		public string Type { get; set; }
		public string Opt { get; set; }
		public int InsCount { get; set; }
		public string Timer { get; }
		public double Time { get; set; }
		public int LNum { get; set; }
		public double AverageTime { get; set; }
		public double AbsErr { get; set; }
		public double RelErr => (AbsErr / AverageTime) * 100;
		public double TaskPerf { get; set; }
	}
}
