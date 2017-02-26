using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CZWA.Common;
using CZWA.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
//using CZWA.DB;

namespace CZWA.DB_Migration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            ConsoleKeyInfo cki;

            _outputMenue();

            do
            {
                cki = Console.ReadKey();

                switch (cki.KeyChar)
                {
                    case '1':
                        Console.WriteLine();
                        Console.WriteLine("Versuche Database zu löschen und neu zu erzeugen, einen Moment bitte...");
                        _create(true, true);
                        _outputMenue();
                        break;
                    case '2':
                        Console.WriteLine();
                        Console.WriteLine("Versuche Database zu löschen, einen Moment bitte...");
                        _create(true, false);
                        _outputMenue();
                        break;
                    case '3':
                        Console.WriteLine();
                        Console.WriteLine("Versuche Database zu erzeugen, einen Moment bitte...");
                        _create(false, true);
                        _outputMenue();
                        break;
                }

            } while (cki.Key != ConsoleKey.Escape);


            Console.WriteLine("Rdy");
            Console.ReadLine();
        }

        internal static void _outputMenue()
        {
            Console.WriteLine();
            Console.WriteLine("1 - Delete/Create");
            Console.WriteLine("2 - Delete");
            Console.WriteLine("3 - Create");
            Console.WriteLine("Esc - Exit");
        }


        internal static void _create(bool del, bool create)
        {
            var optionsbuilder = new DbContextOptionsBuilder<DataContext>();
            optionsbuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Core-Test-V1;Trusted_Connection=True;MultipleActiveResultSets=true");

            using (var context = new DataContext(optionsbuilder.Options, null))
            {
                if (del)
                {
                    if (context.Database.EnsureDeleted())
                    {
                        Console.WriteLine("Database gelöscht...");
                    }
                    else
                    {
                        Console.WriteLine("Keine Database gefunden...");
                    }
                }

                if (create)
                {
                    if (context.Database.EnsureCreated())
                    {
                        Console.WriteLine("Database erzeugt...");
                    }
                    else
                    {
                        Console.WriteLine("Database existiert bereits...");
                        return;

                    }
                    Console.WriteLine("Erzeuge Daten...");

                    Createer_Roles.Create(context);

                    Creater_Users.Create(context,
                        new UserRoleType[] { UserRoleType.Admin, UserRoleType.Default },
                        new User
                        {
                            Name = "Riesner",
                            Vorname = "Rene",
                            Username = "rr1980",
                            Password = "12003"
                        });

                    Creater_Users.Create(context,
                        new UserRoleType[] { UserRoleType.Default },
                        new User
                        {
                            Name = "Riesner",
                            Vorname = "Sven",
                            Username = "Oxi",
                            Password = "12003"
                        });
                }
            }
        }
    }
}
