using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPupHologram
{
    public partial class Form1 : Form
    {
        private static int clickCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            clickCount++;
            PupHologram.PupTopper app = new PupHologram.PupTopper();
            app.GameChanged("TestGameName" + clickCount.ToString(), "TestManuName", "TestDefault");
            
        }

    }
}
