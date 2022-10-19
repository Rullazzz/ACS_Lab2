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
			var listInfo = new List<Info>();
			var sw = new Stopwatch();

			const int SIZE_ARR = 2000;
			const int LAUNCH_COUNT = 10;

			string cpuName = GetCPUName();
			string opt = "None";
			double allSumTime = 0;
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
			double averTime = 0;
			double absErr = 0;
			for (int i = 0; i < LAUNCH_COUNT; ++i)
			{
				sw.Start();

				IntMulPow(arr1, arr2, LAUNCH_COUNT);

				sw.Stop();

				allSumTime += sw.ElapsedMilliseconds;
				averTime = allSumTime / (i + 1);
				absErr = Math.Abs(averTime - sw.ElapsedMilliseconds);

				info = new Info(cpuName, "int", opt, SIZE_ARR * SIZE_ARR, averTime / 1000, sw.ElapsedMilliseconds / 1000.0, absErr / 1000, SIZE_ARR * SIZE_ARR / (averTime / 1000), LAUNCH_COUNT, nameof(IntMulPow));
				listInfo.Add(info);
				sw.Reset();
			}

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

			ResetData(out allSumTime, out info, out averTime, out absErr);
			for (int i = 0; i < LAUNCH_COUNT; ++i)
			{
				sw.Start();

				DoubleMulSin(arr3, arr4, LAUNCH_COUNT);

				sw.Stop();

				allSumTime += sw.ElapsedMilliseconds;
				averTime = allSumTime / (i + 1);
				absErr = Math.Abs(averTime - sw.ElapsedMilliseconds);

				info = new Info(cpuName, "double", opt, SIZE_ARR * SIZE_ARR, averTime / 1000, sw.ElapsedMilliseconds / 1000.0, absErr / 1000, SIZE_ARR * SIZE_ARR / (averTime / 1000), LAUNCH_COUNT, nameof(DoubleMulSin));
				listInfo.Add(info);
				sw.Reset();
			}

			SaveToCSV(listInfo);
			sw.Reset();
			#endregion

			#region DoubleMulCos

			ResetData(out allSumTime, out info, out averTime, out absErr);
			for (int i = 0; i < LAUNCH_COUNT; ++i)
			{
				sw.Start();

				DoubleMulCos(arr3, arr4, SIZE_ARR);

				sw.Stop();

				allSumTime += sw.ElapsedMilliseconds;
				averTime = allSumTime / (i + 1);
				absErr = Math.Abs(averTime - sw.ElapsedMilliseconds);

				info = new Info(cpuName, "double", opt, SIZE_ARR * SIZE_ARR, averTime / 1000, sw.ElapsedMilliseconds / 1000.0, absErr / 1000, SIZE_ARR * SIZE_ARR / (averTime / 1000), LAUNCH_COUNT, nameof(DoubleMulCos));
				listInfo.Add(info);
				sw.Reset();
			}

			SaveToCSV(listInfo);
			sw.Reset();
			#endregion

			SWAllTime.Stop();
			Console.WriteLine($"All time: {SWAllTime.ElapsedMilliseconds}");
		}

		private static void ResetData(out double allSumTime, out Info info, out double averTime, out double absErr)
		{
			info = null;
			averTime = 0;
			absErr = 0;
			allSumTime = 0;
		}
	}
}
