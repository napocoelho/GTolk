using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Manutenção_GTolk
{
    public class Program
    {
        static bool IsMaintenanceActive()
        {
            try
            {
                object obj = ConnectionManagerDll.ConnectionManager.GetConnection.ExecuteScalar("select count(*) from Tlk_Manutenção");
                int count = (int)obj;
                return count > 0;
            }
            catch
            {
                return false;
            }
        }


        static void Main(string[] args)
        {
            if (args.Count() <= 0)
            {
                return;
            }

            int time = new Random().Next(1, 1000);
            System.Threading.Thread.Sleep(time);

            System.Diagnostics.Process.GetCurrentProcess().StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //System.Diagnostics.Process.GetCurrentProcess().StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal; //<--- serve para exibir o programa;

            string exePathName = args[0].Replace("'", "");
            string creationTimeUtc = args[1].Replace("'", "");
            string newExePathName = exePathName + ".bkp";

            try
            {
                Conexão.Connect("SERVIDOR", "GESTPLUS_ALFA", 10, "sistema", "schwer_wissen", "GTolk");
            }
            catch
            {
                return;
            }


            if (File.Exists(exePathName) && !File.Exists(newExePathName))
            {
                try
                {
                    File.Copy(exePathName, newExePathName);
                }
                catch (Exception ex) { }
            }

            while (IsMaintenanceActive())
            {
                time = new Random().Next(2000);
                Console.WriteLine("Dormindo no while..." + time.ToString() + "ms");
                System.Threading.Thread.Sleep(time);

                try
                {
                    char[] barra = { '\\' };
                    string oldExeName = exePathName.Split(barra).Last();

                    if (File.Exists(exePathName))
                    {
                        Console.WriteLine("Achou o arquivo " + oldExeName);

                        string creationTimeUtcFromProductionFile = File.GetCreationTimeUtc(exePathName).ToOADate().ToString();

                        if (creationTimeUtc == creationTimeUtcFromProductionFile)
                        {
                            Console.WriteLine("Apagando arquivo " + oldExeName);
                            File.Delete(exePathName);
                        }
                        else
                        {
                            Console.WriteLine("Trocou a versão do arquivo " + oldExeName + ". Saindo do laço while...");
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não achou o arquivo " + oldExeName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Saiu do laço while!");

            try
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("O serviço não ainda está em manutenção! Preparando para chamar o programa...");

                        Console.WriteLine("Dormindo antes de chamar o programa...");
                        System.Threading.Thread.Sleep(2000);

                        if (!IsMaintenanceActive() && File.Exists(exePathName))
                        {
                            Console.WriteLine("Chamando o arquivo do chat!");
                            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(exePathName);
                            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                            System.Diagnostics.Process.Start(info);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("O arquivo do chat ainda não existe!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Saindo da manutenção!");

            Console.ReadKey();
        }
    }
}