using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;


using log4net;
using log4net.Config;
using System.Timers;

using Kofax.Capture.SDK.CustomModule;
using Kofax.Capture.DBLite;
using Kofax.Capture.SDK.Data;


[assembly: log4net.Config.XmlConfigurator(ConfigFile = "App.config", Watch = true)]

namespace CassCustomModuleService
{
    public partial class Service1 : ServiceBase
    {
        private static IBatch workingbatch;
        private static IRuntimeSession m_oRuntimeSession;
        private static int processId;


        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private System.Timers.Timer aTimer;

        public Service1()
        {
            log.Info("Servicio iniciado");
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            aTimer = new System.Timers.Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            aTimer.Elapsed += new ElapsedEventHandler(CassCustomModule);
            aTimer.Enabled = true;
            log.Info("Servicio en Start");
            
        }

        protected override void OnStop()
        {
            log.Info("Servicio Detenido");
        }

        private void CassCustomModule(object sender, ElapsedEventArgs e)
        {
               //IBatch workingbatch;
               //IRuntimeSession m_oRuntimeSession;
               //int processId;


               UtilsKofax utilsKofax = new UtilsKofax();
               UtilsWorkingBatch utilsWorkingBatch = new UtilsWorkingBatch();
               UtilsQR utilsQR = new UtilsQR();
               try
               {
                   //Login in Kofax Capture
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
                               log.Info("Se ha recuperado el valor " + oField["Value"] + " Del campo Barcode ");

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
               catch (Exception er)
               {
                   log.Error(er.Message);
               }


        }
    }
}
