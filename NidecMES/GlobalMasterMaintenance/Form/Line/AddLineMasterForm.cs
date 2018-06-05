using System;
using Com.Nidec.Mes.Framework;
using Com.Nidec.Mes.GlobalMasterMaintenance.Vo;
using Com.Nidec.Mes.GlobalMasterMaintenance.Cbm;
using System.Resources;

namespace Com.Nidec.Mes.GlobalMasterMaintenance
{
    public partial class AddLineMasterForm
    {

        #region Variables
        /// <summary>
        /// set mode based on insert/update
        /// </summary>
        private readonly string mode;

        /// <summary>
        /// user record selected for update
        /// </summary>
        private readonly LineVo updateData;

        /// <summary>
        /// Check for Database operation success
        /// </summary>
        public int IntSuccess = -1;

        /// <summary>
        /// initialize popupmessagecontroller
        /// </summary>
        private readonly PopUpMessageController popUpMessage = new PopUpMessageController();

        /// <summary>
        /// initialize CommonLogger
        /// </summary>
        private static readonly CommonLogger logger = CommonLogger.GetInstance(typeof(AddFactoryMasterForm));

        /// <summary>
        /// initialize message data
        /// </summary>
        private MessageData messageData;
        #endregion

        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="pmode"></param>
        /// <param name="LineItem"></param>
        public AddLineMasterForm(string pmode, LineVo LineItem = null)
        {
            InitializeComponent();

            mode = pmode;
           
            updateData = LineItem;
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
            if (LineCode_txt.Text == string.Empty)
            {
                messageData = new MessageData("mmce00002", Properties.Resources.mmce00002, LineCode_lbl.Text);
                popUpMessage.Warning(messageData, Text);

                LineCode_txt.Focus();

                return false;
            }

            if (LineName_txt.Text == string.Empty)
            {
                messageData = new MessageData("mmce00002", Properties.Resources.mmce00002, LineName_lbl.Text);
                popUpMessage.Warning(messageData, Text);

                LineName_txt.Focus();

                return false;
            }

            return true;
        }

        /// <summary>
        /// For setting selected Line record into respective controls(textbox and combobox) for update operation
        /// passing selected Line data as parameter 
        /// </summary>
        /// <param name="dgvLine"></param>
        private void LoadLineData(LineVo dgvLine)
        {
            if (dgvLine != null)
            {
                this.LineCode_txt.Text = dgvLine.LineCode;
                this.LineName_txt.Text = dgvLine.LineName;
            }
        }

        /// <summary>
        /// checks duplicate FactoryCode
        /// </summary>
        /// <param name="lineVo"></param>
        /// <returns></returns>
        private LineVo DuplicateCheck(LineVo lineVo)
        {
            LineVo outVo = new LineVo();

            try
            {
                outVo = (LineVo)base.InvokeCbm(new CheckLineMasterMntCbm(), lineVo, false);
            }
            catch (Framework.ApplicationException exception)
            {
                popUpMessage.ApplicationError(exception.GetMessageData(), Text);
                logger.Error(exception.GetMessageData());
            }

            return outVo;
        }

        #endregion

        #region FormEvent
        /// <summary>
        /// load screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLineMasterForm_Load(object sender, EventArgs e)
        {
            LineCode_txt.Select();
            if (string.Equals(mode, CommonConstants.MODE_UPDATE))
            {
                LoadLineData(updateData);

                LineCode_txt.Enabled = false;

                LineName_txt.Select();

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

            LineVo inVo = new LineVo();

            if (CheckMandatory())
            {

                var sch = StringCheckHelper.GetInstance();

                if (string.IsNullOrEmpty(LineCode_txt.Text) || string.IsNullOrEmpty(LineName_txt.Text))
                {
                    messageData = new MessageData("mmce00003", Properties.Resources.mmce00003);
                    logger.Info(messageData);
                    popUpMessage.ConfirmationOkCancel(messageData, Text);

                    if (string.IsNullOrEmpty(LineCode_txt.Text))
                    {
                        LineCode_txt.Focus();
                    }
                    else
                    {
                        LineName_txt.Focus();
                    }

                    return;
                }

                inVo.LineCode = LineCode_txt.Text.Trim();
                inVo.LineName = LineName_txt.Text.Trim();
                //inVo.RegistrationDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                inVo.RegistrationUserCode = UserData.GetUserData().UserCode;
                inVo.FactoryCode = UserData.GetUserData().FactoryCode;

                if (string.Equals(mode, CommonConstants.MODE_ADD))
                {
                    LineVo checkVo = DuplicateCheck(inVo);

                    if (checkVo != null && checkVo.AffectedCount > 0)
                    {
                        messageData = new MessageData("mmce00001", Properties.Resources.mmce00001, LineCode_lbl.Text + " : " + LineCode_txt.Text);
                        logger.Info(messageData);
                        popUpMessage.ApplicationError(messageData, Text);

                        return;
                    }
                }

                try
                {
                    if (string.Equals(mode, CommonConstants.MODE_ADD))
                    {
                        LineVo outVo = (LineVo)base.InvokeCbm(new AddLineMasterMntCbm(), inVo, false);
                        IntSuccess = outVo.AffectedCount;
                    }
                    else if (string.Equals(mode, CommonConstants.MODE_UPDATE))
                    {
                        inVo.LineId = updateData.LineId;
                        LineVo outVo = (LineVo)base.InvokeCbm(new UpdateLineMasterMntCbm(), inVo, false);
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
        /// Window close when Exit button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_btn_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

    }
}
