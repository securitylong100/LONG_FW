﻿namespace Com.Nidec.Mes.GlobalMasterMaintenance
{
    partial class AddLineMasterForm: FormCommonMaster
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddLineMasterForm));
            this.LineCode_txt = new Com.Nidec.Mes.Framework.TextBoxCommon();
            this.LineCode_lbl = new Com.Nidec.Mes.Framework.LabelCommon();
            this.LineName_txt = new Com.Nidec.Mes.Framework.TextBoxCommon();
            this.LineName_lbl = new Com.Nidec.Mes.Framework.LabelCommon();
            this.Exit_btn = new Com.Nidec.Mes.Framework.ButtonCommon();
            this.Ok_btn = new Com.Nidec.Mes.Framework.ButtonCommon();
            this.labelCommon2 = new Com.Nidec.Mes.Framework.LabelCommon();
            this.labelCommon1 = new Com.Nidec.Mes.Framework.LabelCommon();
            this.UpdateText_lbl = new Com.Nidec.Mes.Framework.LabelCommon();
            this.SuspendLayout();
            // 
            // LineCode_txt
            // 
            this.LineCode_txt.ControlId = null;
            resources.ApplyResources(this.LineCode_txt, "LineCode_txt");
            this.LineCode_txt.InputType = Com.Nidec.Mes.Framework.TextBoxCommon.InputTypeList.All;
            this.LineCode_txt.Name = "LineCode_txt";
            // 
            // LineCode_lbl
            // 
            resources.ApplyResources(this.LineCode_lbl, "LineCode_lbl");
            this.LineCode_lbl.ControlId = null;
            this.LineCode_lbl.Name = "LineCode_lbl";
            // 
            // LineName_txt
            // 
            this.LineName_txt.ControlId = null;
            resources.ApplyResources(this.LineName_txt, "LineName_txt");
            this.LineName_txt.InputType = Com.Nidec.Mes.Framework.TextBoxCommon.InputTypeList.All;
            this.LineName_txt.Name = "LineName_txt";
            // 
            // LineName_lbl
            // 
            resources.ApplyResources(this.LineName_lbl, "LineName_lbl");
            this.LineName_lbl.ControlId = null;
            this.LineName_lbl.Name = "LineName_lbl";
            // 
            // Exit_btn
            // 
            this.Exit_btn.BackColor = System.Drawing.SystemColors.Control;
            this.Exit_btn.ControlId = null;
            resources.ApplyResources(this.Exit_btn, "Exit_btn");
            this.Exit_btn.Name = "Exit_btn";
            this.Exit_btn.UseVisualStyleBackColor = true;
            this.Exit_btn.Click += new System.EventHandler(this.Exit_btn_Click);
            // 
            // Ok_btn
            // 
            this.Ok_btn.BackColor = System.Drawing.SystemColors.Control;
            this.Ok_btn.ControlId = null;
            resources.ApplyResources(this.Ok_btn, "Ok_btn");
            this.Ok_btn.Name = "Ok_btn";
            this.Ok_btn.UseVisualStyleBackColor = true;
            this.Ok_btn.Click += new System.EventHandler(this.Ok_btn_Click);
            // 
            // labelCommon2
            // 
            resources.ApplyResources(this.labelCommon2, "labelCommon2");
            this.labelCommon2.ControlId = null;
            this.labelCommon2.ForeColor = System.Drawing.Color.DarkRed;
            this.labelCommon2.Name = "labelCommon2";
            // 
            // labelCommon1
            // 
            resources.ApplyResources(this.labelCommon1, "labelCommon1");
            this.labelCommon1.ControlId = null;
            this.labelCommon1.ForeColor = System.Drawing.Color.DarkRed;
            this.labelCommon1.Name = "labelCommon1";
            // 
            // UpdateText_lbl
            // 
            resources.ApplyResources(this.UpdateText_lbl, "UpdateText_lbl");
            this.UpdateText_lbl.ControlId = null;
            this.UpdateText_lbl.Name = "UpdateText_lbl";
            // 
            // AddLineMasterForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UpdateText_lbl);
            this.Controls.Add(this.labelCommon1);
            this.Controls.Add(this.labelCommon2);
            this.Controls.Add(this.Exit_btn);
            this.Controls.Add(this.Ok_btn);
            this.Controls.Add(this.LineName_txt);
            this.Controls.Add(this.LineName_lbl);
            this.Controls.Add(this.LineCode_txt);
            this.Controls.Add(this.LineCode_lbl);
            this.Name = "AddLineMasterForm";
            this.TitleText = "FormCommon";
            this.Load += new System.EventHandler(this.AddLineMasterForm_Load);
            this.Controls.SetChildIndex(this.LineCode_lbl, 0);
            this.Controls.SetChildIndex(this.LineCode_txt, 0);
            this.Controls.SetChildIndex(this.LineName_lbl, 0);
            this.Controls.SetChildIndex(this.LineName_txt, 0);
            this.Controls.SetChildIndex(this.Ok_btn, 0);
            this.Controls.SetChildIndex(this.Exit_btn, 0);
            this.Controls.SetChildIndex(this.labelCommon2, 0);
            this.Controls.SetChildIndex(this.labelCommon1, 0);
            this.Controls.SetChildIndex(this.UpdateText_lbl, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Com.Nidec.Mes.Framework.TextBoxCommon LineCode_txt;
        private Com.Nidec.Mes.Framework.LabelCommon LineCode_lbl;
        private Com.Nidec.Mes.Framework.TextBoxCommon LineName_txt;
        private Com.Nidec.Mes.Framework.LabelCommon LineName_lbl;
        private Com.Nidec.Mes.Framework.ButtonCommon Exit_btn;
        private Com.Nidec.Mes.Framework.ButtonCommon Ok_btn;
        private Framework.LabelCommon labelCommon2;
        private Framework.LabelCommon labelCommon1;
        private Framework.LabelCommon UpdateText_lbl;
    }
}
