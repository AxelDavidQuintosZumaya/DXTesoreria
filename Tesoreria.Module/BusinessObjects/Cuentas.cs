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
    [DefaultProperty("Numero")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Cuentas : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Cuentas(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _Titular;
        [XafDisplayName("Titular"), ToolTip("My hint message")]
        [Persistent("Titular"), RuleRequiredField(DefaultContexts.Save)]
        public string Titular
        {
            get { return _Titular; }
            set { SetPropertyValue(nameof(Titular), ref _Titular, value); }
        }

        private string _Numero;
        [XafDisplayName("Número")]
        public string Numero
        {
            get { return _Numero; }
            set
            {
                if (SetPropertyValue(nameof(Numero), ref _Numero, value))
                {
                    // Actualizar Digitos automáticamente
                    Digitos = string.IsNullOrEmpty(value) || value.Length < 5
                        ? value
                        : value.Substring(value.Length - 5);
                }
            }
        }

        private string _Digitos;
        [XafDisplayName("Digitos"), ToolTip("My hint message")]
        [Persistent("Digitos"), RuleRequiredField(DefaultContexts.Save)]
        public string Digitos
        {
            get { return _Digitos; }
            set { SetPropertyValue(nameof(Digitos), ref _Digitos, value); }
        }

        private Bancos _Bancos;
        [Association("Banco-Cuentas")]

        [XafDisplayName("Banco")]
        public Bancos Bancos
        {
            get { return _Bancos; }
            set { SetPropertyValue(nameof(Bancos), ref _Bancos, value); }
        }

        [Association("Cuenta-Datos")]
        public XPCollection<Datos> Datos
        {
            get { return GetCollection<Datos>("Datos"); }
        }

        [Association("Cuenta-Corte")]
        public XPCollection<Corte> Corte
        {
            get { return GetCollection<Corte>("Corte"); }
        }

        private Comites _Comites;
        [Association("Comites-Cuentas")]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Comites")]
        public Comites Comites
        {
            get { return _Comites; }
            set { SetPropertyValue(nameof(Comites), ref _Comites, value); }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}