using CsvHelper;
using System.Diagnostics;
using System.Globalization;
using System.Management;

namespace ACS_Lab2
{
	public class Benchmark
	{
		private Random _random = new Random();

		private Stopwatch SWAllTime { get; set; } = new Stopwatch();

		public string GetCPUName()
		{
			SelectQuery Sq = new SelectQuery("Win32_Processor");
			ManagementObjectSearcher objOSDetails = new ManagementObjectSearcher(Sq);
			ManagementObjectCollection osDetailsCollection = objOSDetails.Get();

			string nameProc = "";

			foreach (ManagementObject mo in osDetailsCollection)
			{
				nameProc = (string)mo["Name"];
			}

			return nameProc.Trim();
		}

		public void SaveToCSV(List<Info> list)
		{
			using (var writer = new StreamWriter("file.csv"))
			{
				using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
				{
					csv.WriteRecords(list);
				}
			}
		}

		public void IntMulPow(int[] arr1, int[] arr2, int count)
		{
			var length1 = arr1.Length;
			var length2 = arr2.Length;

			for (int i = 0; i < length1; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					Math.Pow(arr1[i] * arr2[j], 2);
				}
			}	
		}

		public void DoubleMulSin(double[] arr1, double[] arr2, int count)
		{
			var length1 = arr1.Length;
			var length2 = arr2.Length;

			for (int i = 0; i < length1; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					Math.Sin(Math.Pow(arr1[i] * arr2[j], 2));
				}
			}
		}

		public void DoubleMulCos(double[] arr1, double[] arr2, int count)
		{
			var length1 = arr1.Length;
			var length2 = arr2.Length;

			for (int i = 0; i < length1; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					Math.Cos(Math.Pow(arr1[i] * arr2[j], 2));
				}
			}
		}

		public void Run()
		{
			SWAllTime.Start();

			#region Vars
			var cpuName = GetCPUName();

			var listInfo = new List<Info>();
			const int SIZE_ARR = 2000;
			const int LAUNCH_COUNT = 10;
			string opt = "None";

			var sw = new Stopwatch();
			double sum_time = 0;
			double dispersion = 0;
			double summand1 = 0;
			double summand2 = 0;
			var time = new double[LAUNCH_COUNT];
			#endregion

			#region IntMulPow
			var arr1 = new int[SIZE_ARR];
			var arr2 = new int[SIZE_ARR];

			for (int i = 0; i < SIZE_ARR; ++i)
			{
				arr1[i] = _random.Next(0, 3000);
				arr2[i] = _random.Next(0, 3000);
			}

			Info info = null;
			double aver = 0;
			double absver = 0;
			for (int i = 0; i < LAUNCH_COUNT; ++i)
			{
				sw.Start();

				IntMulPow(arr1, arr2, LAUNCH_COUNT);

				sw.Stop();

				time[i] = sw.ElapsedMilliseconds;
				sum_time += time[i];
				summand1 += time[i] * time[i];
				summand2 += time[i];

				aver = sum_time / LAUNCH_COUNT;
				absver += sw.ElapsedMilliseconds;
				absver = absver / (i + 1);

				info = new Info(cpuName, "int", opt, LAUNCH_COUNT, absver, sw.ElapsedMilliseconds, dispersion, absver, LAUNCH_COUNT / absver, nameof(IntMulPow));
				listInfo.Add(info);
			}

			absver /= LAUNCH_COUNT;

			SaveToCSV(listInfo);
			sw.Reset();
			#endregion

			#region DoubleMulSin
			var arr3 = new double[SIZE_ARR];
			var arr4 = new double[SIZE_ARR];

			for (int i = 0; i < SIZE_ARR; ++i)
			{
				arr3[i] = _random.Next(0, 3000);
				arr4[i] = _random.Next(0, 3000);
			}

			info = null;
			aver = 0;
			absver = 0;
			for (int i = 0; i < LAUNCH_COUNT; ++i)
			{
				sw.Start();

				DoubleMulSin(arr3, arr4, LAUNCH_COUNT);

				sw.Stop();

				time[i] = sw.ElapsedMilliseconds;
				sum_time += time[i];
				summand1 += time[i] * time[i];
				summand2 += time[i];

				aver = sum_time / LAUNCH_COUNT;
				for (int j = 0; j < LAUNCH_COUNT; ++j)
				{
					absver += Math.Abs(aver - time[j]);
				}
				absver /= LAUNCH_COUNT;

				info = new Info(cpuName, "double", opt, LAUNCH_COUNT, aver, time[0], dispersion, LAUNCH_COUNT / aver, absver, nameof(DoubleMulSin));
				listInfo.Add(info);
			}

			SaveToCSV(listInfo);
			sw.Reset();
			#endregion

			#region DoubleMulCos
			var arr5 = new double[SIZE_ARR];
			var arr6 = new double[SIZE_ARR];

			for (int i = 0; i < SIZE_ARR; ++i)
			{
				arr5[i] = _random.Next(0, 3000);
				arr6[i] = _random.Next(0, 3000);
			}

			info = null;
			aver = 0;
			absver = 0;
			for (int i = 0; i < LAUNCH_COUNT; ++i)
			{
				sw.Start();

				DoubleMulCos(arr5, arr6, SIZE_ARR);

				sw.Stop();

				time[i] = sw.ElapsedMilliseconds;
				sum_time += time[i];
				summand1 += time[i] * time[i];
				summand2 += time[i];

				aver = sum_time / LAUNCH_COUNT;
				for (int j = 0; j < LAUNCH_COUNT; ++j)
				{
					absver += Math.Abs(aver - time[j]);
				}
				absver /= LAUNCH_COUNT;

				//TODO: Сделать AbsError - Absolute error
				//TODO: Сделать RelError - Relative error
				info = new Info(cpuName, "double", opt, LAUNCH_COUNT, aver, time[0], dispersion, LAUNCH_COUNT / aver, absver, nameof(DoubleMulCos));
				listInfo.Add(info);
			}

			SaveToCSV(listInfo);
			sw.Reset();
			#endregion

			SWAllTime.Stop();
			Console.WriteLine($"All time: {SWAllTime.ElapsedMilliseconds}");
		}
	}
}
