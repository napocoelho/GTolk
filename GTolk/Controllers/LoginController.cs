using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using CoreDll;
using CoreDll.Bindables;
using GTolk.Models;
using GTolk.Controllers.Eventos;

namespace GTolk.Controllers
{
    public class LoginController : BindableBase
    {
        public string Email { get { return base.Get<string>(); } set { base.Set(value); } }
        public string Senha { get { return base.Get<string>(); } set { base.Set(value); } }

        public LoginController()
        {
            this.Email = string.Empty;
            this.Senha = string.Empty;
        }

        public Contato Entrar()
        {
            if (this.Email.Trim().Length <= 10)
            {
                throw new Exception("O email deve conter mais de 10 caracteres!");
            }
            else if (!this.Email.Contains("@") || !this.Email.Contains("."))
            {
                throw new Exception("O email não é válido!");
            }
            else if (this.Senha.Trim().Length < 4)
            {
                throw new Exception("A senha deve conter 4 ou mais caracteres!");
            }

            foreach (Contato contato in Contato.GetBy())
            {
                if (this.Email == contato.Email && this.Senha == contato.Senha)
                {
                    return contato;
                }
            }

            throw new Exception("Conta não encontrada!");
        }

        public bool PermitirAutoLogon(Contato contato)
        {
            if (contato.AutoLogon && Perfil.PERFIL.GuidÚltimoContatoLogado.ToString() == contato.Guid.ToString())
            {
                return true;
            }

            return false;
        }
    }
}