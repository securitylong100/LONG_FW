using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Com.Nidec.Mes.Framework
{
    public partial class WaitProgressForm : Form
    {
        private string displayStatus;

        private Stopwatch sw;

        public WaitProgressForm(string status)
        {
            displayStatus = status;
            InitializeComponent();
        }

        private void WaitProgressForm_Load(object sender, EventArgs e)
        {
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            this.Invoke(new Action(() => Progress_tmr.Enabled = true));
            Status_txt.Invoke(new Action(() => Status_txt.Text = displayStatus));

        }

        private void Progress_tmr_Tick(object sender, EventArgs e)
        {
            if (this.IsHandleCreated)
            {
              //  Time_txt.Invoke(new Action(() => Time_txt.Text = sw.Elapsed.Minutes + ":" + sw.Elapsed.Seconds));
            }            
        }

    }
}
