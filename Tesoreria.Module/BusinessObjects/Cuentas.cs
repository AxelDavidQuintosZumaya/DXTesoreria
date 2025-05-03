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

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

            if (Comites != null && !string.IsNullOrEmpty(Comites.Nombre))
            {
                if (!string.IsNullOrEmpty(Nombre))
                {
                    this.NombreCompleto = Comites.Nombre.Substring(3) + " " + Nombre;
                }
                else
                {
                    this.NombreCompleto = Comites.Nombre.Substring(3);
                }
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!string.IsNullOrWhiteSpace(Cuenta))
            {
                var duplicado = Session.Query<Cuentas>()
                    .Where(x => x.Cuenta == this.Cuenta && x.Oid != this.Oid)
                    .FirstOrDefault();

                if (duplicado != null)
                {
                    throw new UserFriendlyException("Ya existe un registro con el mismo numero de cuenta.");
                }
            }
        }




        private Usuarios _User;
        [Association("Usuarios-Cuentas")]

        [XafDisplayName("Usuarios")]
        public Usuarios User
        {
            get { return _User; }
            set { SetPropertyValue(nameof(User), ref _User, value); }
        }

        private string _Nombre;
        [XafDisplayName("Nombre"), ToolTip("My hint message")]
        [Persistent("Nombre"), RuleRequiredField(DefaultContexts.Save)]
        public string Nombre
        {
            get { return _Nombre; }
            set { SetPropertyValue(nameof(Nombre), ref _Nombre, value); }
        }

        private string _Cuenta;
        [XafDisplayName("Cuenta")]
        public string Cuenta
        {
            get { return _Cuenta; }
            set
            {
                if (SetPropertyValue(nameof(Cuenta), ref _Cuenta, value))
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
        [DataSourceCriteria("CuentaPartido = true")]
        [Association("Banco-Cuentas")]

        [XafDisplayName("Banco")]
        public Bancos Bancos
        {
            get { return _Bancos; }
            set { SetPropertyValue(nameof(Bancos), ref _Bancos, value); }
        }

        public enum TipoCuentaEnum
        {
            [XafDisplayName("Sin Tipo")]
            SinTipo,

            [XafDisplayName("Gasto Programado")]
            GastoProgramado,

            [XafDisplayName("Ordinario")]
            Ordinario
        }


        private TipoCuentaEnum _TipoCuenta;

        [XafDisplayName("TipoCuenta")]
        [ToolTip("My hint message")]
        [Persistent("TipoCuenta")]
        public TipoCuentaEnum TipoCuenta
        {
            get { return _TipoCuenta; }
            set { SetPropertyValue(nameof(TipoCuenta), ref _TipoCuenta, value); }
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

        private string _NombreCompleto;
        [XafDisplayName("NombreCompleto"), ToolTip("My hint message")]
        [Persistent("NombreCompleto"), RuleRequiredField(DefaultContexts.Save)]
        public string NombreCompleto
        {
            get { return _NombreCompleto; }
            set { SetPropertyValue(nameof(NombreCompleto), ref _NombreCompleto, value); }
        }

        private EstadosCuenta _Estados;
        [Association("EstadosCuenta-Cuenta")]

        [XafDisplayName("EstadosCuenta")]
        public EstadosCuenta Estados
        {
            get { return _Estados; }
            set { SetPropertyValue(nameof(Estados), ref _Estados, value); }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}