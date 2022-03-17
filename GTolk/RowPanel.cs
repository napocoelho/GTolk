using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GTolk.Models;

namespace GTolk
{
    public class RowPanel : System.Windows.Forms.Panel
    {
        private object LOCK = new object();

        /// <summary>
        /// Represents a row index;
        /// </summary>
        public int Index { get; set; }
        public Contato Value { get; set; }
        public Label BoxApelido { get; set; }
        public Label BoxDescrição { get; set; }
        public PictureBox BoxImagem { get; set; }
        public Panel BoxStatus { get; set; }
        public bool IsMouseOver { get { lock (LOCK) { return (this.MouseOverCounter > 0); } } }
        private int MouseOverCounter { get; set; }

        public RowPanel()
        {
            //this.Value = default(T);
            this.Index = 0;

            this.BoxApelido = null;
            this.BoxDescrição = null;
            this.BoxImagem = null;
            this.BoxStatus = null;
            this.MouseOverCounter = 0;
        }

        public void MouseHasEntered()
        {
            lock (LOCK)
            {
                this.MouseOverCounter = this.MouseOverCounter + 1;
            }
        }

        public void MouseHasLeaved()
        {
            lock (LOCK)
            {
                this.MouseOverCounter = this.MouseOverCounter - 1;

                if (this.MouseOverCounter <= 0)
                {
                    //this.OnMouseLeave(new EventArgs());
                    this.MouseOverCounter = 0;
                }
            }
        }

        
    }
}