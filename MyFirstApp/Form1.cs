using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.FSharp.Core;
using Microsoft.FSharp.Collections;
using NUnit.Framework;
using NetworkKit;

namespace MyFirstApp
{
    public partial class MyFirstApp : Form
    {
        public MyFirstApp()
        {
            InitializeComponent();
        }

        private void MyFirstApp_Load(object sender, EventArgs e)
        {
            Button[] buttonlist = { button1, button2, button3, button4, button5, button6, button7, button8, button9, button10, button11, button12 };
            foreach (Button item in buttonlist)
                item.Click += new System.EventHandler(Input_func(item));
        }

        private Action<object, EventArgs> Input_func(Button arg)
        {
            Action<object, EventArgs> func = (object sender, EventArgs e) =>
             {
                 if (arg.BackColor == Control.DefaultBackColor)
                     arg.BackColor = Color.Black;
                 else
                     arg.BackColor = Control.DefaultBackColor;
             };
            return func;
        }
    }
}
