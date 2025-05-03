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
    public class Clabe : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Clabe(Session session)
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
                var duplicado = Session.Query<Clabe>()
                    .Where(x => x.Numeros == this.Numeros && x.Oid != this.Oid)
                    .FirstOrDefault();

                if (duplicado != null)
                {
                    throw new UserFriendlyException("Ya existe un registro con la misma clabe interbancaria.");
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
//private string _Titular;
//[XafDisplayName("Titular"), ToolTip("My hint message")]
//[Persistent("Titular"), RuleRequiredField(DefaultContexts.Save)]
//public string Titular
//{
//    get { return _Titular; }
//    set { SetPropertyValue(nameof(Titular), ref _Titular, value); }
//}

//private string _Numero;
//[XafDisplayName("Número")]
//public string Numero
//{
//    get { return _Numero; }
//    set
//    {
//        if (SetPropertyValue(nameof(Numero), ref _Numero, value))
//        {
//            // Actualizar Digitos automáticamente
//            Digitos = string.IsNullOrEmpty(value) || value.Length < 5
//                ? value
//                : value.Substring(value.Length - 5);
//        }
//    }
//}

//private string _Digitos;
//[XafDisplayName("Digitos"), ToolTip("My hint message")]
//[Persistent("Digitos"), RuleRequiredField(DefaultContexts.Save)]
//public string Digitos
//{
//    get { return _Digitos; }
//    set { SetPropertyValue(nameof(Digitos), ref _Digitos, value); }
//}

//private Bancos _Bancos;
//[Association("Banco-Cuentas")]

//[XafDisplayName("Banco")]
//public Bancos Bancos
//{
//    get { return _Bancos; }
//    set { SetPropertyValue(nameof(Bancos), ref _Bancos, value); }
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

        private string _NombreCompleto;
        [XafDisplayName("NombreCompleto"), ToolTip("My hint message")]
        [Persistent("NombreCompleto"), RuleRequiredField(DefaultContexts.Save)]
        public string NombreCompleto
        {
            get { return _NombreCompleto; }
            set { SetPropertyValue(nameof(NombreCompleto), ref _NombreCompleto, value); }
        }

        [Association("Clabe-EmisorProveedorBeneficiario")]
        public XPCollection<EmisorProveedorBeneficiario> EmisorProveedorBenefisiarios
        {
            get { return GetCollection<EmisorProveedorBeneficiario>("EmisorProveedorBenefisiarios"); }
        }

        [Association("Clabe-Datos")]
        public XPCollection<Datos> Datos
        {
            get { return GetCollection<Datos>("Datos"); }
        }
    }
}