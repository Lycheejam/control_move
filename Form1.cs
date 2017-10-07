using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arduino_builder
{
    public partial class Form1 : Form
    {
        /*
         * 1.Drag&Dropで画像を取得する。
         * 2.Drag&Dropで画像をパネル内で移動させる。
         */
        
        //ドラッグ中かのフラグ
        bool isDraging = false;
        //ポインタ宣言
        //C言語とかで言うポインタじゃなくて
        //座標位置の格納オブジェクト?変数?
        //drawingの方のpoint
        Point? diffPoint = null;

        public Form1()
        {
            InitializeComponent();
        }
        
        //ファイルパス取得、画像を表示
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            pb1.Image = Image.FromFile(@fileName[0]);
        }

        //ドラッグされた対象がファイル化どうかを判断
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }else{
                e.Effect = DragDropEffects.None;
            }
        }

        private void pb1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            /*
             * 参考：フォームやコントロールのマウスポインタ（カーソル）を待機状態にする: .NET Tips: C#, VB.NET 
             *  >https://dobon.net/vb/dotnet/form/cursorcurrent.html
             */
            Cursor.Current = Cursors.Hand;
            isDraging = true;
            diffPoint = e.Location;
            
        }

        private void pb1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDraging)
            {
                return;
            }

            //移動中の座標を取得？
            int x = pb1.Location.X + e.X - diffPoint.Value.X;
            int y = pb1.Location.Y + e.Y - diffPoint.Value.Y;

            //0,0より左と上に行かないよう制限
            if (x <= 0) x = 0;
            if (y <= 0) y = 0;

            //フォームの右と下にはみ出さないよう制限
            if (x >= this.ClientSize.Width - pb1.Width) x = this.ClientSize.Width - pb1.Width;
            if (y >= this.ClientSize.Height - pb1.Height) y = this.ClientSize.Height - pb1.Height;

            pb1.Location = new Point(x, y);
        }

        private void pb1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            isDraging = false;

            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            
        }
    }
}
