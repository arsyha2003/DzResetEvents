using System.Threading;


internal class Program
{
    static string paresPath = "pares.txt";
    static string sumPath = "sum.txt";
    static string proizvPath = "proizv.txt";
    static AutoResetEvent are = new AutoResetEvent(false);
    static void Main(string[] args)
    {
        Task.Run(() => GenerateNumbers());
        Task.Run(() => CountSum());
        Task.Run(() => CountProizv());
        Console.ReadKey();  
    }
    static void GenerateNumbers()
    {
        string pare = string.Empty;
        if(File.Exists(paresPath)) File.Delete(paresPath);
        Thread.Sleep(500);
        using(FileStream fs = new FileStream(paresPath, FileMode.CreateNew, FileAccess.Write))
        {
            using(StreamWriter sw = new StreamWriter(fs))
            {
                for(int i=0;i<100;i++)
                {
                    pare += new Random().Next(10, 200) + " " + new Random().Next(40, 200);
                    sw.WriteLine(pare);
                    pare = string.Empty;
                }
            }
        }
        Console.WriteLine("Генерация закончена");
        are.Set();
    }
    static void CountSum()
    {
        are.WaitOne();
        List<int> pares = new List<int>();
        using (FileStream fs = new FileStream(paresPath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    pares.Add(int.Parse(line.Split()[0]) + int.Parse(line.Split()[1]));
                }
            }
        }
        if (File.Exists(sumPath)) File.Delete(sumPath);
        Thread.Sleep(500);
        using (FileStream fs = new FileStream(sumPath, FileMode.CreateNew, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach(int p in pares)
                {
                    sw.WriteLine(p);
                }
            }
        }
        Console.WriteLine("Сумма подсчитана");
        are.Set();
    }
    static void CountProizv()
    {
        are.WaitOne();
        List<int> pares = new List<int>();
        using (FileStream fs = new FileStream(paresPath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    pares.Add(int.Parse(line.Split()[0]) + int.Parse(line.Split()[1]));
                }
            }
        }
        if (File.Exists(proizvPath)) File.Delete(proizvPath);
        Thread.Sleep(500);
        using (FileStream fs = new FileStream(proizvPath, FileMode.CreateNew, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (int p in pares)
                {
                    sw.WriteLine(p);
                }
            }
        }
        Console.WriteLine("Произведение подсчитано");
        are.Set();
    }

}