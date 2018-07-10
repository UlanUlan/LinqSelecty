using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        private static crcms db = new crcms();
        static void Main(string[] args)
        {
            Exmpl07();
        }

        //Фильтрация
        //Where
        //Take
        //Skip
        //TakeWhile
        //SkipWhile
        //Distinct
        static void Exmlp01()
        {
            var q1 = db.Area.Where(w => w.ParentId == 0).ToList();
            //  PrintInfo(q1);

            var qq1 = (from a in db.Area
                       where a.ParentId == 0
                       && !string.IsNullOrEmpty(a.IP)
                       select a).ToList();
            //   PrintInfo(qq1);

            //TAke
            //возвращает первые n элементов и игнорирует остальные
            var q2 = db.Area.Take(5);
            //   PrintInfo(q2);

            var q3 = db.Area.ToList().Skip(3);
            //  PrintInfo(q3.ToList());

            var a4 = db.Timer.Where(w => w.DateFinish != null).ToList().Skip(10).Take(10).ToList();

            //TakeWhile
            var q5 = db.Timer.ToList().TakeWhile(s => s.DateFinish != null).ToList();

            //PrintInfo(q5);

            //SkipWhile
            var q6 = db.Timer.ToList().SkipWhile(w => w.DateFinish != null).ToList();
            // PrintInfo(q6);

            //Distinct
            var q7 = db.Area.Select(s => new { s.IP }).Distinct();
            Console.WriteLine("Distinct: " + q7.Count());

            var q7_1 = db.Area.Select(s => new { s.IP });
            Console.WriteLine("Clear: " + q7_1.Count());


        }

        //Проецирование
        //Select
        //SelectMany

        public static void Exmpl02()
        {
            DirectoryInfo[] dirs = new DirectoryInfo(@"\\dc\Студенты\ПКО").GetDirectories();

            var q1 = from d in dirs
                     where (d.Attributes & FileAttributes.System) == 0
                     select new
                     {
                         DirectoryName = d.FullName,
                         Created = d.CreationTime,
                         Files = from f in d.GetFiles()
                                 select new
                                 {
                                     FileName = f.FullName,
                                     f.Length
                                 }
                     };


            //синт облг воспр
            List<SysFileInfo> q2 = (from d in dirs
                                    select new SysFileInfo
                                    {
                                        Directory = d.FullName,
                                        Created = d.CreationTime,
                                        Files = (from f in d.GetFiles()
                                                 select f.Name).ToList()
                                    }).ToList();


            //LinQ
            List<SysFileInfo> q2_1 = dirs.Select(s => new SysFileInfo()
            {
                Directory = s.FullName,
                Created = s.CreationTime,
                Files = s.GetFiles().Select(f => f.Name).ToList()
            }).ToList();




            foreach (var file in q1)
            {
                Console.WriteLine("{0,-40}\t{1}", file.DirectoryName, file.Created);
                foreach (var f in file.Files)
                {
                    Console.WriteLine("\t--> {0} ({1})", f.FileName, f.Length);
                }
            }
        }

        //Объединение
        //объединяет две входные последовательности в одну выходную
        //Join
        //GroupJoin

        public static void Exmpl03()
        {
            var q1 = db.Timer.Join(
                db.Area,
                t => t.AreaId,
                a => a.AreaId,

                (t, a) => new
                {
                    a.FullName,
                    t.DateStart,
                    t.DateFinish,
                    t.TimerId
                });

            foreach (var q in q1)
            {
                Console.WriteLine("{0}. {1,-40} {2}:{3}", q.TimerId, q.FullName, q.DateStart, q.DateFinish);
            }
        }

        //Упорядочевание
        //OrderBy
        //ThenBy
        //OrderByDesc
        //ThenByDesc
        //Revers

        public static void Exmpl04()
        {

            //A-Z
            var q1 = db.Document.OrderBy(o => o.DocumentCreateDate);
            q1 = q1.ThenBy(t => t.CreatedBy);

            var q2 = db.Document.OrderByDescending(o => o.DocumentCreateDate);

            var q3 = db.Area.Reverse();
            PrintInfo(q1);
        }

        //Группирование
        //Group by

        static void Exmpl05()
        {
            string[] files = Directory.GetFiles(@"\\dc\Студенты\ПКО\PDD 171\Фотография\Ли Анастасия\стекло вода");

            //IEnumerable<IGrouping<string, string>> q = files.GroupBy(f => Path.GetExtension(f));

            var q =
                files.GroupBy(f => new
                {
                    Extension = Path.GetExtension(f)
                });

            foreach (var item in q)
            {
                Console.WriteLine(item.Key);
            }
        }

        //Методы преобразования
        //ofType
        //Cast
        //ToArray
        //ToList
        //ToDictionary
        //ToLookup
        //AsEnumerable
        //AsQueryable

        public void Exmpl06()
        {
            ArrayList classList = new ArrayList();
            classList.AddRange(new int[] { 3, 4, 5 });
            classList.AddRange(new string[] { "3", "4", "5" });

            IEnumerable<int> seq = classList.Cast<int>();

            IEnumerable<int> seq2 = classList.Cast<int>();

            Dictionary<string, string> q2 = db.Area.ToDictionary(a => a.IP, a => a.Name);

            Dictionary<string, string> q3 = new Dictionary<string, string>();

            foreach (Area item in db.Area)
            {
                q3.Add(item.IP, item.Name);
            }
        }

        //Поэлементные операции
        //First
        //FirstOdDefault
        //Last
        //LastOrDefault
        //Singl
        //SinglORDeafult
        //ElementAt
        //ElemnentAtOrDeafult
        //DefaultIfEmpty

        public static void Exmpl07()
        {
            //var q1 = db.Area.First(f=>!string.IsNullOrEmpty(f.IP));
            var q3 = db.Area.FirstOrDefault(f => f.IP == "000");

            //var q4 = db.Area.ElementAt(3); //db.Area[3];

            List<int?> obj = new List<int?>() { 1, 2, null };

            var q4 = obj.ElementAtOrDefault(2);
        }

        //Методы агерегирования
        //Count
        //longCount
        //Max, MIX
        //SUM, AVearge

        //Квантификаторы
        //Contains
        //Any
        //all
        //Sequence  -- последолвательности
        //Equal

        public static void Exmpl08()
        {
            int[] zone = new[] { 2, 3, 4, 5, 6, 7 };
            var q1 = db.Area.Where(w => zone.Contains(w.AreaId));

            bool q2 = db.Area.Select(s => s.AreaId).Contains(3);

            bool q3 = db.Document.Any(d => d.CreatedBy == "Gertsen");

            bool q4 = db.Timer.All(a => a.DateFinish != null);

            //Enumerable.Empty<string>();  --  пустая последвоательность

            foreach (int month in Enumerable.Range(1,12))   //List<int> i = Enumerable.Range(1,12).ToList();
            {
                Console.WriteLine(month);
            }
        }

        static void PrintInfo(List<Area> areas)
        {
            foreach (Area area in areas)
            {
                Console.WriteLine("{0,-60}\t{1}", area.Name, area.IP);
            }
        }
        static void PrintInfo(IQueryable<Area> areas)
        {
            foreach (Area area in areas)
            {
                Console.WriteLine("{0,-60}\t{1}", area.Name, area.IP);
            }
        }

        static void PrintInfo(List<Timer> areas)
        {
            foreach (Timer area in areas)
            {
                Console.WriteLine("{0,-60}{1,-20}{2}", area.DocumentId, area.DateStart, area.DateFinish);
            }
        }

        static void PrintInfo(IQueryable<Document> areas)
        {
            foreach (Document area in areas)
            {
                Console.WriteLine("{0,-20}\t{1}:{2}", area.DocumentId, area.DocumentCreateDate, area.CreatedBy);
            }
            Console.WriteLine("-------------------------------------------------------");
        }

    }
    public class SysFileInfo
    {
        public string Directory { get; set; }
        public DateTime Created { get; set; }
        public List<string> Files { get; set; }
        public int Size { get; set; }
    }

}

