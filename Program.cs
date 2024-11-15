using System.Data.SqlTypes;
using System.Threading;


class Person
{
    public int numberOfBus;
    public Person(int numberOfBus)
    {
        this.numberOfBus = numberOfBus;
    }   
}
class BusStation
{
    public int numberOfStation;
    public List<Person> persons;
    public BusStation(int num)
    {
        numberOfStation = num;
        persons = new List<Person>();
    }
}
class Bus
{
    public int numberOfBus;
    public int maxCountOfPeoples;
    public int currentCountOfPeoples;

    public Bus(int num, int max, int current)
    {
        numberOfBus = num;
        maxCountOfPeoples = max;
        currentCountOfPeoples = current;
    }
    public void GetPeoplesFromStation(List<Person> peoples)
    {
        if(currentCountOfPeoples ==maxCountOfPeoples)
        {
            Console.WriteLine("Автобус переполнен");
            return;
        }
        foreach(Person person in peoples)
        {
            if (person.numberOfBus == numberOfBus && currentCountOfPeoples<=maxCountOfPeoples)
                currentCountOfPeoples++;
        }
        Console.WriteLine($"Автобус номер {numberOfBus} забрал в себя {currentCountOfPeoples} человек");
    }

}
internal class Program
{
    static Semaphore sema = new Semaphore(2,2);
    static List<Bus> buses = new List<Bus>()
    {
    new Bus(125, 400, new Random().Next(0, 400)),
    new Bus(666, 1000, new Random().Next(0, 1000)),
    new Bus(258, 600, new Random().Next(0, 600)),
    new Bus(543, 120, new Random().Next(0, 120)),
    new Bus(111, 100, new Random().Next(0, 100)),
    new Bus(60, 332, new Random().Next(0, 332)),
    }; 
    static List<BusStation> stations = new List<BusStation>()
    {
        new BusStation(1),
        new BusStation(2),
        new BusStation(3),
    };
    static void Main(string[] args)
    {
        
        while (true)
        {
            for (int i = 0; i < stations.Count; i++)
            {
                int a = i;
                Task.Run(() => SetPeoples(stations[a]));
            }
            Thread.Sleep(200);
            for (int i = 0; i < stations.Count; i++)
            {
                Task.Run(() => ApproachStation(stations[i])).Wait();
            }
        }  
    }
    static void ApproachStation(BusStation station)
    {
        sema.WaitOne();
        List<int> numbers = new List<int>();
        for(int i = 0; i < buses.Count; i++)
        {
            Thread.Sleep(400);
            int tmp = new Random().Next(0,buses.Count);
            while (numbers.Contains(tmp)) tmp = new Random().Next(0, buses.Count);
            numbers.Add(tmp);
            buses[tmp].GetPeoplesFromStation(station.persons);
        }
        sema.Release();
    }
    static void SetPeoples(BusStation station)
    {
        while(true) 
        {
      
            int count = new Random().Next(0, 150);
            for (int i = 0; i < count; i++)
            {
                station.persons.Add(new Person(buses[new Random().Next(0, buses.Count)].numberOfBus));
            }
            Console.WriteLine($"Люди пришли на станцию {station.numberOfStation} в количестве {count} человек");
            Thread.Sleep(new Random().Next(100, 600));
        }
    }

}