using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autoRunOptions
{
    class Program
    {
        static void Main(string[] args)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            

        Start:

            String msgCheck = "/check - sprawdzenie programów uruchamianych przy starcie systemu";
            String msgAdd = "/add [nazwa] [ścieżka do pliku] - dodanie pogramu do autostartu";
            String msgDel = "/del [nazwa] - usunięcie programu z autostartu";
            Console.WriteLine("");
            Console.WriteLine("Dostępne polecenie: ");
            Console.WriteLine(msgCheck);
            Console.WriteLine(msgAdd);
            Console.WriteLine(msgDel);
            Console.WriteLine("");
            String a = Console.ReadLine();

            void checkList()
            {
                Console.WriteLine("");
                Console.WriteLine("Ilosc programów: " + rk.GetValueNames().Length);
                Console.WriteLine("");

                var valueNames = rk.GetValueNames();

                Dictionary<string, string> appList = valueNames
                    .Where(valueName => rk.GetValueKind(valueName) == RegistryValueKind.String)
                    .ToDictionary(valueName => valueName, valueName => rk.GetValue(valueName).ToString());
                foreach (String viewAppList in valueNames)
                {
                    Console.WriteLine(viewAppList);
                }
            }

            void addToList()
            {
                try
                {
                    String[] sub = a.Split(' ');
                    if (sub.Length != 3)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Poprawne uzycie: " + msgAdd);
                        Console.WriteLine("");
                    }
                    else
                    {
                        String name = sub[1];
                        String path = sub[2];
                        rk.SetValue(name.ToString(), path.ToString());
                        Console.WriteLine(""); 
                        Console.WriteLine("Pomysłnie dodano: " + path + " do autostartu twojego systemu!");
                        Console.WriteLine("");

                    }
                }catch(Exception e) { Console.WriteLine(e.Message); }
            }
            void removeFromList()
            {
                String[] sub = a.Split(' ');
                if (sub.Length != 2)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Poprawne użycie: " + msgDel);
                    Console.WriteLine("");
                }
                else
                {
                    String name = sub[1];
                    if (rk.GetValue(name) != null)
                    {
                        rk.DeleteValue(name, false);
                        Console.WriteLine("");
                        Console.WriteLine("Pomyślnie usunięto: " + name);
                        Console.WriteLine("");
                    }
                    else
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Nie znaleziono nazwy "+name+" w celu sprawdzenia programów wpisz "+msgCheck);
                        Console.WriteLine("");
                    }
                }
            }
            if (a.Equals("/check"))
            {
                checkList();
                goto Start;
            }
            else if (a.Contains("/add"))
            {
                addToList();
                goto Start;
            }
            else if(a.Contains("/del"))
            {
                removeFromList();
                goto Start;
            }
            Console.WriteLine("Błąd, nie odnaleziono polecenia");
            goto Start;
        }
    }
}
