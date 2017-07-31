namespace CassCustomModuleService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.CassCustomModuleServiceInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.CassCustomModuleInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // CassCustomModuleServiceInstaller
            // 
            this.CassCustomModuleServiceInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.CassCustomModuleServiceInstaller.Password = null;
            this.CassCustomModuleServiceInstaller.Username = null;
            // 
            // CassCustomModuleInstaller
            // 
            this.CassCustomModuleInstaller.Description = "CassCustomModuleInstaller";
            this.CassCustomModuleInstaller.DisplayName = "CassCustomModuleInstaller";
            this.CassCustomModuleInstaller.ServiceName = "Service1";
            this.CassCustomModuleInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.CassCustomModuleServiceInstaller,
            this.CassCustomModuleInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller CassCustomModuleServiceInstaller;
        private System.ServiceProcess.ServiceInstaller CassCustomModuleInstaller;
    }
}