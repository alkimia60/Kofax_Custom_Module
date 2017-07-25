using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

using log4net;
using log4net.Config;

using System.Collections;

using Kofax.Capture.SDK.Data;
using Kofax.Capture.SDK.CustomModule;
using Kofax.Capture.DBLite;

[assembly: log4net.Config.XmlConfigurator(ConfigFile="App.config",Watch = true)]

namespace CassCustomModule
{
    class Program
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static IBatch workingbatch;
        private static IRuntimeSession m_oRuntimeSession;
        //private static Login m_oLogin;
        private static int processId;

        static void Main(string[] args)
        {
            UtilsKofax utilsKofax = new UtilsKofax();
            UtilsWorkingBatch utilsWorkingBatch = new UtilsWorkingBatch();
            UtilsQR utilsQR = new UtilsQR();
            try
            {
                m_oRuntimeSession = utilsKofax.getLogonKofax();
                //*** Get the Process ID
                processId = utilsKofax.getProcessID();
               
                KfxDbFilter dbfilter = KfxDbFilter.KfxDbFilterOnProcess;
                KfxDbState dbstate = KfxDbState.KfxDbBatchReady;

                log.Info("Adquiriendo lote en curso...");
                workingbatch = m_oRuntimeSession.NextBatchGet(
                                 processId,
                                 dbfilter,
                                 dbstate);
                //log.Info("lote en curso conseguido por el usuario: " + workingbatch.ScanUser);
                if (workingbatch != null)
                {
                    IACDataElementCollection oDocCol = utilsWorkingBatch.getIndexFieldsCollection(workingbatch);

                    //Iterate Collection Docs to Extract Collection of IndexFields
                    if (oDocCol != null & workingbatch.BatchClassName == "Receta") 
                    {
                        log.Info("Por cada elemento se buscará el indexField Barcode ");
                        foreach (IACDataElement oDoc in oDocCol)
                        {

                            log.Debug("Adquiriendo elemento IndexField del Documento " + oDoc["BatchDocGUID"]);
                            IACDataElement oFields = oDoc.FindChildElementByName("IndexFields");
                            log.Debug("Adquirido elemento IndexFields");
                            log.Debug("Adquiriendo elemento IndexField a través del atributo Name Barcode");
                            IACDataElement oField = oFields.FindChildElementByAttribute("IndexField", "Name", "Barcode");
                            log.Info("Se ha recuperado el valor " + oField["Value"] + " Del campo Barcode");

                        }
                    }
                    else if (oDocCol != null & workingbatch.BatchClassName == "FullCotitzacio") 
                    {
                        log.Info("Por cada elemento se buscará el indexField QRCass ");
                        foreach (IACDataElement oDoc in oDocCol)
                        {

                            log.Debug("Adquiriendo elemento IndexField del Documento " + oDoc["BatchDocGUID"]);
                            IACDataElement oFields = oDoc.FindChildElementByName("IndexFields");
                            log.Debug("Adquirido elemento IndexFields");
                            log.Debug("Adquiriendo elemento IndexField a través del atributo Name QRCass");
                            IACDataElement oField = oFields.FindChildElementByAttribute("IndexField", "Name", "QRCass");
                            String cleanQR = utilsQR.cleanQR(oField["Value"]);

                            //log.Info("Se ha recuperado el valor " + oField["Value"] + " Del campo QRCass");
                            log.Info("Se ha recuperado el valor " + cleanQR + " Del campo QRCass");

                        }

                    }
                    
                    workingbatch.BatchClose(Kofax.Capture.SDK.CustomModule.KfxDbState.KfxDbBatchReady, Kofax.Capture.SDK.CustomModule.KfxDbQueue.KfxDbQueueNext, 0, "");
                    log.Info("Se ha procesado correctamente el lote con nombre " + workingbatch.Name);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                log.Error(e.Message);
            }
        }
    }
}
