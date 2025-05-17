using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Tesoreria.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("CuentaExterna")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EmisorProveedorBeneficiario : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public EmisorProveedorBeneficiario(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _Nombre;
        [XafDisplayName("Nombre"), ToolTip("My hint message")]
        [Persistent("Nombre"), RuleRequiredField(DefaultContexts.Save)]
        public string Nombre
        {
            get { return _Nombre; }
            set { SetPropertyValue(nameof(Nombre), ref _Nombre, value); }
        }

        private Bancos _BancoReceptor;
        [Association("BancoReceptor-EmisorProveedorBeneficiario")]

        [XafDisplayName("BancoReceptor")]
        public Bancos BancoReceptor
        {
            get { return _BancoReceptor; }
            set { SetPropertyValue(nameof(BancoReceptor), ref _BancoReceptor, value); }
        }

        private CuentaExtrena _CuentaExterna;
        [Association("CuentaExterna-EmisorProveedorBeneficiario")]

        [XafDisplayName("CuentaExterna")]
        public CuentaExtrena CuentaExterna
        {
            get { return _CuentaExterna; }
            set { SetPropertyValue(nameof(CuentaExterna), ref _CuentaExterna, value); }
        }

        private string _ClabeProveedor;
        [XafDisplayName("Clabe del proveedor"), ToolTip("My hint message")]
        [ModelDefault("AllowEdit", "False")]
        [Persistent("ClabeProveedor")]
        public string ClabeProveedor
        {
            get { return _ClabeProveedor; }
            set { SetPropertyValue(nameof(ClabeProveedor), ref _ClabeProveedor, value); }
        }



        [Association("EmisorProveedorBeneficiario-Datos")]
        public XPCollection<Datos> Datos
        {
            get { return GetCollection<Datos>("Datos"); }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}