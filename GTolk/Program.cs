using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

using CoreDll;

namespace GTolk
{
    static class Program
    {


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            //UInt16 xxx = BitConverter.ToUInt16(new byte[] { 1, 1, 1 }, 0);

            //            byte[] bts = BitConverter.GetBytes((ushort)UInt16.MaxValue);


            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OpçõesForm());
            return;
            */

            try
            {


                string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;


                if (System.Diagnostics.Process.GetProcessesByName(processName).Count() > 1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            try
            {
                //Conexão.Connect("SERVIDOR", "GTolk", 10, "sistema", "schwer_wissen", "GTolk");
                Conexão.Connect("SERVIDOR", "GESTPLUS_ALFA", 10, "sistema", "schwer_wissen", "GTolk");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            List<Models.Contato> contatos = Models.Contato.GetBy();

            //Models.Contato contato = contatos.Where(x => x.Email.Contains("mariana")).FirstOrDefault();
            //contato.Senha = "321654";
            //Models.Contato.Save(contato);



            try
            {
                if (Program.AtualizarVersãoDoInternetExplorer())
                {
                    return;
                }
            }
            catch (Exception ex)
            {
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm loginFrm = new LoginForm();
            loginFrm.ShowDialog();

            GTolk.Models.Contato usuárioLogado = loginFrm.UsuárioCredenciado;

            loginFrm = null;

            if (usuárioLogado != null)
            {
                if (!usuárioLogado.IsAdmin)
                {
                    try
                    {
                        object obj = Conexão.ExecuteScalar("select count(*) from Tlk_Manutenção");
                        int count = (int)obj;

                        if (count > 0)
                        {
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                

                Application.Run(new ContatosForm(usuárioLogado));
            }

        }

        public static bool AtualizarVersãoDoInternetExplorer()
        {
            string PATH = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(PATH);

            string exeName = Application.ExecutablePath.Split(@"\").Last().SkipRight(4);

            object valor = registryKey.GetValue(exeName + ".exe");
            bool hasValue = (valor != null);

            if (!hasValue)
            {
                registryKey.SetValue(exeName + ".exe", 11001);
                registryKey.SetValue(exeName + ".vshost.exe", 11001);

                System.Diagnostics.Process.Start(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

                //MessageBox.Show("O aplicativo acabou de atualizar a versão do Internet Explorer e deverá ser reiniciado. Esse procedimento acontece na primeira execução.");
                return true;
            }
            else
            {
                try
                {
                    registryKey.SetValue(exeName + ".exe", 11001);
                    registryKey.SetValue(exeName + ".vshost.exe", 11001);
                }
                catch { }
                return false;
            }
        }
    }
}