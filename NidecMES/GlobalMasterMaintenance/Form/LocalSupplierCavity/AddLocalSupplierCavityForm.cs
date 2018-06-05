using System;
using System.Data;
using System.Windows.Forms;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Cbm;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;
using System.Resources;

namespace Com.Nidec.Mes.GlobalMasterMaintenance
{
    public partial class AddLocalSupplierCavityForm
    {

        #region Variables

        /// <summary>
        /// set mode based on insert/update
        /// </summary>
        private readonly string mode;

        /// <summary>
        /// user record selected for update
        /// </summary>
        private readonly LocalSupplierCavityVo updateData;

        /// <summary>
        /// Check for Database operation success
        /// </summary>
        public int IntSuccess = -1;


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
        private static readonly CommonLogger logger = CommonLogger.GetInstance(typeof(AddLocalSupplierCavityForm));

        /// <summary>
        /// initialize message data
        /// </summary>
        private MessageData messageData;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the  form
        /// </summary>
        /// <param name="pmode"></param>
        /// <param name="userItem"></param>
        public AddLocalSupplierCavityForm(string pmode, LocalSupplierCavityVo userItem = null)
        {
            InitializeComponent();

            mode = pmode;
         
            updateData = userItem;
            if (string.Equals(mode, CommonConstants.MODE_UPDATE))
            {
                this.Text = UpdateText_lbl.Text;
            }
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Checks mandatory fields
        /// </summary>
        /// <returns></returns>
        private bool CheckMandatory()
        {
            if (LocalSupplierCavityCode_txt.Text == string.Empty)
            {
                messageData = new MessageData("mmce00002", Properties.Resources.mmce00002, LocalSupplierCavityCode_lbl.Text);
                popUpMessage.Warning(messageData, Text);

                LocalSupplierCavityCode_txt.Focus();

                return false;
            }

            if (LocalSupplierCavityName_txt.Text == string.Empty)
            {
                messageData = new MessageData("mmce00002", Properties.Resources.mmce00002, LocalSupplierCavityName_lbl.Text);
                popUpMessage.Warning(messageData, Text);

                LocalSupplierCavityName_txt.Focus();

                return false;
            }
            
            if (LocalSupplier_cmb.Text == string.Empty || LocalSupplier_cmb.SelectedIndex < 0)
            {
                messageData = new MessageData("mmce00002", Properties.Resources.mmce00002, LocalSupplier_lbl.Text);
                popUpMessage.Warning(messageData, Text);

                LocalSupplier_cmb.Focus();

                return false;
            }

            return true;
        }


        /// <summary>
        /// For binding item
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
        /// For setting selected user record into respective controls(textbox and combobox) for update operation
        /// passing selected user data as parameter 
        /// </summary>
        /// <param name="dgvMoldType"></param>
        private void LoadUserData(LocalSupplierCavityVo dgvLocalSupplierCavity)
        {
            if (dgvLocalSupplierCavity != null)
            {
                this.LocalSupplierCavityCode_txt.Text = dgvLocalSupplierCavity.LocalSupplierCavityCode;

                this.LocalSupplierCavityName_txt.Text = dgvLocalSupplierCavity.LocalSupplierCavityName;

                this.LocalSupplier_cmb.SelectedValue = dgvLocalSupplierCavity.LocalSupplierId.ToString();

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
                LocalSupplierVo localSupplierOutVo = (LocalSupplierVo)base.InvokeCbm(new GetLocalSupplierMasterMntCbm(), new LocalSupplierVo(), false);

                foreach (LocalSupplierVo localSupplierVo in localSupplierOutVo.LocalSupplierListVo)
                {
                    localSupplierDatatable.Rows.Add(localSupplierVo.LocalSupplierId, localSupplierVo.LocalSupplierCode + ":" + localSupplierVo.LocalSupplierName);
                }
            }
            catch (Framework.ApplicationException exception)
            {
                popUpMessage.ApplicationError(exception.GetMessageData(), Text);
                logger.Error(exception.GetMessageData());
            }
        }

        /// <summary>
        /// checks duplicate mold Code
        /// </summary>
        /// <param name="moldVo"></param>
        /// <returns></returns>
        private LocalSupplierCavityVo DuplicateCheck(LocalSupplierCavityVo localSupplierCavityVo)
        {
            LocalSupplierCavityVo outVo = new LocalSupplierCavityVo();

            try
            {
                outVo = (LocalSupplierCavityVo)base.InvokeCbm(new CheckLocalSupplierCavityMasterMntCbm(), localSupplierCavityVo, false);
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
        /// Handles Load event for mold data Insert/Update operations 
        /// Loading mold data for update mold data and binding controls with selected mold record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLocalSupplierCavityForm_Load(object sender, EventArgs e)
        {
            FormDatatableFromVo();

            ComboBind(LocalSupplier_cmb, localSupplierDatatable, "code", "id");

            LocalSupplierCavityCode_txt.Select();

            if (string.Equals(mode, CommonConstants.MODE_UPDATE))
            {
                LoadUserData(updateData);

                LocalSupplierCavityCode_txt.Enabled = false;

                LocalSupplierCavityName_txt.Select();

            }
        }
        #endregion

        #region ButtonClick

        /// <summary>
        /// update data to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_btn_Click(object sender, EventArgs e)
        {

            if (CheckMandatory())
            {
                var sch = StringCheckHelper.GetInstance();

                if (string.IsNullOrEmpty(LocalSupplierCavityCode_txt.Text) || string.IsNullOrEmpty(LocalSupplierCavityName_txt.Text))
                {
                    messageData = new MessageData("mmce00003", Properties.Resources.mmce00003);
                    logger.Info(messageData);
                    popUpMessage.ConfirmationOkCancel(messageData, Text);

                    if (string.IsNullOrEmpty(LocalSupplierCavityCode_txt.Text))
                    {
                        LocalSupplierCavityCode_txt.Focus();
                    }
                    else
                    {
                        LocalSupplierCavityName_txt.Focus();
                    }

                    return;
                }

                LocalSupplierCavityVo inVo = new LocalSupplierCavityVo
                {
                    LocalSupplierCavityCode = LocalSupplierCavityCode_txt.Text.Trim(),
                    LocalSupplierCavityName = LocalSupplierCavityName_txt.Text.Trim(),
                    LocalSupplierId = Convert.ToInt32(LocalSupplier_cmb.SelectedValue),
                    //RegistrationDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    RegistrationUserCode = UserData.GetUserData().UserCode,
                    FactoryCode = UserData.GetUserData().FactoryCode
                };

                if (string.Equals(mode, CommonConstants.MODE_ADD))
                {
                    LocalSupplierCavityVo checkVo = DuplicateCheck(inVo);

                    if (checkVo != null && checkVo.AffectedCount > 0)
                    {
                        messageData = new MessageData("mmce00001", Properties.Resources.mmce00001, LocalSupplierCavityCode_lbl.Text + " : " + LocalSupplierCavityCode_txt.Text);
                        logger.Info(messageData);
                        popUpMessage.ApplicationError(messageData, Text);

                        return;
                    }
                }

                try
                {
                    if (string.Equals(mode, CommonConstants.MODE_ADD))
                    {
                        LocalSupplierCavityVo outVo = (LocalSupplierCavityVo)base.InvokeCbm(new AddLocalSupplierCavityMasterMntCbm(), inVo, false);

                        IntSuccess = outVo.AffectedCount;
                    }
                    else if (mode.Equals(CommonConstants.MODE_UPDATE))
                    {
                        inVo.LocalSupplierCavityId = updateData.LocalSupplierCavityId;

                        LocalSupplierCavityVo outVo = (LocalSupplierCavityVo)base.InvokeCbm(new UpdateLocalSupplierCavityMasterMntCbm(), inVo, false);

                        IntSuccess = outVo.AffectedCount;
                    }

                }
                catch (Framework.ApplicationException exception)
                {
                    popUpMessage.ApplicationError(exception.GetMessageData(), Text);
                    logger.Error(exception.GetMessageData());
                    return;
                }

                if ((IntSuccess > 0) || (IntSuccess == 0))
                {
                    this.Close();
                }

            }
        }

        /// <summary>
        /// close the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

    }
}
