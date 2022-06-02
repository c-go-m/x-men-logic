using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Test.CommonTest
{
    public class TestFile
    {
        public static StreamReader GetFile(string nameFile)
        {
            var urlBase = ReplacePath(Directory.GetCurrentDirectory());

            var result = new StreamReader(urlBase + nameFile);

            return result;
        }

        public static List<T> Deserialize<T>(string path, string type)
        {
            try
            {
                StreamReader jsonFile = GetFile(path);
                JObject jsonObject = JObject.Parse(jsonFile.ReadToEnd());
                var list = new List<T>();
                foreach (var item in jsonObject["list"])
                {
                    list.Add(item[type].ToObject<T>());
                }

                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string ReplacePath(string path)
        {
            var result = "";
            var url = path.Split('\\');
            for (int i = 0; i < url.Length - 3; i++)
            {
                result = result + url[i] + "\\";
            }
            return (result + "CommonTest\\Files\\");
        }
    }
}
