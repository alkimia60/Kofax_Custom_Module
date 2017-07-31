using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using log4net;
using log4net.Config;

//using System.Runtime.InteropServices;
//using System.Collections;

using Kofax.Capture.SDK.Data;
using Kofax.Capture.SDK.CustomModule;
using Kofax.Capture.DBLite;


namespace CassCustomModuleService
{
    class UtilsKofax
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Login m_oLogin;

        public IRuntimeSession getLogonKofax()
        {
            try
            {
                //*** Initialize the login object
                if (m_oLogin == null)
                {
                    m_oLogin = new Login();
                    m_oLogin.Login("Admin", "");
                    m_oLogin.ApplicationName = "Kofax.CassCustomModuleService";
                    m_oLogin.Version = "1.0";
                }
                log.Debug("m_oLogin realizado correctamente");
                //*** Validate the user with the module UniqueID
                m_oLogin.ValidateUser(m_oLogin.ApplicationName, true, "", "");

            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
            return m_oLogin.RuntimeSession;
        }
        public int getProcessID()
        {
            int processId = m_oLogin.ProcessID;

            return processId;
        }
    }
}
