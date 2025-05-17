using DevExpress.CodeParser;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraSpreadsheet.Model.NumberFormatting;
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
    public class Datos : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Datos(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {


            this.FechaOperacion = DateTime.Now;

            if (SecuritySystem.CurrentUserId != null)
            {
                Usuarios actual = Session.FindObject<Usuarios>(
                    CriteriaOperator.Parse("[Oid] = ?", SecuritySystem.CurrentUserId)
                );
                if (actual != null)
                {
                    this.User = actual;
                }
            }
            this.FolioInterno = "SP-";
            base.AfterConstruction();
        }


        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (propertyName == "Cuenta")
            {
                if (Cuenta != null)
                {
                    this.Comites = Cuenta.Comites != null ? Cuenta.Comites : Comites;
                    this.BancoRegistros = Cuenta.Bancos != null ? Cuenta.Bancos : BancoRegistros;
                    this.NombreCtaRegistro = Cuenta.Nombre != null ? Cuenta.Nombre : NombreCtaRegistro;
                    this.Banco = Cuenta.Estados != null ? Cuenta.Estados.Estado : Banco;
                    this.Clabes = Cuenta.Clabes != null ? Cuenta.Clabes : Clabes;
                    this.NombreAlias = Cuenta.Nombre != null ? Cuenta.Nombre : NombreAlias;
                }
            }

            if (propertyName == "EmisorProveedorBeneficiarios")
            {
                if(EmisorProveedorBeneficiarios != null)
                {
                    this.BancoReceptorS = EmisorProveedorBeneficiarios.BancoReceptor != null ? EmisorProveedorBeneficiarios.BancoReceptor : BancoReceptorS;
                    this.CuentaExterna = EmisorProveedorBeneficiarios.CuentaExterna != null ? EmisorProveedorBeneficiarios.CuentaExterna : CuentaExterna;
                    this.ClabeProveedor = EmisorProveedorBeneficiarios.CuentaExterna.Clabes != null ? EmisorProveedorBeneficiarios.CuentaExterna.Clabes : Clabes;
                    //this.Clabes = EmisorProveedorBeneficiarios.CuentaExterna.Clabes != null ? EmisorProveedorBeneficiarios.CuentaExterna.Clabes : Clabes;
                    
                    //if(EmisorProveedorBeneficiarios.Clabe.Numeros != null)
                    //{
                    //    this.Clabe = EmisorProveedorBeneficiarios.Clabe.Numeros != null ? EmisorProveedorBeneficiarios.Clabe.Numeros : Clabe;
                    //}
                    

                }
            }

            if (TipoMovimientosE == null) {
                if (Mes == null)
                {

                }
                else
                {
                    this.Concatenar = MesNumerico;
                }
            }
            else
            {
                this.Concatenar = Comites + " " + TipoMovimientosE.Nombre + " " + MesNumerico;
            }

            if (propertyName == nameof(TipoMovimientosE) || propertyName == nameof(Cuenta))
            {
                ActualizarTipoGasto();
            }

            if (propertyName == nameof(Cuenta))
            {
                ActualizarNomenclatura();
            }
        }


    protected override void OnSaving()
        {
            base.OnSaving();

            DateTime hoy = DateTime.Today;
            DateTime inicioMesActual = new DateTime(hoy.Year, hoy.Month, 1);
            DateTime finMesActual = inicioMesActual.AddMonths(1).AddDays(-1);

            DateTime inicioMesPasado = inicioMesActual.AddMonths(-1);
            DateTime finMesPasado = inicioMesActual.AddDays(-1);
            DateTime limiteMesPasado = finMesPasado.AddDays(5);

            if (!(
                (FechaOperacion.Date >= inicioMesActual && FechaOperacion.Date <= finMesActual) ||
                (FechaOperacion.Date >= inicioMesPasado && FechaOperacion.Date <= finMesPasado && hoy <= limiteMesPasado)
            ))
            {
                throw new UserFriendlyException("La fecha debe estar en el mes actual o en el mes pasado (solo si no han pasado más de 5 días desde que terminó ese mes).");
            }

            // Solo continuar si hay importes válidos
            if (ImporteAbono == 0 && ImporteCargo == 0)
                return;

            if (Corte == null)
            {
                Corte = new Corte(Session);
            }

            Corte.Fecha = this.FechaOperacion;
            Corte.Cuenta = this.Cuenta;
            Corte.Monto = ImporteAbono != 0 ? ImporteAbono : -ImporteCargo;

            if (!string.IsNullOrWhiteSpace(FolioIdentificadorOficio))
            {
                var duplicado = Session.Query<Datos>()
                    .Where(x => x.FolioIdentificadorOficio == this.FolioIdentificadorOficio
                                && x.Oid != this.Oid
                                ) // Solo registros activos
                    .FirstOrDefault();

                if (duplicado != null)
                {
                    throw new UserFriendlyException("Ya existe un registro con el mismo Folio Identificador de Oficio.");
                }
            }


            if (!string.Equals(Banco, "ACTIVA", StringComparison.OrdinalIgnoreCase))
            {
                throw new UserFriendlyException("Cuenta no activa.");
            }
        }

    protected override void OnDeleting()
        {
            // Eliminar el Corte asociado si existe
            if (Corte != null && !Corte.IsDeleted)
            {
                Corte.Delete();
            }

            base.OnDeleting();
        }

        private Corte _Corte;
        [XafDisplayName("Corte Asociado")]
        public Corte Corte
        {
            get => _Corte;
            set => SetPropertyValue(nameof(Corte), ref _Corte, value);
        }

        private void ActualizarTipoGasto()
        {
            if (TipoMovimientosE != null && TipoMovimientosE.Nombre == "Pago Proveedor ")
            {
                if (Cuenta != null)
                {
                    TipoGasto = Cuenta.TipoCuenta == Cuentas.TipoCuentaEnum.GastoProgramado
                        ? "Gasto Programado"
                        : "Ordinario";
                }
                else
                {
                    TipoGasto = "Ordinario";
                }
            }
            else
            {
                TipoGasto = "Ordinario";
            }
        }

        private void ActualizarNomenclatura()
        {
            if (Cuenta != null)
            {
                Nomenclatura = Cuenta.TipoCuenta == Cuentas.TipoCuentaEnum.GastoProgramado
                    ? "Gasto Programado"
                    : "Ordinario";
            }
            else
            {
                Nomenclatura = null;
            }
        }


        private Usuarios _User;
        [Association("Usuarios-Datos")]

        [XafDisplayName("Usuarios")]
        public Usuarios User
        {
            get { return _User; }
            set { SetPropertyValue(nameof(User), ref _User, value); }
        }

        private Comites _Comites;
        [Association("Comites-Datos")]

        [XafDisplayName("Comites")]
        [ModelDefault("AllowEdit", "False")]
        public Comites Comites
        {
            get { return _Comites; }
            set { SetPropertyValue(nameof(Comites), ref _Comites, value); }
        }

        private DateTime _FechaOperacion;

        [XafDisplayName("Fecha y Hora de Operación"), ToolTip("Ingrese la fecha y hora de la operación")]
        [Persistent("FechaOperacion")]
        [ModelDefault("DisplayFormat", "{0:g}")]
        [ModelDefault("EditMask", "g")]
        [ModelDefault("PropertyEditorType", "DevExpress.ExpressApp.Editors.DateTimePropertyEditor")]
        [ModelDefault("DateEditCalendarSettings", @"ShowTodayButton=true; ShowClearButton=true; ShowWeekNumbers=false; ShowFooter=true; ShowHour=true; ShowMinute=true; ShowSecond=false;")]
        public DateTime FechaOperacion
        {
            get { return _FechaOperacion; }
            set
            {
                if (SetPropertyValue(nameof(FechaOperacion), ref _FechaOperacion, value))
                {
                    Mes = _FechaOperacion.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                    MesNumerico = _FechaOperacion.Month.ToString("D2"); 
                }
            }
        }

        private string _Mes;
        [VisibleInDetailView(false)]
        [ModelDefault("AllowEdit", "False")]
        [XafDisplayName("Mes"), ToolTip("Nombre del mes en texto")]
        [Persistent("Mes")]
        public string Mes
        {
            get { return _Mes; }
            set { SetPropertyValue(nameof(Mes), ref _Mes, value); }
        }

        private string _MesNumerico;
        [XafDisplayName("Mes Numérico"), ToolTip("Número del mes con dos dígitos")]
        [Persistent("MesNumerico")]
        public string MesNumerico
        {
            get { return _MesNumerico; }
            set { SetPropertyValue(nameof(MesNumerico), ref _MesNumerico, value); }
        }

        private TipoOperacion _TipoOperacion;

        [ImmediatePostData]
        [RuleRequiredField(DefaultContexts.Save)]
        [Association("Ingreso / Egreso-Datos")]
        [XafDisplayName("Ingreso / Egreso")]

        public TipoOperacion TipoOperacion
        {
            get => _TipoOperacion;
            set => SetPropertyValue(nameof(TipoOperacion), ref _TipoOperacion, value);
        }

        //private TipoOperacion _TipoOperacion;
        //[Association("Ingreso / Egreso-Datos")]

        //[XafDisplayName("Ingreso / Egreso")]
        //public TipoOperacion TipoOperacion
        //{
        //    get { return _TipoOperacion; }
        //    set { SetPropertyValue(nameof(TipoOperacion), ref _TipoOperacion, value); }
        //}

        private TipoMovimiento _TipoMovimientosE;
        [Association("TipoMovimientoEntrada-Datos")]

        [XafDisplayName("TipoMovimiento")]
        public TipoMovimiento TipoMovimientosE
        {
            get { return _TipoMovimientosE; }
            set { SetPropertyValue(nameof(TipoMovimientosE), ref _TipoMovimientosE, value); }
        }

        private Cuentas _Cuenta;
        [Association("Cuenta-Datos")]

        [XafDisplayName("Cuenta de Registro")]
        public Cuentas Cuenta
        {
            get { return _Cuenta; }
            set { SetPropertyValue(nameof(Cuenta), ref _Cuenta, value); }
        }

        private Bancos _BancoRegistros;
        [DataSourceCriteria("CuentaPartido = true")]
        [ModelDefault("AllowEdit", "False")]
        [Association("BancoRegistro-Datos")]

        [XafDisplayName("BancoRegistro")]
        public Bancos BancoRegistros
        {
            get { return _BancoRegistros; }
            set { SetPropertyValue(nameof(BancoRegistros), ref _BancoRegistros, value); }
        }
        

        private string _NombreCtaRegistro;
        [XafDisplayName("Nombre Cta Registro"), ToolTip("My hint message")]
        [ModelDefault("AllowEdit", "False")]
        [Persistent("NombreCtaRegistro")]
        public string NombreCtaRegistro
        {
            get { return _NombreCtaRegistro; }
            set { SetPropertyValue(nameof(NombreCtaRegistro), ref _NombreCtaRegistro, value); }
        }

        private EmisorProveedorBeneficiario _EmisorProveedorBeneficiarios;
        [Association("EmisorProveedorBeneficiario-Datos")]

        [XafDisplayName("EmisorProveedorBeneficiario")]
        public EmisorProveedorBeneficiario EmisorProveedorBeneficiarios
        {
            get { return _EmisorProveedorBeneficiarios; }
            set { SetPropertyValue(nameof(EmisorProveedorBeneficiarios), ref _EmisorProveedorBeneficiarios, value); }
        }

        private Bancos _BancoReceptorS;
        [Association("BancoReceptor-Datos")]
        [ModelDefault("AllowEdit", "False")]
        [XafDisplayName("BancoReceptor")]
        public Bancos BancoReceptorS
        {
            get { return _BancoReceptorS; }
            set { SetPropertyValue(nameof(BancoReceptorS), ref _BancoReceptorS, value); }
        }

        private CuentaExtrena _CuentaExterna;
        [Association("CuentaExterna-Datos")]
        [ModelDefault("AllowEdit", "False")]

        [XafDisplayName("CuentaExterna")]
        public CuentaExtrena CuentaExterna
        {
            get { return _CuentaExterna; }
            set { SetPropertyValue(nameof(CuentaExterna), ref _CuentaExterna, value); }
        }

        private string _Clabes;
        [XafDisplayName("Clabe"), ToolTip("My hint message")]
        [ModelDefault("AllowEdit", "False")]
        [Persistent("Clabes")]
        public string Clabes
        {
            get { return _Clabes; }
            set { SetPropertyValue(nameof(Clabes), ref _Clabes, value); }
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

        //private string _CuentaCuentaClabeLineaCap;
        //[XafDisplayName("Cuenta/Clabe/Linea Cap"), ToolTip("My hint message")]
        //[Persistent("CuentaCuentaClabeLineaCap")]
        //public string CuentaCuentaClabeLineaCap
        //{
        //    get { return _CuentaCuentaClabeLineaCap; }
        //    set { SetPropertyValue(nameof(CuentaCuentaClabeLineaCap), ref _CuentaCuentaClabeLineaCap, value); }
        //}

        private string _FolioInterno;

        [XafDisplayName("Folio Interno")]
        [Persistent("FolioInterno")]
        public string FolioInterno
        {
            get { return _FolioInterno; }
            set
            {
                var valorConPrefijo = value != null && value.StartsWith("SP-") ? value : $"SP-{value}";
                SetPropertyValue(nameof(FolioInterno), ref _FolioInterno, valorConPrefijo);
            }
        }


        private string _FolioIdentificadorOficio;
        [XafDisplayName("Folio Identificador Oficio"), ToolTip("My hint message")]
        [Persistent("FolioIdentificadorOficio")]
        public string FolioIdentificadorOficio
        {
            get { return _FolioIdentificadorOficio; }
            set { SetPropertyValue(nameof(FolioIdentificadorOficio), ref _FolioIdentificadorOficio, value); }
        }

        private DateTime _FechaOficio;
        [XafDisplayName("Fecha Oficio"), ToolTip("Ingrese la fecha de la operacion")]
        [Persistent("FechaOficio")]
        [ModelDefault("DisplayFormat", "{0:g}")]
        [ModelDefault("EditMask", "g")]
        [ModelDefault("PropertyEditorType", "DevExpress.ExpressApp.Editors.DateTimePropertyEditor")]
        [ModelDefault("DateEditCalendarSettings", @"ShowTodayButton=true; ShowClearButton=true; ShowWeekNumbers=false; ShowFooter=true; ShowHour=true; ShowMinute=true; ShowSecond=false;")]
        public DateTime FechaOficio
        {
            get { return _FechaOficio; }
            set
            {
                if (SetPropertyValue(nameof(FechaOficio), ref _FechaOficio, value))
                {
                    Mes = _FechaOperacion.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                }
            }
        }

        private string _FolioFiscal;
        [XafDisplayName("Folio Fiscal"), ToolTip("My hint message")]
        [Persistent("FolioFiscal")]
        public string FolioFiscal
        {
            get { return _FolioFiscal; }
            set { SetPropertyValue(nameof(FolioFiscal), ref _FolioFiscal, value); }
        }

        private string _NoFacturaDocumentoComprobatorio;
        [XafDisplayName("No. Factura/Documento Comprobatorio"), ToolTip("My hint message")]
        [Persistent("NoFacturaDocumentoComprobatorio")]
        public string NoFacturaDocumentoComprobatorio
        {
            get { return _NoFacturaDocumentoComprobatorio; }
            set { SetPropertyValue(nameof(NoFacturaDocumentoComprobatorio), ref _NoFacturaDocumentoComprobatorio, value); }
        }

        private Concepto _Concepto;
        [Association("Concepto-Datos")]

        [XafDisplayName("Concepto")]
        public Concepto Concepto
        {
            get { return _Concepto; }
            set { SetPropertyValue(nameof(Concepto), ref _Concepto, value); }
        }

        //private string _ConceptoPagoGasto;
        //[XafDisplayName("Concepto Pago/Gasto"), ToolTip("My hint message")]
        //[Size (SizeAttribute.Unlimited)]
        //[Persistent("ConceptoPagoGasto")]
        //public string ConceptoPagoGasto
        //{
        //    get { return _ConceptoPagoGasto; }
        //    set { SetPropertyValue(nameof(ConceptoPagoGasto), ref _ConceptoPagoGasto, value); }
        //}

        private string _ConceptoPortal;
        [XafDisplayName("Concepto Portal"), ToolTip("My hint message")]
        [RuleRegularExpression(@"^[A-ZÁÉÍÓÚÑ\s]+$")]
        [Persistent("ConceptoPortal")]
        public string ConceptoPortal
        {
            get { return _ConceptoPortal; }
            set { SetPropertyValue(nameof(ConceptoPortal), ref _ConceptoPortal, value); }
        }

        private string _ClaveRastreoReferencia;
        [XafDisplayName("Clave Rastreo/Referencia"), ToolTip("My hint message")]
        [Persistent("ClaveRastreoReferencia")]
        public string ClaveRastreoReferencia
        {
            get { return _ClaveRastreoReferencia; }
            set { SetPropertyValue(nameof(ClaveRastreoReferencia), ref _ClaveRastreoReferencia, value); }
        }

        private decimal _ImporteCargo;
        [XafDisplayName("Importe Cargo"), ToolTip("My hint message")]
        public decimal ImporteCargo
        {
            get { return _ImporteCargo; }
            set { SetPropertyValue(nameof(ImporteCargo), ref _ImporteCargo, value); }
        }

        private decimal _ImporteAbono;
        [XafDisplayName("Importe Abono"), ToolTip("My hint message")]
        public decimal ImporteAbono
        {
            get { return _ImporteAbono; }
            set { SetPropertyValue(nameof(ImporteAbono), ref _ImporteAbono, value); }
        }

        private Areas _Area;
        [Association("Areas-Datos")]

        [XafDisplayName("Areas")]
        public Areas Area
        {
            get { return _Area; }
            set { SetPropertyValue(nameof(Area), ref _Area, value); }
        }

        private Status _Status;
        [Association("Status-Datos")]

        [XafDisplayName("Estatus")]
        public Status Status
        {
            get { return _Status; }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }

        private string _Observaciones;
        [XafDisplayName("Observaciones"), ToolTip("My hint message")]
        [Size(SizeAttribute.Unlimited)]
        [Persistent("Observaciones")]
        public string Observaciones
        {
            get { return _Observaciones; }
            set { SetPropertyValue(nameof(Observaciones), ref _Observaciones, value); }
        }

        private string _Banco;
        [XafDisplayName("Banco"), ToolTip("My hint message")]
        [ModelDefault("AllowEdit", "False")]
        [Persistent("Banco")]
        public string Banco
        {
            get { return _Banco; }
            set { SetPropertyValue(nameof(Banco), ref _Banco, value); }
        }

        private string _TipoGasto;
        [XafDisplayName("Tipo Gasto"), ToolTip("My hint message")]
        [ModelDefault("AllowEdit", "False")]
        [Persistent("TipoGasto")]
        public string TipoGasto
        {
            get { return _TipoGasto; }
            set { SetPropertyValue(nameof(TipoGasto), ref _TipoGasto, value); }
        }

        private string _Nomenclatura;
        [XafDisplayName("Nomenclatura"), ToolTip("My hint message")]
        [ModelDefault("AllowEdit", "False")]
        [Persistent("Nomenclatura")]
        public string Nomenclatura
        {
            get { return _Nomenclatura; }
            set { SetPropertyValue(nameof(Nomenclatura), ref _Nomenclatura, value); }
        }

        private string _NombreAlias;
        [XafDisplayName("Nombre/Alias"), ToolTip("My hint message")]
        [ModelDefault("AllowEdit", "False")]
        [Persistent("NombreAlias")]
        public string NombreAlias
        {
            get { return _NombreAlias; }
            set { SetPropertyValue(nameof(NombreAlias), ref _NombreAlias, value); }
        }

        private string _Concatenar;
        [XafDisplayName("Concatenar"), ToolTip("My hint message")]
        [ModelDefault("AllowEdit", "False")]
        [Persistent("Concatenar")]
        public string Concatenar
        {
            get { return _Concatenar; }
            set { SetPropertyValue(nameof(Concatenar), ref _Concatenar, value); }
        }

        private FileData _Cargas;
        [XafDisplayName("Cargas")]
        [VisibleInDetailView(false)]
        [Persistent("Cargas")]
        public FileData Cargas
        {
            get { return _Cargas; }
            set { SetPropertyValue(nameof(Cargas), ref _Cargas, value); }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}