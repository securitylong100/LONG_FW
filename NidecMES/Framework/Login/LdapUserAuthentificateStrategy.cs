using System;
using System.Net;
using System.DirectoryServices.Protocols;


namespace Com.Nidec.Mes.Framework
{
    internal class LdapUserAuthentificateStrategy : UserAuthentificateStrategy
    {

        private static CommonLogger logger = CommonLogger.GetInstance(typeof(LdapUserAuthentificateStrategy));


        /// <summary>
        /// ldap authentication
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public Boolean Authentificate(string user, string pass)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                return false;
            }

            string ldapConnectionHost = ConfigurationDataTypeEnum.LDAP_CONNECTION_HOST.GetValue(); //Properties.Settings.Default.LdapConnectionHost;
            string searchDN = "dc=nidec,dc=com";
            string searchFilterFormat = "(&(exgEnabledFlag=enabled)(uid={0}))";
            string distinguishedName = string.Empty;

            // search DistinguishedName
            LdapConnection ldapconnection = new LdapConnection(ldapConnectionHost);
            ldapconnection.AuthType = AuthType.Anonymous;
            ldapconnection.SessionOptions.ProtocolVersion = 3;
            ldapconnection.Bind();

            SearchRequest searchRequest = new SearchRequest(
                                                 searchDN,
                                                 string.Format(searchFilterFormat, user),
                                                 SearchScope.Subtree
                                                   );
            SearchResponse response = null;
            try
            {
                response = (SearchResponse)ldapconnection.SendRequest(searchRequest);
            }
            catch (ObjectDisposedException ex)
            {
                MessageData messageData = new MessageData("llce00007", Properties.Resources.llce00001, ex.Message);
                logger.Info(messageData, ex);
                SystemException sysEx = new SystemException(messageData, ex);

                throw sysEx;
            }
            catch (ArgumentNullException ex)
            {
                MessageData messageData = new MessageData("llce00007", Properties.Resources.llce00001, ex.Message);
                logger.Info(messageData, ex);
                SystemException sysEx = new SystemException(messageData, ex);

                throw sysEx;
            }
            catch (NotSupportedException ex)
            {
                MessageData messageData = new MessageData("llce00007", Properties.Resources.llce00001, ex.Message);
                logger.Info(messageData, ex);
                SystemException sysEx = new SystemException(messageData, ex);

                throw sysEx;
            }
            catch (LdapException ex)
            {
                MessageData messageData = new MessageData("llce00007", Properties.Resources.llce00001, ex.Message);
                logger.Info(messageData, ex);
                SystemException sysEx = new SystemException(messageData, ex);

                throw sysEx;
            }
            catch (DirectoryOperationException ex)
            {
                MessageData messageData = new MessageData("llce00007", Properties.Resources.llce00001, ex.Message);
                logger.Info(messageData, ex);
                SystemException sysEx = new SystemException(messageData, ex);

                throw sysEx;
            }


            if (response != null)
            {
                SearchResultEntryCollection collection = response.Entries;

                foreach (SearchResultEntry searchResultEntry in collection)
                {
                    distinguishedName = searchResultEntry.DistinguishedName;
                    break;
                }
            }

            // authenticate
            if (string.IsNullOrEmpty(distinguishedName))
            {
                return false;// user does not exist.

            }

            ldapconnection.AuthType = AuthType.Basic;
            ldapconnection.SessionOptions.ProtocolVersion = 3;
            try
            {
                ldapconnection.Bind(new NetworkCredential(distinguishedName, pass));
            }
            catch (ObjectDisposedException ex)
            {
                MessageData messageData = new MessageData("llce00007", Properties.Resources.llce00001, ex.Message);
                logger.Info(messageData, ex);
                SystemException sysEx = new SystemException(messageData, ex);

                throw sysEx;
            }
            catch (LdapException ex)
            {
                MessageData messageData = new MessageData("llce00007", Properties.Resources.llce00001, ex.Message);
                logger.Info(messageData, ex);
                SystemException sysEx = new SystemException(messageData, ex);

                throw sysEx;
            }
            catch (InvalidOperationException ex)
            {
                MessageData messageData = new MessageData("llce00007", Properties.Resources.llce00001, ex.Message);
                logger.Info(messageData, ex);
                SystemException sysEx = new SystemException(messageData, ex);

                throw sysEx;
            }
            return true;

        }
    }
}