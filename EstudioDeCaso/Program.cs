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
        // Cola principal para almacenar las solicitudes de inscripción de asignaturas.
        // Simula cómo un servidor recibe múltiples solicitudes y las procesa en orden FIFO.
        static Queue<string> colaSolicitudes = new Queue<string>();

        // Cola paralela que almacena las notas en el mismo orden en que se registraron las asignaturas.
        // También utiliza el principio FIFO para mantener la correspondencia entre ambas colas.
        static Queue<double> colaNotas = new Queue<double>();


        static void Main(string[] args)
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ DEL SERVIDOR UNIVERSITARIO ===");
                Console.WriteLine("1. Inscribir asignaturas (Encolar)");
                Console.WriteLine("2. Ver asignaturas inscritas");
                Console.WriteLine("3. Ingresar notas (Encolar)");
                Console.WriteLine("4. Consultar notas (en orden de registro)");
                Console.WriteLine("5. Desinscribir asignatura (Desencolar)");
                Console.WriteLine("6. Salir");
                Console.Write("\nSeleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("\n Opción inválida. Presione una tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        // Simula una solicitud de inscripción, agregando (Encolando) materias.
                        InscribirAsignaturas(); 
                        break;

                    case 2:
                        // Permite visualizar el estado actual de la cola de solicitudes.
                        MostrarAsignaturas();
                        break;

                    case 3:
                        // Agrega notas a las materias en el mismo orden de registro (otra cola FIFO).
                        IngresarNotas();
                        break;
                    case 4:
                        // Simula la solicitud de consulta de notas, mostrando datos en el orden recibido.
                        ConsultarNotas();
                        break;
                    case 5:
                        // Simula el proceso de atender una solicitud del servidor eliminando la primera asignatura en cola.
                        DesinscribirAsignatura();
                        break;

                    case 6:
                        Console.WriteLine("\n Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("\n Opción no válida. Presione una tecla para continuar...");
                        break;
                }

                Console.ReadKey();

            } while (opcion != 6);
        }

        static void InscribirAsignaturas()
        {
            Console.WriteLine("Ingrese el nombre de la asignatura a inscribir (o 'salir' para terminar):");

            while (true)
            {
                string asignatura = Console.ReadLine();

                // Verifica entradas vacías o nulas
                if (string.IsNullOrWhiteSpace(asignatura))
                {
                    Console.WriteLine("Ingrese un nombre válido o 'salir' para terminar:");
                    continue;
                }

                // Palabra clave para salir del ciclo
                if (asignatura.ToLower() == "salir")
                    break;

                // Cada nueva asignatura representa una nueva solicitud enviada al servidor,
                // que se almacena en la cola para ser procesada en el orden recibido.
                colaSolicitudes.Enqueue(asignatura);
                Console.WriteLine($" '{asignatura}' agregada correctamente. Ingrese otra asignatura o 'salir' para terminar:");
            }

            Console.WriteLine("\n--- Asignaturas inscritas ---");
            foreach (var materia in colaSolicitudes)
            {
                Console.WriteLine($"- {materia}");
            }
        }

        static void MostrarAsignaturas()
        {
            Console.WriteLine("\n--- Asignaturas inscritas ---");

            // Si la cola está vacía, no hay solicitudes pendientes
            if (colaSolicitudes.Count == 0)
            {
                Console.WriteLine("No hay asignaturas en la cola.");
                return;
            }

            // Recorre la cola sin modificar su orden ni eliminar elementos
            foreach (var materia in colaSolicitudes)
            {
                Console.WriteLine($"- {materia}");
            }
        }
        static void IngresarNotas()
        {
            //Si no hay asignaturas inscritas, no se pueden ingresar notas.
            if (colaSolicitudes.Count == 0)
            {
                Console.WriteLine("\n No hay asignaturas inscritas. Primero debe inscribir materias.");
                return;
            }

            Console.WriteLine("\n--- INGRESO DE NOTAS ---");

            // Si ya existen notas, se reinicia la cola para evitar inconsistencias.
            if (colaNotas.Count > 0)
            {
                Console.WriteLine("Ya existen notas registradas. Se reiniciarán las notas anteriores.");
                colaNotas.Clear();
            }

            // Recorre la cola de asignaturas en el mismo orden en que fueron inscritas,
            // encolando las notas correspondientes a cada una.
            foreach (var asignatura in colaSolicitudes)
            {
                double nota;
                while (true)
                {
                    Console.Write($"Ingrese la nota para '{asignatura}' (0 a 100): ");
                    string entrada = Console.ReadLine();

                    // Validación de nota numérica dentro del rango permitido
                    if (double.TryParse(entrada, out nota) && nota >= 0 && nota <= 100)
                    {
                        // Cada nota se encola manteniendo el orden de llegada de las asignaturas
                        colaNotas.Enqueue(nota);
                        break;
                    }
                    else
                    {
                        Console.WriteLine(" Valor inválido. Ingrese una nota entre 0 y 100.");
                    }
                }
            }

            Console.WriteLine("\n Notas registradas correctamente.");
        }

        static void ConsultarNotas()
        {
            Console.WriteLine("\n--- CONSULTA DE NOTAS ---");

            if (colaSolicitudes.Count == 0)
            {
                Console.WriteLine(" No hay asignaturas registradas.");
                return;
            }

            if (colaNotas.Count == 0)
            {
                Console.WriteLine(" No hay notas registradas.");
                return;
            }

            // Se convierten ambas colas a arreglos para recorrerlas en paralelo
            // sin modificar su estructura original.
            string[] materias = colaSolicitudes.ToArray();
            double[] notas = colaNotas.ToArray();

            // Calcular el ancho máximo de la columna de asignaturas
            int anchoAsignatura = materias.Max(m => m.Length) + 5;
            if (anchoAsignatura < 20) anchoAsignatura = 20; // ancho mínimo

            Console.WriteLine($"\n{"Asignatura".PadRight(anchoAsignatura)}\t{"Nota",-8}{"Estado"}");
            Console.WriteLine(new string('-', anchoAsignatura + 25));

            // Se recorren ambas estructuras en el mismo orden de inserción (FIFO),
            // garantizando la correspondencia entre asignatura y nota.
            for (int i = 0; i < materias.Length; i++)
            {
                string estado = notas[i] >= 60 ? "Aprobado" : "Reprobado";
                Console.WriteLine($"{materias[i].PadRight(anchoAsignatura)}\t{notas[i],-8:F1}{estado}");
            }

            Console.WriteLine("\nPresione una tecla para regresar al menú...");
        }

        static void DesinscribirAsignatura()
        {
            Console.WriteLine("\n--- DESINSCRIPCIÓN DE ASIGNATURAS (Desencolar) ---");

            // Si no hay asignaturas, no hay nada que eliminar.
            if (colaSolicitudes.Count == 0)
            {
                Console.WriteLine(" No hay asignaturas para desinscribir.");
                return;
            }

            // Muestra la primera asignatura que será eliminada (principio FIFO)
            string asignaturaEliminada = colaSolicitudes.Dequeue();

            // Si hay notas registradas, también se elimina la primera nota correspondiente
            if (colaNotas.Count > 0)
            {
                double notaEliminada = colaNotas.Dequeue();
                Console.WriteLine($"\n Se ha desinscrito la asignatura '{asignaturaEliminada}' junto con su nota ({notaEliminada:F1}).");
            }
            else
            {
                Console.WriteLine($"\n Se ha desinscrito la asignatura '{asignaturaEliminada}', sin nota registrada.");
            }

            // Muestra el estado actualizado de la cola
            if (colaSolicitudes.Count > 0)
            {
                Console.WriteLine("\n--- Asignaturas restantes en la cola ---");
                foreach (var materia in colaSolicitudes)
                {
                    Console.WriteLine($"- {materia}");
                }
            }
            else
            {
                Console.WriteLine("\n No quedan asignaturas en la cola.");
            }

            Console.WriteLine("\nPresione una tecla para regresar al menú...");
        }
    }
}