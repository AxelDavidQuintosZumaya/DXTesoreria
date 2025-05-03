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
    [DefaultProperty("Numeros")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CuentaExtrena : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public CuentaExtrena(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!string.IsNullOrWhiteSpace(Numeros))
            {
                var duplicado = Session.Query<CuentaExtrena>()
                    .Where(x => x.Numeros == this.Numeros && x.Oid != this.Oid)
                    .FirstOrDefault();

                if (duplicado != null)
                {
                    throw new UserFriendlyException("Ya existe un registro con el mismo numero de cuenta.");
                }
            }
        }

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
        private string _Titular;
        [XafDisplayName("Titular"), ToolTip("My hint message")]
        [Persistent("Titular"), RuleRequiredField(DefaultContexts.Save)]
        public string Titular
        {
            get { return _Titular; }
            set { SetPropertyValue(nameof(Titular), ref _Titular, value); }
        }

        private string _Numeros;
        [XafDisplayName("Numeros"), ToolTip("My hint message")]
        [Persistent("Numeros"), RuleRequiredField(DefaultContexts.Save)]
        public string Numeros
        {
            get { return _Numeros; }
            set { SetPropertyValue(nameof(Numeros), ref _Numeros, value); }
        }

        [Association("CuentaExterna-EmisorProveedorBeneficiario")]
        public XPCollection<EmisorProveedorBeneficiario> EmisorProveedorBenefisiarios
        {
            get { return GetCollection<EmisorProveedorBeneficiario>("EmisorProveedorBenefisiarios"); }
        }

        [Association("CuentaExterna-Datos")]
        public XPCollection<Datos> Datos
        {
            get { return GetCollection<Datos>("Datos"); }
        }
    }
}