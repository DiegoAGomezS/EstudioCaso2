using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EstudioCaso2
{
    internal class Program
    {
        static Queue<string> colaSolicitudes = new Queue<string>();
        static void InscribirAsignaturas()
        {
            Console.WriteLine("Ingrese el nombre de la asignatura a inscribir (o 'salir' para terminar):");

            while (true)
            {
                string asignatura = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(asignatura))
                {
                    Console.WriteLine("Ingrese un nombre válido o 'salir' para terminar:");
                    continue;
                }

                if (asignatura.ToLower() == "salir")
                    break;

                colaSolicitudes.Enqueue(asignatura);
                Console.WriteLine($" '{asignatura}' agregada correctamente. Ingrese otra asignatura o 'salir' para terminar:");
            }

            Console.WriteLine("\n--- Asignaturas inscritas ---");
            foreach (var materia in colaSolicitudes)
            {
                Console.WriteLine($"- {materia}");
            }
        }
    }
}
