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
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
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

        private CuentaCuentaClabeLineaCap _CuentaCuentaClabeLineaCap;
        [Association("CuentaCuentaClabeLineaCap-EmisorProveedorBeneficiario")]

        [XafDisplayName("CuentaCuentaClabeLineaCap")]
        public CuentaCuentaClabeLineaCap CuentaCuentaClabeLineaCap
        {
            get { return _CuentaCuentaClabeLineaCap; }
            set { SetPropertyValue(nameof(CuentaCuentaClabeLineaCap), ref _CuentaCuentaClabeLineaCap, value); }
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