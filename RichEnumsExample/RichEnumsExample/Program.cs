using System;

namespace RichEnumsExample
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            double earthWeight = 85;
            double mass = earthWeight / Planet.Earth.SurfaceGravity();

            foreach (var p in Planet.GetAll())
            {
                Console.WriteLine("Weight on {0} is {1}",
                             p, p.SurfaceWeight(mass));
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
