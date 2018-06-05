using System;
using System.Data;
using System.Windows.Forms;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Cbm;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;

namespace Com.Nidec.Mes.GlobalMasterMaintenance
{
    public partial class LocalSupplierCavityForm
    {
        #region Variables

        /// <summary>
        /// datatable for item data
        /// </summary>
        private DataTable localSupplierDatatable;

        /// <summary>
        /// initialize popupmessagecontroller
        /// </summary>
        private readonly PopUpMessageController popUpMessage = new PopUpMessageController();

        /// <summary>
        /// initialize CommonLogger
        /// </summary>
        private static readonly CommonLogger logger = CommonLogger.GetInstance(typeof(LocalSupplierCavityForm));

        /// <summary>
        ///  get message data
        /// </summary>
        private MessageData messageData;

        #endregion

        #region Constructor

        /// <summary>
        /// constructor of the form
        /// </summary>
        public LocalSupplierCavityForm()
        {
            InitializeComponent();
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Fills all user records to gridview control
        /// </summary>
        private void GridBind(LocalSupplierCavityVo conditionInVo)
        {
            LocalSupplierCavity_dgv.DataSource = null;

            try
            {
                LocalSupplierCavityVo outVo = (LocalSupplierCavityVo)base.InvokeCbm(new GetLocalSupplierCavityMasterMntCbm(), conditionInVo, false);

                LocalSupplierCavity_dgv.AutoGenerateColumns = false;

                BindingSource bindingSource1 = new BindingSource(outVo.LocalSupplierCavityListVo, null);

                if (bindingSource1.Count > 0)
                {
                    LocalSupplierCavity_dgv.DataSource = bindingSource1;
                }
                else
                {
                    messageData = new MessageData("mmci00006", Properties.Resources.mmci00006, null); //"mold"
                    logger.Info(messageData);
                    popUpMessage.Information(messageData, Text);
                }

                LocalSupplierCavity_dgv.ClearSelection();

                Update_btn.Enabled = false;

                Delete_btn.Enabled = false;
            }
            catch (Framework.ApplicationException exception)
            {
                popUpMessage.ApplicationError(exception.GetMessageData(), Text);
                logger.Error(exception.GetMessageData());
            }
        }


        /// <summary>
        /// Creates seacrh condition as per user inputs 
        /// </summary>
        /// <returns>search condition</returns>
        private LocalSupplierCavityVo FormConditionVo()
        {
            LocalSupplierCavityVo inVo = new LocalSupplierCavityVo();

            if (LocalSupplierCavityCode_txt.Text != string.Empty) { inVo.LocalSupplierCavityCode = LocalSupplierCavityCode_txt.Text; }

            if (LocalSupplierCavityName_txt.Text != string.Empty)
            {
                inVo.LocalSupplierCavityName = LocalSupplierCavityName_txt.Text;
            }

            if (LocalSupplier_cmb.SelectedIndex > -1)
            {
                inVo.LocalSupplierId = Convert.ToInt32(LocalSupplier_cmb.SelectedValue);
            }

            return inVo;

        }


        /// <summary>
        /// Handles Combobox loading for Item
        /// </summary>
        /// <param name="pCombo"></param>
        /// <param name="pDatasource"></param>
        /// <param name="pDisplay"></param>
        /// <param name="pValue"></param>
        private void ComboBind(ComboBox pCombo, DataTable pDatasource, string pDisplay, string pValue)
        {
            pCombo.DataSource = pDatasource;
            pCombo.DisplayMember = pDisplay;
            pCombo.ValueMember = pValue;
            pCombo.SelectedIndex = -1;
            pCombo.Text = string.Empty;
        }

        /// <summary>
        /// selects user record for updation and show user form
        /// </summary>
        private void BindUpdateUserData()
        {
            int selectedrowindex = LocalSupplierCavity_dgv.SelectedCells[0].RowIndex;

            LocalSupplierCavityVo selectedMold = (LocalSupplierCavityVo)LocalSupplierCavity_dgv.Rows[selectedrowindex].DataBoundItem;

            AddLocalSupplierCavityForm newAddForm = new AddLocalSupplierCavityForm(CommonConstants.MODE_UPDATE, selectedMold);

            newAddForm.ShowDialog(this);

            if (newAddForm.IntSuccess > 0)
            {
                messageData = new MessageData("mmci00002", Properties.Resources.mmci00002, null);
                logger.Info(messageData);
                popUpMessage.Information(messageData, Text);

                GridBind(FormConditionVo());
            }
        }


        /// <summary>
        /// form country and factory data for combo
        /// </summary>
        private void FormDatatableFromVo()
        {
            localSupplierDatatable = new DataTable();
            localSupplierDatatable.Columns.Add("id");
            localSupplierDatatable.Columns.Add("code");

            try
            {
                LocalSupplierVo localSupplierVo = (LocalSupplierVo)base.InvokeCbm(new GetLocalSupplierMasterMntCbm(), new LocalSupplierVo(), false);

                foreach (LocalSupplierVo localSupplier in localSupplierVo.LocalSupplierListVo)
                {
                    localSupplierDatatable.Rows.Add(localSupplier.LocalSupplierId, localSupplier.LocalSupplierName );
                }
            }
            catch (Framework.ApplicationException exception)
            {
                popUpMessage.ApplicationError(exception.GetMessageData(), Text);
                logger.Error(exception.GetMessageData());
            }
        }


        #endregion

        #region FormEvents
        /// <summary>
        /// Loads Mold form
        /// Fill item combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocalSupplierCavityForm_Load(object sender, EventArgs e)
        {
            FormDatatableFromVo();

            ComboBind(LocalSupplier_cmb, localSupplierDatatable, "code", "id");

            LocalSupplierCavityCode_txt.Select();

            Update_btn.Enabled = Delete_btn.Enabled = false;
        }

        #endregion

        #region ButtonClick

        /// <summary>
        /// event to clear the controls of search criteria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_btn_Click(object sender, EventArgs e)
        {
            LocalSupplierCavityCode_txt.Text = string.Empty;

            LocalSupplierCavityName_txt.Text = string.Empty;

            LocalSupplier_cmb.SelectedIndex = -1;

            LocalSupplierCavity_dgv.DataSource = null;

            LocalSupplierCavityCode_txt.Select();

            Delete_btn.Enabled = Update_btn.Enabled = false;
        }

        /// <summary>
        /// event to get the record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_btn_Click(object sender, EventArgs e)
        {
            GridBind(FormConditionVo());
        }

        /// <summary>
        /// event to open the  add screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_btn_Click(object sender, EventArgs e)
        {
            AddLocalSupplierCavityForm newAddForm = new AddLocalSupplierCavityForm(CommonConstants.MODE_ADD);

            newAddForm.ShowDialog();

            if (newAddForm.IntSuccess > 0)
            {
                messageData = new MessageData("mmci00001", Properties.Resources.mmci00001, null);
                logger.Info(messageData);
                popUpMessage.Information(messageData, Text);

                GridBind(FormConditionVo());
            }
            else if (newAddForm.IntSuccess == 0)
            {
                messageData = new MessageData("mmci00007", Properties.Resources.mmci00007, null);
                logger.Info(messageData);
                popUpMessage.Information(messageData, Text);
                GridBind(FormConditionVo());
            }
        }

        /// <summary>
        /// event to open the  updatescreen 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_btn_Click(object sender, EventArgs e)
        {
            BindUpdateUserData();
        }

        /// <summary>
        /// event to delete the selected record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_btn_Click(object sender, EventArgs e)
        {

            int selectedrowindex = LocalSupplierCavity_dgv.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = LocalSupplierCavity_dgv.Rows[selectedrowindex];

            messageData = new MessageData("mmcc00004", Properties.Resources.mmcc00004, selectedRow.Cells["colLocalSupplierCavityCode"].Value.ToString());
            logger.Info(messageData);
            DialogResult dialogResult = popUpMessage.ConfirmationOkCancel(messageData, Text);

            if (dialogResult == DialogResult.OK)
            {
                LocalSupplierCavityVo inVo = new LocalSupplierCavityVo
                {
                    LocalSupplierCavityId = Convert.ToInt32(selectedRow.Cells["colLocalSupplierCavityId"].Value),
                    RegistrationDateTime = Convert.ToDateTime(DateTime.Now.ToString(UserData.GetUserData().DateTimeFormat)),
                    RegistrationUserCode = UserData.GetUserData().UserCode,
                };

                try
                {
                    LocalSupplierCavityVo outVo = (LocalSupplierCavityVo)base.InvokeCbm(new DeleteLocalSupplierCavityMasterMntCbm(), inVo, false);

                    if (outVo.AffectedCount > 0)
                    {
                        messageData = new MessageData("mmci00003", Properties.Resources.mmci00003, null);
                        logger.Info(messageData);
                        popUpMessage.Information(messageData, Text);

                        GridBind(FormConditionVo());
                    }
                    else if (outVo.AffectedCount == 0)
                    {
                        messageData = new MessageData("mmci00007", Properties.Resources.mmci00007, null);
                        logger.Info(messageData);
                        popUpMessage.Information(messageData, Text);
                        GridBind(FormConditionVo());
                    }
                }
                catch (Framework.ApplicationException exception)
                {
                    popUpMessage.ApplicationError(exception.GetMessageData(), Text);
                    logger.Error(exception.GetMessageData());
                }
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                //do something else
            }
        }

        /// <summary>
        /// close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region ControlEvents

        /// <summary>
        /// Handles user record selection for Update/Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mold_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (LocalSupplierCavity_dgv.SelectedRows.Count > 0)
            {
                Update_btn.Enabled = Delete_btn.Enabled = true;
            }
            else
            {
                Update_btn.Enabled = Delete_btn.Enabled = false;
            }
        }

        /// <summary>
        /// Handles update user form show on row double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mold_dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (LocalSupplierCavity_dgv.SelectedRows.Count > 0)
            {
                BindUpdateUserData();
            }
        }

        #endregion

    }
}
