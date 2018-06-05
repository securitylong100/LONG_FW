using System;
using System.Data;
using System.Windows.Forms;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;
using Com.Nidec.Mes.GlobalMasterMaintenance.Cbm;
using Com.Nidec.Mes.Framework;



namespace Com.Nidec.Mes.GlobalMasterMaintenance
{
    public partial class DefectiveReasonForm
    {

        #region Variables

        private DataTable defectiveCategoryDatatable;

        /// <summary>
        /// initialize popupmessagecontroller
        /// </summary>
        private readonly PopUpMessageController popUpMessage = new PopUpMessageController();

        /// <summary>
        /// initialize CommonLogger
        /// </summary>
        private static readonly CommonLogger logger = CommonLogger.GetInstance(typeof(DefectiveReasonForm));

        /// <summary>
        ///  get message data
        /// </summary>
        private MessageData messageData;

        /// <summary>
        ///  get message data
        /// </summary>
        private const string DefectiveReason = "ProcessWorkDefectiveReason";

        #endregion

        #region Constructor

        /// <summary>
        /// constructor of the form
        /// </summary>
        public DefectiveReasonForm()
        {
            InitializeComponent();

        }
        #endregion

        #region PrivateMethods

        /// <summary>
        /// Fills all user records to gridview control
        /// </summary>
        private void GridBind(DefectiveReasonVo conditionInVo)
        {
            DefectiveReason_dgv.DataSource = null;


            try
            {
                DefectiveReasonVo outVo = (DefectiveReasonVo)base.InvokeCbm(new GetDefectiveReasonMasterMntCbm(), conditionInVo, false);

                DefectiveReason_dgv.AutoGenerateColumns = false;

                BindingSource bindingSource1 = new BindingSource(outVo.DefectiveReasonListVo, null);

                if (bindingSource1 != null && bindingSource1.Count > 0)
                {
                    DefectiveReason_dgv.DataSource = bindingSource1;
                }
                else
                {
                    messageData = new MessageData("mmci00006", Properties.Resources.mmci00006, null); //"defective reason"
                    logger.Info(messageData);
                    popUpMessage.Information(messageData, Text);
                }

                DefectiveReason_dgv.ClearSelection();

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
        private DefectiveReasonVo FormConditionVo()
        {
            DefectiveReasonVo inVo = new DefectiveReasonVo();

            if (DefectiveReasonCode_txt.Text != string.Empty)
            {
                inVo.DefectiveReasonCode = DefectiveReasonCode_txt.Text;
            }

            if (DefectiveReasonName_txt.Text != string.Empty)
            {
                inVo.DefectiveReasonName = DefectiveReasonName_txt.Text;

            }
            if (DefectiveCategory_cmb.SelectedIndex > -1)
            {
                inVo.DefectiveCategoryId = Convert.ToInt32(DefectiveCategory_cmb.SelectedValue);
            }

            return inVo;

        }

        /// <summary>
        /// selects user record for updation and show user form
        /// </summary>
        private void BindUpdateUserData()
        {
            int selectedrowindex = DefectiveReason_dgv.SelectedCells[0].RowIndex;

            DefectiveReasonVo selectedData = (DefectiveReasonVo)DefectiveReason_dgv.Rows[selectedrowindex].DataBoundItem;

            AddDefectiveReasonForm newAddForm = new AddDefectiveReasonForm(CommonConstants.MODE_UPDATE, selectedData);

            newAddForm.ShowDialog(this);

            if (newAddForm.IntSuccess > 0)
            {
                messageData = new MessageData("mmci00002", Properties.Resources.mmci00002, null);
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
        /// To idenctify the relation ship with tables
        /// </summary>
        private DefectiveReasonVo CheckDefectiveRelation(DefectiveReasonVo inVo)
        {
            DefectiveReasonVo outVo = new DefectiveReasonVo();

            try
            {
                outVo = (DefectiveReasonVo)base.InvokeCbm(new CheckDefectiveReasonRelationCbm(), inVo, false);
            }
            catch (Framework.ApplicationException exception)
            {
                popUpMessage.ApplicationError(exception.GetMessageData(), Text);
                logger.Error(exception.GetMessageData());
            }

            return outVo;
        }
        #endregion

        #region FormEvents
        /// <summary>
        /// form loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefectiveReasonForm_Load(object sender, EventArgs e)
        {
            FormDatatableFromVo();
            ComboBind(DefectiveCategory_cmb, defectiveCategoryDatatable, "name", "id");
            Update_btn.Enabled = Delete_btn.Enabled = false;
            DefectiveReasonCode_txt.Select();
        }
        #endregion
        private void FormDatatableFromVo()
        {
            defectiveCategoryDatatable = new DataTable();
            defectiveCategoryDatatable.Columns.Add("id");
            defectiveCategoryDatatable.Columns.Add("name");

            try
            {
                DefectiveCategoryVo defectiveCategoryOutVo = (DefectiveCategoryVo)base.InvokeCbm(new GetDefectiveCategoryMasterMntCbm(), new DefectiveCategoryVo(), false);

                foreach (DefectiveCategoryVo defectiveCategory in defectiveCategoryOutVo.DefectiveCategoryListVo)
                {
                    defectiveCategoryDatatable.Rows.Add(defectiveCategory.DefectiveCategoryId, defectiveCategory.DefectiveCategoryName);
                }
            }
            catch (Framework.ApplicationException exception)
            {
                popUpMessage.ApplicationError(exception.GetMessageData(), Text);
                logger.Error(exception.GetMessageData());
            }
        }
        private void ComboBind(ComboBox pCombo, DataTable pDatasource, string pDisplay, string pValue)
        {
            pCombo.DataSource = pDatasource;
            pCombo.DisplayMember = pDisplay;
            pCombo.ValueMember = pValue;
            pCombo.SelectedIndex = -1;
            pCombo.Text = string.Empty;
        }

        #region ButtonClick

        /// <summary>
        /// event to clear the controls of search criteria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_btn_Click(object sender, EventArgs e)
        {
            DefectiveReasonCode_txt.Text = string.Empty;
            DefectiveReasonName_txt.Text = string.Empty;
            DefectiveCategory_cmb.SelectedIndex = -1;
            DefectiveReason_dgv.DataSource = null;

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
            AddDefectiveReasonForm newAddForm = new AddDefectiveReasonForm(CommonConstants.MODE_ADD, null);

            newAddForm.ShowDialog();

            if (newAddForm.IntSuccess > 0)
            {
                messageData = new MessageData("mmci00001", Properties.Resources.mmci00001, null);
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
            int selectedrowindex = DefectiveReason_dgv.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = DefectiveReason_dgv.Rows[selectedrowindex];

            messageData = new MessageData("mmcc00004", Properties.Resources.mmcc00004, selectedRow.Cells["colDefectiveReasonCode"].Value.ToString());
            logger.Info(messageData);
            DialogResult dialogResult = popUpMessage.ConfirmationOkCancel(messageData, Text);

            if (dialogResult == DialogResult.OK)
            {
                DefectiveReasonVo inVo = new DefectiveReasonVo();

                inVo.DefectiveReasonId = Convert.ToInt32(selectedRow.Cells["colDefectiveReasonId"].Value);

                try
                {
                    inVo.DefectiveReasonCode = selectedRow.Cells["colDefectiveReasonCode"].Value.ToString();
                    DefectiveReasonVo tableCount = CheckDefectiveRelation(inVo);
                    if (tableCount.AffectedCount > 0)
                    {
                        messageData = new MessageData("mmce00007", Properties.Resources.mmce00007, DefectiveReason.ToString());
                        popUpMessage.Information(messageData, Text);
                        return;
                    }


                    DefectiveReasonVo outVo = (DefectiveReasonVo)base.InvokeCbm(new DeleteDefectiveReasonMasterMntCbm(), inVo, false);

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
        private void DefectiveReason_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DefectiveReason_dgv.SelectedRows.Count > 0)
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
        private void DefectiveReason_dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DefectiveReason_dgv.SelectedRows.Count > 0)
            {
                BindUpdateUserData();
            }
        }

        #endregion

    }
}
