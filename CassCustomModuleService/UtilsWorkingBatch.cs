using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kofax.Capture.SDK.Data;
using Kofax.Capture.SDK.CustomModule;
using Kofax.Capture.DBLite;

using log4net;

namespace CassCustomModuleService
{
    class UtilsWorkingBatch
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IACDataElementCollection getIndexFieldsCollection(IBatch workingbatch)
        {
            try
            {
                //now that we have the next batch, get Document collection, loop through that and set document classes
                int hints = 0;
                //Get Batch Element
                log.Debug("Adquiriendo elemento Batch");
                IACDataElement oBatchElement = workingbatch.ExtractRuntimeACDataElement(hints).FindChildElementByName("Batch");
                log.Debug("Adquirido eLemento " + oBatchElement.ElementName);
                log.Debug("Adquiriendo Elemento Documents");
                //Get Element Document
                IACDataElement oDocuments = oBatchElement.FindChildElementByName("Documents");
                log.Debug("Adquirido elemento " + oDocuments.ElementName);
                //Get Collection of Document in Documents
                log.Debug("Adquiriendo colección de elementos");
                IACDataElementCollection oDocCol = oDocuments.FindChildElementsByName("Document");
                log.Debug("Adquirida collección con " + oDocCol.Count);

                return oDocCol;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
            return null;
        }
    }
}
