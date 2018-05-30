using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmKSrednich
{ 
        [Serializable]
public class Punkt
{
    public int X1 { get; set; }
    public int X2 { get; set; }

    public int Grupa { get; set; }
}

[Serializable]
public class PunktK
{
    public double X1 { get; set; }
    public double X2 { get; set; }

    public int Grupa { get; set; }
}

    


    class Program
    {
        private static List<PunktK> memeory = new List<PunktK>();

        public static object DeepClone(object obj)
        {
            object objResult = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult;
        }

        public static void ZapamietajOstatniePolozenieKPunkt(List<PunktK> kpunkty)
        {
            memeory = new List<PunktK>();

            memeory = (List<PunktK>)DeepClone(kpunkty);
        }

        public static void ZerowanieGrup(ref List<Punkt> zbior)
        {
            foreach (var punkt2 in zbior)
            {
                punkt2.Grupa = -1;
            }
        }

        public static void NadanieGrupZbiorowi(ref List<Punkt> zbior, List<PunktK> kpunkty)
        {
            for (int i = 0; i < zbior.Count; i++)
            {
                Punkt punkt2 = zbior[i];
                NadanieGrupy(ref punkt2, kpunkty);
            }
        }
        public static double LiczenieOdleglosc(PunktK p1, Punkt p2)
        {
            var x = Math.Pow(p1.X1 - p2.X1, 2);

            var x2 = Math.Pow(p1.X2 - p2.X2, 2);

            double a = Math.Sqrt(x + x2);
            return a;
        }

        public static void NadanieGrupy(ref Punkt punkt, List<PunktK> kpunkty)
        {
            Dictionary<double, PunktK> d = new Dictionary<double, PunktK>();

            foreach (var kpunkt in kpunkty)
            {
                double od = LiczenieOdleglosc(kpunkt, punkt);

                while (d.ContainsKey(od))
                {
                    od = od + 0.000001;
                }

                d.Add(od, kpunkt);
            }

            var g = d.First(k => k.Key == d.Keys.Min()).Value.Grupa;

            punkt.Grupa = g;
        }

        public static void PrzesunieciePunkuK(List<Punkt> zbior, ref List<PunktK> kpunkty)
        {
            foreach (var kpunkt in kpunkty)
            {
                List<Punkt> punkty = new List<Punkt>();

                foreach (var punkt in zbior)
                {
                    if (punkt.Grupa == kpunkt.Grupa)
                    {
                        punkty.Add(punkt);
                    }
                }

                double x1 = 0;
                double x2 = 0;

                if (punkty.Count > 0)
                {
                    x1 = punkty.Average(i => i.X1);
                    x2 = punkty.Average(i => i.X2);

                    kpunkt.X1 = x1;
                    kpunkt.X2 = x2;
                }
            }
        }

        static void Main(string[] args)
        {
            List<PunktK> kpunkty = new List<PunktK>();

            kpunkty.Add(new PunktK() { X1 = 0, X2 = 1, Grupa = 1 });
            kpunkty.Add(new PunktK() { X1 = 2, X2 = -1, Grupa = 2 });
            kpunkty.Add(new PunktK() { X1 = 6, X2 = -2, Grupa = 3 });

            List<Punkt> punkty = new List<Punkt>();

            punkty.Add(new Punkt() { X1 = -7, X2 = 5 });
            punkty.Add(new Punkt() { X1 = -5, X2 = 3 });
            punkty.Add(new Punkt() { X1 = -6, X2 = 1 });
            punkty.Add(new Punkt() { X1 = -3, X2 = -1 });
            punkty.Add(new Punkt() { X1 = -5, X2 = -3 });
            punkty.Add(new Punkt() { X1 = 3, X2 = 7 });
            punkty.Add(new Punkt() { X1 = 2, X2 = 2 });
            punkty.Add(new Punkt() { X1 = 8, X2 = 5 });
            punkty.Add(new Punkt() { X1 = 10, X2 = 4 });

            int counter = 0;

            int amount = KSrednieMetody.AlgoStart(ref punkty, ref kpunkty, 100);

            Console.WriteLine("\nPunkty: \n");

            foreach (var punkt in punkty)
            {
                Console.WriteLine("\tX1: " + punkt.X1 + " X2 " + punkt.X2 + "  Grupa " + punkt.Grupa);
            }

            Console.WriteLine("\nK punkty: \n");

            foreach (var punktK in kpunkty)
            {
                Console.WriteLine("\tX1: " + punktK.X1 + " X2 " + punktK.X2 + "  Grupa " + punktK.Grupa);
            }

            Console.WriteLine("\nLiczba pętli: " + amount);
            Console.ReadKey();
        }
    }
}
