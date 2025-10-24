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
        // COLA PRINCIPAL DEL SERVIDOR: Almacena las peticiones de acción a realizar (FIFO).
        // Contiene strings que representan las funciones a ejecutar (ej: "InscribirAsignaturas").
        static Queue<string> colaPeticiones = new Queue<string>();

        // COLA DE DATOS 1: Almacena las asignaturas inscritas (modificada por las acciones).
        static Queue<string> colaSolicitudes = new Queue<string>();

        // COLA DE DATOS 2: Almacena las notas en el mismo orden de registro (FIFO).
        static Queue<double> colaNotas = new Queue<double>();

        static void Main(string[] args)
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("=== SIMULACIÓN DEL SERVIDOR UNIVERSITARIO (COLA DE PETICIONES) ===");
                Console.WriteLine("1. Agregar Petición (ENCOLAR nueva acción)");
                Console.WriteLine("2. Ver Peticiones pendientes (Cola de acciones)");
                Console.WriteLine("3. Atender la Petición más antigua (DESENCOLAR y ejecutar)");
                Console.WriteLine("4. Salir");
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
                        // Permite al usuario encolar una solicitud de acción.
                        AgregarPeticion();
                        break;

                    case 2:
                        // Muestra el contenido de la cola de peticiones.
                        MostrarPeticionesPendientes();
                        break;

                    case 3:
                        // Atiende y ejecuta la petición más antigua.
                        AtenderPeticion();
                        break;

                    case 4:
                        Console.WriteLine("\n Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("\n Opción no válida. Presione una tecla para continuar...");
                        break;
                }

                if (opcion != 4)
                {
                    Console.ReadKey();
                }

            } while (opcion != 4);
        }

        // Función para agregar una nueva petición a la cola de acciones.
        static void AgregarPeticion()
        {
            Console.Clear();
            Console.WriteLine("--- ENCOLAR UNA NUEVA PETICIÓN DE ACCIÓN ---");
            Console.WriteLine("Seleccione la acción que desea enviar al servidor:");
            Console.WriteLine("1. Inscribir asignaturas");
            Console.WriteLine("2. Ver asignaturas inscritas");
            Console.WriteLine("3. Ingresar notas");
            Console.WriteLine("4. Consultar notas");
            Console.WriteLine("5. Desinscribir asignatura");
            Console.Write("\nOpción (1-5): ");

            if (int.TryParse(Console.ReadLine(), out int subOpcion))
            {
                string peticion = null;
                switch (subOpcion)
                {
                    case 1:
                        peticion = "InscribirAsignaturas";
                        break;
                    case 2:
                        peticion = "MostrarAsignaturas";
                        break;
                    case 3:
                        peticion = "IngresarNotas";
                        break;
                    case 4:
                        peticion = "ConsultarNotas";
                        break;
                    case 5:
                        peticion = "DesinscribirAsignatura";
                        break;
                }

                if (peticion != null)
                {
                    // La petición se encola en la cola principal del servidor
                    colaPeticiones.Enqueue(peticion);
                    Console.WriteLine($"\nPetición '{peticion}' encolada. Posición: {colaPeticiones.Count}.");
                }
                else
                {
                    Console.WriteLine("\n Opción inválida. No se agregó ninguna petición.");
                }
            }
            else
            {
                Console.WriteLine("\n Entrada inválida. No se agregó ninguna petición.");
            }
        }

        // Función para mostrar las peticiones en cola.
        static void MostrarPeticionesPendientes()
        {
            Console.WriteLine("\n--- PETICIONES PENDIENTES EN COLA (FIFO) ---");

            if (colaPeticiones.Count == 0)
            {
                Console.WriteLine(" La cola de peticiones está vacía.");
                return;
            }

            Console.WriteLine($"Total de peticiones pendientes: {colaPeticiones.Count}\n");

            int contador = 1;
            // Recorre la cola sin modificarla.
            foreach (var peticion in colaPeticiones)
            {
                Console.WriteLine($"{contador}. {peticion}");
                contador++;
            }
        }

        // Función para desencolar y ejecutar la petición.
        static void AtenderPeticion()
        {
            Console.WriteLine("\n--- ATENDER PETICIÓN (DESENCOLAR Y EJECUTAR) ---");

            if (colaPeticiones.Count == 0)
            {
                Console.WriteLine(" No hay peticiones pendientes para atender.");
                return;
            }

            // 1. DESENCOLAR: Obtiene y elimina la petición más antigua.
            string peticionAtendida = colaPeticiones.Dequeue();
            Console.WriteLine($"\nAtendiendo la petición: '{peticionAtendida}'...");
            Console.WriteLine("------------------------------------------\n");

            // 2. EJECUTAR: Llama a la función correspondiente según el nombre desencolado.
            switch (peticionAtendida)
            {
                case "InscribirAsignaturas":
                    InscribirAsignaturas();
                    break;
                case "MostrarAsignaturas":
                    MostrarAsignaturas();
                    break;
                case "IngresarNotas":
                    IngresarNotas();
                    break;
                case "ConsultarNotas":
                    ConsultarNotas();
                    break;
                case "DesinscribirAsignatura":
                    DesinscribirAsignatura();
                    break;
                default:
                    Console.WriteLine($"Error: Petición '{peticionAtendida}' no reconocida.");
                    break;
            }

            Console.WriteLine("\n------------------------------------------");
            Console.WriteLine($"Petición '{peticionAtendida}' finalizada.");
        }

        // *******************************************************************
        // FUNCIONES DE LÓGICA DE NEGOCIO (Ejecutadas por la petición atendida)
        // *******************************************************************

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