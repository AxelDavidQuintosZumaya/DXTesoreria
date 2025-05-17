using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesoreria.Module.BusinessObjects;

namespace Tesoreria.Module.Controllers
{

    public partial class BotonDatos : ViewController
    {
        public BotonDatos()
        {
            InitializeComponent();
            TargetObjectType = typeof(Datos);

            SimpleAction cloneAction = new SimpleAction(this, "ClonarDatos", PredefinedCategory.Edit)
            {
                Caption = "Clonar",
                ImageName = "Action_Copy"
            };

            cloneAction.Execute += CloneAction_Execute;
        }

        private void CloneAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();

            Tesoreria.Module.BusinessObjects.Datos datos = (BusinessObjects.Datos)e.CurrentObject;
            Datos datos1 = objectSpace.FindObject<Datos>(CriteriaOperator.Parse("[Oid] = ?", datos.Oid));

            var nuevoDatos = objectSpace.CreateObject<Datos>();

            nuevoDatos.Corte = datos1.Corte;
            nuevoDatos.User = datos1.User;
            nuevoDatos.Comites = datos1.Comites;
            //nuevoDatos.FechaOperacion = datos1.FechaOperacion;
            nuevoDatos.Mes = datos1.Mes;
            nuevoDatos.MesNumerico = datos1.MesNumerico;
            nuevoDatos.TipoOperacion = datos1.TipoOperacion;
            nuevoDatos.TipoMovimientosE = datos1.TipoMovimientosE;
            nuevoDatos.Cuenta = datos1.Cuenta;
            nuevoDatos.BancoRegistros = datos1.BancoRegistros;
            nuevoDatos.NombreCtaRegistro = datos1.NombreCtaRegistro;
            nuevoDatos.EmisorProveedorBeneficiarios = datos1.EmisorProveedorBeneficiarios;
            nuevoDatos.BancoReceptorS = datos1.BancoReceptorS;
            nuevoDatos.Cuenta = datos1.Cuenta;
            nuevoDatos.FolioInterno = datos1.FolioInterno;
            nuevoDatos.FolioIdentificadorOficio = datos1.FolioIdentificadorOficio;
            nuevoDatos.FechaOficio = datos1.FechaOficio;
            nuevoDatos.FolioFiscal = datos1.FolioFiscal;
            nuevoDatos.NoFacturaDocumentoComprobatorio = datos1.NoFacturaDocumentoComprobatorio;
            nuevoDatos.Concepto = datos1.Concepto;
            nuevoDatos.ConceptoPortal = datos1.ConceptoPortal;
            nuevoDatos.ClaveRastreoReferencia = datos1.ClaveRastreoReferencia;
            nuevoDatos.ImporteCargo = datos1.ImporteCargo;
            nuevoDatos.ImporteAbono = datos1.ImporteAbono;
            nuevoDatos.Area = datos1.Area;
            nuevoDatos.Status = datos1.Status;
            nuevoDatos.Observaciones = datos1.Observaciones;
            nuevoDatos.Banco = datos1.Banco;
            nuevoDatos.TipoGasto = datos1.TipoGasto;
            nuevoDatos.Nomenclatura = datos1.Nomenclatura;
            nuevoDatos.NombreAlias = datos1.NombreAlias;
            nuevoDatos.Concatenar = datos1.Concatenar;

            objectSpace.CommitChanges();

            // Mostrar el nuevo objeto clonado en una nueva DetailView
            var detailView = Application.CreateDetailView(objectSpace, nuevoDatos);

            detailView.ViewEditMode = ViewEditMode.Edit; // Para permitir edición inmediatamente

            e.ShowViewParameters.CreatedView = detailView;
            e.ShowViewParameters.TargetWindow = TargetWindow.Current;
            e.ShowViewParameters.Context = TemplateContext.View;
            e.ShowViewParameters.CreateAllControllers = true;


        

        Application.ShowViewStrategy.ShowMessage(new MessageOptions
                {
                    Duration = 5000, // Duración en milisegundos (2 segundos)
                    Message = "¡Registro clonado exitosamente!",
                    Type = InformationType.Success,
                    Web = { Position = InformationPosition.Top
    } // En web se muestra arriba
});
            }

        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}