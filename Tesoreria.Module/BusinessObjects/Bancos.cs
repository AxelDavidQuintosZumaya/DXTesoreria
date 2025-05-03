using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
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
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Bancos : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Bancos(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            this.Concatenacion = Nombre + "-" + ClaveBanco;
        }

        private string _Nombre;
        [XafDisplayName("Nombre"), ToolTip("My hint message")]
        [Persistent("Nombre"), RuleRequiredField(DefaultContexts.Save)]
        public string Nombre
        {
            get { return _Nombre; }
            set { SetPropertyValue(nameof(Nombre), ref _Nombre, value); }
        }

        private string _ClaveBanco;
        [XafDisplayName("ClaveBanco"), ToolTip("My hint message")]
        [Persistent("ClaveBanco"), RuleRequiredField(DefaultContexts.Save)]
        public string ClaveBanco
        {
            get { return _ClaveBanco; }
            set { SetPropertyValue(nameof(ClaveBanco), ref _ClaveBanco, value); }
        }

        private string _NombreRazonSocial;
        [XafDisplayName("NombreRazonSocial"), ToolTip("My hint message")]
        [Persistent("NombreRazonSocial"), RuleRequiredField(DefaultContexts.Save)]
        public string NombreRazonSocial
        {
            get { return _NombreRazonSocial; }
            set { SetPropertyValue(nameof(NombreRazonSocial), ref _NombreRazonSocial, value); }
        }

        private string _Concatenacion;
        [XafDisplayName("Concatenacion"), ToolTip("My hint message")]
        [Persistent("Concatenacion"), RuleRequiredField(DefaultContexts.Save)]
        public string Concatenacion
        {
            get { return _Concatenacion; }
            set { SetPropertyValue(nameof(Concatenacion), ref _Concatenacion, value); }
        }

        private bool _CuentaPartido;

        [XafDisplayName("CuentaPartido")]
        [ToolTip("My hint message")]
        [Persistent("CuentaPartido")]
        public bool CuentaPartido
        {
            get { return _CuentaPartido; }
            set { SetPropertyValue(nameof(CuentaPartido), ref _CuentaPartido, value); }
        }

        [Association("Banco-Cuentas")]
        public XPCollection<Cuentas> Cuentas
        {
            get { return GetCollection<Cuentas>("Cuentas"); }
        }

        [Association("BancoRegistro-Datos")]
        public XPCollection<Datos> Datos
        {
            get { return GetCollection<Datos>("Datos"); }
        }

        [Association("BancoReceptor-Datos")]
        public XPCollection<Datos> Datos2
        {
            get { return GetCollection<Datos>("Datos2"); }
        }

        [Association("BancoReceptor-EmisorProveedorBeneficiario")]
        public XPCollection<EmisorProveedorBeneficiario> EmisorProveedorBeneficiario
        {
            get { return GetCollection<EmisorProveedorBeneficiario>("EmisorProveedorBeneficiario"); }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}