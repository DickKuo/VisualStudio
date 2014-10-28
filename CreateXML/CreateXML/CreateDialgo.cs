using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CreateXML
{
    public partial class CreateDialgo : BaseDialog
    {
        public CreateDialgo()
        {
            InitializeComponent();
        }


        public bool IsEntities { set; get; }
        public bool IsInterFace { set; get; }
        public bool IsService { set; get; }
        public bool IsQieryView { set; get; }
        public bool IsQueryViewLanguage { set; get; }
        public bool IsUI { set; get; }
        public bool IsUILanguage { set; get; }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

       

    }
}
