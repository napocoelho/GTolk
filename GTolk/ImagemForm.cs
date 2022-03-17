using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using GTolk.Util;

namespace GTolk
{
    public partial class ImagemForm : Form
    {
        private int Área { get; set; }
        private int ÁreaMáxima { get; set; }
        private int ÁreaMínima { get; set; }

        private Point? ImagePart { get; set; }

        //private bool MousePressionado { get; set; }
        private bool MouseSobre { get; set; }

        public static Image ImagemEditada { get; private set; }

        public ImagemForm()
        {
            InitializeComponent();

            ImagemEditada = null;

            pictureOrigem.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureOrigem.BorderStyle = BorderStyle.FixedSingle;
            pictureOrigem.MouseClick += pictureOrigem_MouseClick;
            pictureOrigem.MouseMove += pictureOrigem_MouseMove;
            pictureOrigem.MouseDown += pictureOrigem_MouseDown;
            pictureOrigem.MouseUp += pictureOrigem_MouseUp;
            

            pictureFinal.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureFinal.BorderStyle = BorderStyle.FixedSingle;

            this.ImagePart = null;
            this.ÁreaMáxima = 200;
            this.ÁreaMínima = 0;
            this.Área = this.ÁreaMáxima / 2;

            pictureOrigem.Image = Image.FromFile(@"sem_imagem.png");
            pictureOrigem.Focus();
        }
        
        void pictureOrigem_MouseUp(object sender, MouseEventArgs e)
        {
            this.MouseSobre = false;
        }

        void pictureOrigem_MouseDown(object sender, MouseEventArgs e)
        {
            this.MouseSobre = true;
        }

        void pictureOrigem_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.MouseSobre)
            {
                this.SetPicture(e.Location);
            }
        }

        void pictureOrigem_MouseClick(object sender, MouseEventArgs e)
        {
            this.SetPicture(e.Location);
        }

        private void SetPicture(Point location)
        {
            Image imagem = this.GetRoundedPicture(location);
            pictureFinal.Image = imagem;
        }

        private Image GetSquaredPicture(Point location)
        {
            if (this.pictureOrigem.Image == null)
                return null;

            this.ImagePart = location;

            int quadrado = this.Área;
            int correçãoHorizontal = (int)(pictureOrigem.Width - pictureOrigem.Image.Width) / 2;
            int correçãoVertical = (int)(pictureOrigem.Height - pictureOrigem.Image.Height) / 2;

            int centroHorizontal = location.X - (quadrado / 2);
            int centroVertical = location.Y - (quadrado / 2);

            centroHorizontal = centroHorizontal - correçãoHorizontal;
            centroVertical = centroVertical - correçãoVertical;

            Image imagem = Imagens.CutRectangle(pictureOrigem.Image, centroHorizontal, centroVertical, quadrado, quadrado);
            return imagem;
        }

        private Image GetRoundedPicture(Point location)
        {
            Image imagem = this.GetSquaredPicture(location);
            imagem = Imagens.CropToCircle(Imagens.ScaleImage(imagem, 200, 200), 200, true);
            return imagem;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Procurar imagem...";
            openFileDialog1.DefaultExt = "";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (openFileDialog1.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            if (!File.Exists(openFileDialog1.FileName))
            {
                return;
            }


            //string imagemPath = @"C:\Users\Junio Coelho\Pictures\teste1.png";
            string imagemPath = openFileDialog1.FileName;
            pictureOrigem.Image = Imagens.ScaleImage(Image.FromFile(imagemPath), 320, 240);

            Point location = new Point(this.ÁreaMáxima / 2, this.ÁreaMáxima / 2);
            this.SetPicture(location);
        }

        private void MaisZoom(int valor = 10)
        {
            if (this.Área == this.ÁreaMínima)
            {
                return;
            }

            if ((this.Área - valor) < this.ÁreaMínima)
            {
                valor = this.ÁreaMínima;
            }

            this.Área -= valor;

            if (this.ImagePart.HasValue)
            {
                this.SetPicture(this.ImagePart.Value);
            }
        }

        private void MenosZoom(int valor = 10)
        {
            if (this.Área == this.ÁreaMáxima)
            {
                return;
            }

            if ((this.Área + valor) > this.ÁreaMáxima)
            {
                valor = this.ÁreaMáxima;
            }

            this.Área += valor;

            if (this.ImagePart.HasValue)
            {
                this.SetPicture(this.ImagePart.Value);
            }
        }

        private void btnMaisZoom_Click_1(object sender, EventArgs e)
        {
            this.MaisZoom();
        }

        private void btnMenosZoom_Click_1(object sender, EventArgs e)
        {
            this.MenosZoom();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ImagemEditada = null;
            this.Close();
        }

        private void btnAceitar_Click(object sender, EventArgs e)
        {
            ImagemEditada = pictureFinal.Image;
            this.Close();
        }

        private void pictureFinal_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureFinal_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(pictureFinal, e.Location);
            }
        }

        private void salvarImagemQuadradaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.ImagePart.HasValue)
                return;

            Image imagem = this.GetSquaredPicture(this.ImagePart.Value);

            this.SalvarImagem(Imagens.ScaleImage(imagem, 200, 200));
        }

        private void salvarImagemRedondaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.ImagePart.HasValue)
                return;

            Image imagem = this.GetRoundedPicture(this.ImagePart.Value);

            this.SalvarImagem(imagem);
        }

        private void SalvarImagem(Image imagem)
        {
            try
            {
                if (imagem != null)
                {

                    saveFileDialog1.Title = "Salvar imagem...";
                    saveFileDialog1.DefaultExt = "";
                    saveFileDialog1.CheckFileExists = false;
                    saveFileDialog1.CheckPathExists = true;
                    saveFileDialog1.FileName = imagem.GetHashCode() + ".png";
                    saveFileDialog1.Filter = "Image files (*.png) | *.png";

                    if (saveFileDialog1.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }

                    imagem.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
