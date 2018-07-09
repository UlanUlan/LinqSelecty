using System;
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
            Exmpl05();
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

