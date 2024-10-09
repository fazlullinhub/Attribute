using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace ConsoleApp38
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IniValueAttribute : Attribute
    {
        public string FileName { get; }



        public IniValueAttribute(string fileName)
        {
            FileName = fileName;
        }
    }

    public class IniReader
    {
        public static string GetValue(string filePath, string section, string key)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);
            return data[section][key];
        }
    }

    public class Configuration
    {
        [IniValue("config1.ini")]
        public string Setting1 { get; set; }

        [IniValue("config2.ini")]
        public string Setting2 { get; set; }

        [IniValue("config3.ini")]
        public string Setting3 { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var config = new Configuration();
            LoadSettings(config);

            Console.WriteLine($"Setting1: {config.Setting1}");
            Console.WriteLine($"Setting2: {config.Setting2}");
            Console.WriteLine($"Setting3: {config.Setting3}");
        }

        static void LoadSettings(Configuration config)
        {
            var properties = config.GetType().GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<IniValueAttribute>();
                if (attribute != null)
                {
                    string filePath = attribute.FileName;
                    string section = "Settings"; // Убедитесь, что у вас есть такая секция в ini-файле
                    string key = property.Name;

                    try
                    {
                        var value = IniReader.GetValue(filePath, section, key);
                        property.SetValue(config, value);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading value for {key} from {filePath}: {ex.Message}");
                    }
                }
            }
        }
    }
}
