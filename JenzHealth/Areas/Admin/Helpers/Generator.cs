using JenzHealth.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace JenzHealth.Areas.Admin.Helpers
{
    public class Generator
    {
        public static string GeneratorCode()
        {
            var db = new DatabaseEntities();
            int length = db.ApplicationSettings.FirstOrDefault().CodeGenSeed;
            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            return str_build.ToString();
        }
    }
}