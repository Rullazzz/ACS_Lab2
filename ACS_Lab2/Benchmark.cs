using CsvHelper;
using System.Diagnostics;
using System.Globalization;
using System.Management;

namespace ACS_Lab2
{
	public class Benchmark
	{
		private Random _random = new Random();

		public Stopwatch SWAllTime { get; private set; } = new Stopwatch();

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
				using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
				{
					csv.WriteRecords(list);
				}
			}
		}

		public void int_matrix_mul(int[,] matrix_A, int[,] matrix_B, int[,] matrix_RESULT, int MATRIX_SIZE)
		{
			for (int i = 0; i < MATRIX_SIZE; ++i)
			{
				for (int j = 0; j < MATRIX_SIZE; ++j)
				{
					for (int k = 0; k < MATRIX_SIZE; ++k)
					{
						matrix_RESULT[i, j] += matrix_A[i, k] * matrix_B[k, j];
					}
				}
			}
		}

		public void Run()
		{
			SWAllTime.Start();
			var cpuName = GetCPUName();

			var listInfo = new List<Info>();
			const int MATRIX_SIZE = 100;
			const int LAUNCH_COUNT = 200;
			string opt = "None";

			var sw = new Stopwatch();
			double sum_time = 0;
			double dispersion = 0;
			double summand1 = 0;
			double summand2 = 0;
			var time = new double[LAUNCH_COUNT];

			/* ПЕРЕМНОЖЕНИЕ МАТРИЦ (int) */
			int[,] matrix_A_i = new int[MATRIX_SIZE, MATRIX_SIZE];
			var matrix_B_i = new int[MATRIX_SIZE, MATRIX_SIZE];
			var matrix_RESULT_i = new int[MATRIX_SIZE, MATRIX_SIZE];

			for (int i = 0; i < MATRIX_SIZE; ++i)
			{
				for (int j = 0; j < MATRIX_SIZE; ++j)
				{
					matrix_A_i[i, j] = _random.Next(0, 3000);
					matrix_B_i[i, j] = _random.Next(0, 3000);
					matrix_RESULT_i[i, j] = 0;
				}
			}

			for (int i = 0; i < LAUNCH_COUNT; ++i)
			{
				sw.Start();

				int_matrix_mul(matrix_A_i, matrix_B_i, matrix_RESULT_i, MATRIX_SIZE);

				sw.Stop();

				time[i] = sw.ElapsedMilliseconds;
				sum_time += time[i];
				summand1 += time[i] * time[i];
				summand2 += time[i];
			}

			dispersion = summand1 / LAUNCH_COUNT - summand2 / LAUNCH_COUNT;
			double aver = sum_time / LAUNCH_COUNT;
			double absver = 0;

			for (int i = 0; i < LAUNCH_COUNT; ++i)
			{
				absver += Math.Abs(aver - time[i]);
			}

			absver /= LAUNCH_COUNT;

			var info = new Info(cpuName, "int", opt, LAUNCH_COUNT, aver, time[0], dispersion, absver, MATRIX_SIZE);
			listInfo.Add(info);

			SaveToCSV(listInfo);

			/* ПЕРЕМНОЖЕНИЕ МАТРИЦ (int) */
			matrix_A_i = new int[MATRIX_SIZE, MATRIX_SIZE];
			matrix_B_i = new int[MATRIX_SIZE, MATRIX_SIZE];
			matrix_RESULT_i = new int[MATRIX_SIZE, MATRIX_SIZE];

			for (int i = 0; i < MATRIX_SIZE; ++i)
			{
				for (int j = 0; j < MATRIX_SIZE; ++j)
				{
					matrix_A_i[i, j] = _random.Next(0, 3000);
					matrix_B_i[i, j] = _random.Next(0, 3000);
					matrix_RESULT_i[i, j] = 0;
				}
			}

			for (int i = 0; i < LAUNCH_COUNT; ++i)
			{
				sw.Start();

				int_matrix_mul(matrix_A_i, matrix_B_i, matrix_RESULT_i, MATRIX_SIZE);

				sw.Stop();

				time[i] = sw.ElapsedMilliseconds;
				sum_time += time[i];
				summand1 += time[i] * time[i];
				summand2 += time[i];
			}

			dispersion = summand1 / LAUNCH_COUNT - summand2 / LAUNCH_COUNT;
			aver = sum_time / LAUNCH_COUNT;
			absver = 0;

			for (int i = 0; i < LAUNCH_COUNT; ++i)
			{
				absver += Math.Abs(aver - time[i]);
			}

			absver /= LAUNCH_COUNT;

			info = new Info(cpuName, "int", opt, LAUNCH_COUNT, aver, time[0], dispersion, absver, MATRIX_SIZE);
			listInfo.Add(info);

			SaveToCSV(listInfo);

			SWAllTime.Stop();

			Console.WriteLine($"All time: {SWAllTime.ElapsedMilliseconds}");
		}
	}
}
