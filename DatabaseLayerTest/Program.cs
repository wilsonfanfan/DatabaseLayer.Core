using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DatabaseLayer;
using DatabaseLayer.Entity;
using Entity;

namespace DatabaseLayerTest
{

    class Program
    {

        static Program() => new EntityManager();
        static void Main(string[] args)
        {

            //View document:https://github.com/wilsonfanfan/Elfin/blob/master/ElfinTutorial.docx
            var logs = new r_LogManager().GetEntityObjects();

            var log = (r_Log)logs[1];
            try
            {
                var dt2 = Query.ExecuteSQLQuery($"select * from {nameof(r_Log)}", "Db2");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
