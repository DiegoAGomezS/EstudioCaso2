using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudioCaso2
{
    internal class Program
    {
        static Queue<string> colaSolicitudes = new Queue<string>();

        static void Main(string[] args)
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ DEL SERVIDOR UNIVERSITARIO ===");
                Console.WriteLine("1. Inscribir asignaturas (Encolar)");
                Console.WriteLine("2. Ver asignaturas inscritas");
                Console.WriteLine("3. Salir");
                Console.Write("\nSeleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("\n⚠ Opción inválida. Presione una tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        InscribirAsignaturas();
                        break;

                    case 2:
                        MostrarAsignaturas();
                        break;

                    case 3:
                        Console.WriteLine("\n Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("\n Opción no válida. Presione una tecla para continuar...");
                        break;
                }

                Console.ReadKey();

            } while (opcion != 3);
        }

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
        }

        static void MostrarAsignaturas()
        {
            Console.WriteLine("\n--- Asignaturas inscritas ---");

            if (colaSolicitudes.Count == 0)
            {
                Console.WriteLine("No hay asignaturas en la cola.");
                return;
            }

            foreach (var materia in colaSolicitudes)
            {
                Console.WriteLine($"- {materia}");
            }
        }
    }
}
