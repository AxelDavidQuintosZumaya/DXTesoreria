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
    public class Corte : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Corte(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private DateTime _Fecha;
        [XafDisplayName("Fecha"), ToolTip("My hint message")]
        [Persistent("Fecha")]
        public DateTime Fecha
        {
            get { return _Fecha; }
            set { SetPropertyValue(nameof(Fecha), ref _Fecha, value); }
        }

        private decimal _Monto;
        [XafDisplayName("Monto"), ToolTip("My hint message")]
        [Persistent("Monto")]
        public decimal Monto
        {
            get { return _Monto; }
            set { SetPropertyValue(nameof(Monto), ref _Monto, value); }
        }

        private Cuentas _Cuenta;
        [Association("Cuenta-Corte")]

        [XafDisplayName("Cuenta de Registro")]
        public Cuentas Cuenta
        {
            get { return _Cuenta; }
            set { SetPropertyValue(nameof(Cuenta), ref _Cuenta, value); }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}