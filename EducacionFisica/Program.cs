using System;
using NLog;

namespace EducacionFisica
{
    class Program
    {
        static void Main(string[] args)
        {
            var Logger = LogManager.GetCurrentClassLogger();
            int option = 0;
            do
            {
                try
                {
                    Console.Write("Cargar Alumnos = 1 / Limpiar Archivo = 2 \n Opcion: ");
                    option = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                }
                catch(FormatException ex)
                {
                    Logger.Debug("Error de Formato! Solo se aceptan numeros!");
                    Logger.Warn($"Error de Formato! {ex.Message}");

                }
                catch(OverflowException ex){
                    Logger.Debug("Error de Desbordamiento! Solo se aceptan numeros pequeños!");
                    Logger.Warn($"Error de Desbordamiento! {ex.Message}");
                }
            } while (option<1 || option>2);
            

            switch (option)
            {
                case 1:
                    Logger.Info("Se selecciono la opcion de cargar alumnos");
                    CargarAlumnos();
                break;
                case 2:
                    Logger.Info("Se selecciono la opcion de limpiar archivo");
                    LimpiarArchivo();
                break;
            }
        }
        static void CargarAlumnos(){
            var Logger = LogManager.GetCurrentClassLogger();
            bool flag = true;

            var Alumnos = new List<Alumno>();
            int id = HelperDeArchivos.LastID();

            do
            {
                Alumno Alumno = new Alumno();
                Alumno.Id = id;
                try
                {   
                    Console.Write("Nombre del Alumno: ");
                    Alumno.Nombre = Convert.ToString(Console.ReadLine());
                    Console.Write("Apellido: ");
                    Alumno.Apellido = Convert.ToString(Console.ReadLine());
                    Console.Write("DNI: ");
                    Alumno.Dni = Convert.ToInt32(Console.ReadLine());
                    do
                    {
                        Console.Write("Seleccionar Curso\n [1]Atletismo [2]Voley [3]Futbol\nIngresar opcion: ");
                        Alumno.Curso = Convert.ToInt32(Console.ReadLine());
                    } while (Alumno.Curso <1 || Alumno.Curso>3);
                    Alumnos.Add(Alumno);

                    Logger.Debug($"Alumno cargado con exito!");
                    Logger.Info($"Cargó un alumno");


                    Console.Write("¿Quiere cargar otro alumno? Y/N ");
                    var aux = Convert.ToString(Console.ReadLine());
                    if (aux == "Y" || aux == "y")
                    {
                        flag = true;
                    }else
                    {
                        flag = false;
                    }

                    id++;
                }
                catch(FormatException ex)
                {
                    Logger.Debug("Error de Formato! Solo se aceptan numeros!");
                    Logger.Warn($"Error de Formato! {ex.Message}");

                }
                catch(OverflowException ex){
                    Logger.Debug("Error de Desbordamiento! Solo se aceptan numeros pequeños!");
                    Logger.Warn($"Error de Desbordamiento! {ex.Message}");
                }
            } while (flag);
            HelperDeArchivos.ChargeLinesAndWrite(Alumnos);
        }

        static void LimpiarArchivo(){
            var Logger = LogManager.GetCurrentClassLogger();

            bool flag = true;
            int option = 0;

            do
            {
                try
                {
                    do
                    {
                        
                        Console.Write("¿Cual archivo desea limpiar?\n [1]Atletismo [2]Voley [3]Futbol\nIngresar opcion: ");
                        option = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                    } while (option<1 || option>3);
                    Logger.Info($"Eligio la opcion {option} para limpiar");
                    HelperDeArchivos.CleanFile(option);
                    Logger.Debug("Se limpio el archivo seleccionado con exito!");

                    Console.Write("¿Desea limpiar otro archivo? Y/N ");
                    var aux = Convert.ToString(Console.ReadLine());
                    if (aux == "Y" || aux == "y")
                    {
                        flag = true;
                    }else
                    {
                        flag = false;
                    }
                }
                catch(FormatException ex)
                {
                    Logger.Debug("Error de Formato! Solo se aceptan numeros!");
                    Logger.Warn($"Error de Formato! {ex.Message}");
                }
                catch(OverflowException ex){
                    Logger.Debug("Error de Desbordamiento! Solo se aceptan numeros pequeños!");
                    Logger.Warn($"Error de Desbordamiento! {ex.Message}");
                }
            } while (flag);
        }
    }

    public class Alumno
    {
        private int id;
        private string nombre;
        private string apellido;
        private int dni;
        private int curso;

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public int Dni { get => dni; set => dni = value; }
        public int Curso { get => curso; set => curso = value; }
    }

    public static class HelperDeArchivos
    {
        static string[] paths = {"Atletismo.csv", "Voley.csv", "Futbol.csv"};
        public static void ChargeLinesAndWrite(List<Alumno> Alumnos){
            int longitudAtletismo = 0, longitudVoley = 0, longitudFutbol = 0;

            foreach (var Alumno in Alumnos)
            {
                if (Alumno.Curso == 1)
                {
                    longitudAtletismo++;
                }else if (Alumno.Curso == 2)
                {
                    longitudVoley++;
                }else if (Alumno.Curso == 3)
                {
                    longitudFutbol++;
                }
            }
            
            string[] lineasAtletismo = new string[longitudAtletismo];
            string[] lineasVoley = new string[longitudVoley];
            string[] lineasFutbol = new string[longitudFutbol];

            int i = 0, j = 0, k = 0;

            foreach (var Alumno in Alumnos)
            {
                if (Alumno.Curso == 1)
                {
                    lineasAtletismo[i] = Alumno.Id + ";" + Alumno.Nombre + ";" + Alumno.Apellido + ";" + Alumno.Dni;
                    i++;
                }else if (Alumno.Curso == 2)
                {
                    lineasVoley[j] = Alumno.Id + ";" + Alumno.Nombre + ";" + Alumno.Apellido + ";" + Alumno.Dni;
                    j++;
                }else if (Alumno.Curso == 3)
                {
                    lineasFutbol[k] = Alumno.Id + ";" + Alumno.Nombre + ";" + Alumno.Apellido + ";" + Alumno.Dni;
                    k++;
                }
            }


            WriteFile("Atletismo.csv",lineasAtletismo);
            WriteFile("Voley.csv",lineasVoley);
            WriteFile("Futbol.csv",lineasFutbol);
        }

        public static void WriteFile(string path, string[] lines){
            if (File.Exists(path) && new FileInfo("Voley.csv").Length > 0)
            {
                string[] lineasDelArchivo = File.ReadAllLines(path);
                string[] concatenado = lineasDelArchivo.Concat(lines).ToArray();
                File.WriteAllLines(path, concatenado);
            }else
            {
                File.WriteAllLines(path, lines);
            }
        }
        public static void CleanFile(int option){
            switch (option)
            {
                case 1:
                    File.WriteAllText("Atletismo.csv",string.Empty);
                    break;
                case 2:
                    File.WriteAllText("Voley.csv",string.Empty);
                    break;
                case 3:
                    File.WriteAllText("Futbol.csv",string.Empty);
                    break;
            }  
        }

        public static int LastID(){
            //string[] atletismoLines, voleyLines, futbolLines;
            int id1 = 0, id2 = 0, id3 = 0;

            if (File.Exists(paths[0]) & new FileInfo(paths[0]).Length>0)
            {
                //atletismoLines = File.ReadAllLines(paths[0]);
                id1 = Convert.ToInt32(File.ReadAllLines(paths[0]).Last().Split(';',2)[0]);
            }
            if (File.Exists(paths[1]) & new FileInfo(paths[1]).Length>0)
            {
                //voleyLines = File.ReadAllLines(paths[1]);
                id2 = Convert.ToInt32(File.ReadAllLines(paths[1]).Last().Split(';',2)[0]);
            }
            if (File.Exists(paths[2]) & new FileInfo(paths[2]).Length>0)
            {
                //futbolLines = File.ReadAllLines(paths[2]);
                id3 = Convert.ToInt32(File.ReadAllLines(paths[2]).Last().Split(';',2)[0]);
            }

            if (id1>id2 && id1>id3) return id1+1;
            else if (id2>id1 && id2>id3) return id2+1;
            else if (id3>id1 && id3>id2) return id3+1;

            return 0;
        }
    }
}