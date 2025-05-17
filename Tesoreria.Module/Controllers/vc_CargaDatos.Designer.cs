namespace Tesoreria.Module.Controllers
{
    partial class vc_CargaDatos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            var popupCargarHojaDatos = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);

            popupCargarHojaDatos.AcceptButtonCaption = "Subir";
            popupCargarHojaDatos.CancelButtonCaption = null;
            popupCargarHojaDatos.Caption = "Subir Excel de Bitacora";
            popupCargarHojaDatos.ConfirmationMessage = null;
            popupCargarHojaDatos.Id = "SubirHojaRegistroEntradas_VC";
            popupCargarHojaDatos.TargetObjectType = typeof(Module.BusinessObjects.Datos);
            popupCargarHojaDatos.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            popupCargarHojaDatos.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            popupCargarHojaDatos.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupCargarHojaDatos_CustomizePopupWindowParams);
            popupCargarHojaDatos.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupCargarHojaDatos_Execute);

            this.Actions.Add(popupCargarHojaDatos);
            this.TargetObjectType = typeof(Module.BusinessObjects.Datos);
        }

        #endregion
    }
}
