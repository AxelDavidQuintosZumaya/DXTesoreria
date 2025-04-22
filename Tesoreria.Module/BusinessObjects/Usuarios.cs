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
    public class Usuarios : ApplicationUser
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Usuarios(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _Nombre;
        [XafDisplayName("Nombre"), ToolTip("Nombre del empleado")]
        [Persistent("Nombre")]
        public string Nombre
        {
            get { return _Nombre; }
            set { SetPropertyValue(nameof(Nombre), ref _Nombre, value); }
        }

        private string _APaterno;
        [XafDisplayName("Apellido Paterno"), ToolTip("Apellido paterno del empleado")]
        [Persistent("APaterno")]
        public string APaterno
        {
            get { return _APaterno; }
            set { SetPropertyValue(nameof(APaterno), ref _APaterno, value); }
        }

        private string _AMaterno;
        [XafDisplayName("Apellido Materno"), ToolTip("Apellido materno del empleado")]
        [Persistent("AMaterno")]
        public string AMaterno
        {
            get { return _AMaterno; }
            set { SetPropertyValue(nameof(AMaterno), ref _AMaterno, value); }
        }

        private string _ClaveEmpleado;
        [XafDisplayName("Clave Empleado"), ToolTip("Identificador único del empleado")]
        [Persistent("ClaveEmpleado")]
        public string ClaveEmpleado
        {
            get { return _ClaveEmpleado; }
            set { SetPropertyValue(nameof(ClaveEmpleado), ref _ClaveEmpleado, value); }
        }

        [Association("Usuarios-Datos")]
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