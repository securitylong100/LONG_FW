using System;
using System.Windows.Forms;
using System.Reflection;

namespace Com.Nidec.Mes.Framework
{
    public partial class FactorySelectionForm
    {


        /// <summary>
        /// 
        /// </summary>
        private string applicationAssemblyName = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        private string applicationTypeName = string.Empty;

        public FactorySelectionForm(string assemblyname, string typename)
        {

            applicationAssemblyName = assemblyname;

            applicationTypeName = typename;

            InitializeComponent();
        }


        private void Ok_btn_Click(object sender, EventArgs e)
        {
            if (Factory_cmb.SelectedIndex > -1 && Factory_cmb.SelectedItem !=null)
            {
                UserData userData = UserData.GetUserData();
                userData.FactoryCode = Factory_cmb.SelectedItem.ToString();


                Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + applicationAssemblyName); // dll name
                Type type = assembly.GetType(applicationTypeName);  //form name with namespace
                FormCommon menuform = Activator.CreateInstance(type) as FormCommon;


                //MainForm mainForm = new MainForm();

                this.Hide();

                menuform.ShowDialog(menuform);
                //mainForm.ShowDialog(mainForm);

                this.Show();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Please select factory", "factory selection", MessageBoxButtons.OK);

                Factory_cmb.Focus();
            }

        }

        private void Exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FactorySelectionForm_Load(object sender, EventArgs e)
        {
            Factory_cmb.Items.Clear();
            foreach (string factory in UserData.GetUserData().FactoryCodeList)
            {
                Factory_cmb.Items.Add(factory);
            }
            Factory_cmb.SelectedIndex = 0;
            Factory_cmb.Select();
        }
    }
}
