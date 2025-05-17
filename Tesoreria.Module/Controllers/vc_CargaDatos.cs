using Tesoreria.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace Tesoreria.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vc_CargaDatos : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public vc_CargaDatos()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void popupCargarHojaDatos_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            Datos datos = (BusinessObjects.Datos)View.CurrentObject;
            Console.WriteLine(datos);

            IObjectSpace objectSpace = Application.CreateObjectSpace(((ListView)View).ObjectTypeInfo.Type);
            var parameters = (NotMapped.DC_SubirArchivo)e.PopupWindowView.SelectedObjects[0];
            Session session = ((XPObjectSpace)this.ObjectSpace).Session;

            if (parameters.Archivo == null && parameters.Archivo.Size > 0)
                throw new Exception("Debes cargar un archivo Excel");
            if (parameters.Archivo.IsEmpty)
            {
                Application.ShowViewStrategy.ShowMessage("Debes cargar un archivo válido", InformationType.Error, 3500);
                return;
            }
            MemoryStream streamExcel = new MemoryStream();
            parameters.Archivo.SaveToStream(streamExcel);

            int registrosCargados = 0;

            using (MemoryStream stream = streamExcel)
            {
                Boolean versionXlsx = Path.GetExtension(parameters.Archivo.FileName) == ".xlsx" ? true : false;
                IExcelDataReader excelReader = versionXlsx ? ExcelReaderFactory.CreateOpenXmlReader(stream) : ExcelReaderFactory.CreateBinaryReader(stream);

                DataSet result = excelReader.AsDataSet();

                Int32 recorrido = 0;
                var rows = result.Tables[0].Rows;

                IEnumerable<DataRow> collection = result.Tables[0].Rows.Cast<DataRow>();

                string format = "dd/MM/yyyy";

                foreach (DataRow row in collection)
                {
                    if (recorrido > 0)
                    {

                        string
                            Registro = String.Empty,
                            FechaOperacion = String.Empty,
                            Mes = String.Empty,
                            MesNumerico = String.Empty,
                            Usuarios = String.Empty,
                            CuentaRegistro = String.Empty,
                            Clabe = String.Empty,
                            Comites = String.Empty,
                            BancoRegistro = String.Empty,
                            NombreCtaRegistro = String.Empty,
                            IngresoEgreso = String.Empty,
                            TipoMovimiento = String.Empty,
                            EmisorProveedorBeneficiario = String.Empty,
                            CuentaExterna = String.Empty,
                            BancoReceptor = String.Empty,
                            ClabeProveedor = String.Empty,
                            FechaOficio = String.Empty,
                            FolioInterno = String.Empty,
                            FolioIdentificadorOficio = String.Empty,
                            FolioFiscal = String.Empty,
                            NoFactura = String.Empty,
                            Concepto = String.Empty,
                            ConceptoPortal = String.Empty,
                            ClaveRastreoReferencia = String.Empty,
                            ImporteCargo = String.Empty,
                            ImporteAbono = String.Empty,
                            Area = String.Empty,
                            Estatus = String.Empty,
                            Observaciones = String.Empty,
                            Banco = String.Empty,
                            TipoGasto = String.Empty,
                            Nomenclatura = String.Empty,
                            NombreAlias = String.Empty,
                            Concatenar = String.Empty;

                        int arr = 0;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            Registro = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(Registro))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                Registro = Registro.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            FechaOperacion = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(FechaOperacion))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                FechaOperacion = FechaOperacion.Trim();
                            }
                        }
                        arr++;
                                                
                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            Usuarios = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(Usuarios))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                Usuarios = Usuarios.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            CuentaRegistro = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(CuentaRegistro))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                CuentaRegistro = CuentaRegistro.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            IngresoEgreso = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(IngresoEgreso))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                IngresoEgreso = IngresoEgreso.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            TipoMovimiento = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(TipoMovimiento))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                TipoMovimiento = TipoMovimiento.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            EmisorProveedorBeneficiario = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(EmisorProveedorBeneficiario))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                EmisorProveedorBeneficiario = EmisorProveedorBeneficiario.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            FechaOficio = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(FechaOficio))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                FechaOficio = FechaOficio.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            FolioInterno = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(FolioInterno))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                FolioInterno = FolioInterno.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            FolioIdentificadorOficio = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(FolioIdentificadorOficio))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                FolioIdentificadorOficio = FolioIdentificadorOficio.Trim();
                            }
                        }
                        arr++;
                        
                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            FolioFiscal = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(FolioFiscal))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                FolioFiscal = FolioFiscal.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            NoFactura = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(NoFactura))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                NoFactura = NoFactura.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            Concepto = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(Concepto))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                Concepto = Concepto.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            ConceptoPortal = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(ConceptoPortal))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                ConceptoPortal = ConceptoPortal.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            ClaveRastreoReferencia = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(ClaveRastreoReferencia))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                ClaveRastreoReferencia = ClaveRastreoReferencia.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            ImporteCargo = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(ImporteCargo))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                ImporteCargo = ImporteCargo.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            ImporteAbono = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(ImporteAbono))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                ImporteAbono = ImporteAbono.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            Area = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(Area))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                Area = Area.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            Estatus = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(Estatus))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                Estatus = Estatus.Trim();
                            }
                        }
                        arr++;

                        if (row.ItemArray.Length > arr && !string.IsNullOrEmpty(row.ItemArray[arr].ToString()))
                        {
                            Observaciones = row.ItemArray[arr].ToString();
                            if (!string.IsNullOrEmpty(Observaciones))
                            {
                                if (rows.Count == 0)
                                    collection = collection.Skip(0);
                                Observaciones = Observaciones.Trim();
                            }
                        }
                        arr++;

                        Console.WriteLine(Registro);
                        Console.WriteLine(FechaOperacion);
                        Console.WriteLine(Usuarios);
                        Console.WriteLine(CuentaRegistro);
                        Console.WriteLine(IngresoEgreso);
                        Console.WriteLine(TipoMovimiento);
                        Console.WriteLine(EmisorProveedorBeneficiario);
                        Console.WriteLine(FechaOficio);
                        Console.WriteLine(FolioInterno);
                        Console.WriteLine(FolioIdentificadorOficio);
                        Console.WriteLine(FolioFiscal);
                        Console.WriteLine(NoFactura);
                        Console.WriteLine(Concepto);
                        Console.WriteLine(ConceptoPortal);
                        Console.WriteLine(ClaveRastreoReferencia);
                        Console.WriteLine(ImporteCargo);
                        Console.WriteLine(ImporteAbono);
                        Console.WriteLine(Area);
                        Console.WriteLine(Estatus);
                        Console.WriteLine(Observaciones);

                        Datos dato = new Datos(session);
                        bool EncontroError = false;
                        bool Advertencia = false;

                        throw new UserFriendlyException($"El registro ubicado en la Fila '{row.ItemArray.Count()}' no se creo debido a los siguientes errores: No se creara el registro.");

                        List<string> errores = new List<string>(); // Lista para almacenar los mensajes de error

                        try
                        {
                            if (!string.IsNullOrEmpty(FechaOperacion))
                            {
                                DateTime dateTime = DateTime.Parse(FechaOperacion);
                                dato.FechaOperacion = dateTime;
                                dato.Mes = dateTime.ToString("MMMM", new CultureInfo("es-MX"));
                                dato.MesNumerico = dateTime.Month.ToString();
                                EncontroError = true;
                                errores.Add("Error al procesar la fecha de operación"); // Agregar mensaje de error
                            }
                        }
                        catch (Exception ex)
                        {
                            FechaOperacion = null;
                            errores.Add($"Error al convertir la fecha: {ex.Message}"); // Agregar mensaje de error con detalle
                        }

                        Usuarios UsuarioDuplicado = new XPCollection<Usuarios>(session).Where(x => x.UserName.ToUpper() == Usuarios.ToUpper()).FirstOrDefault();
                        if (UsuarioDuplicado == null)
                        {
                            EncontroError = true;
                            errores.Add("Usuario no encontrado"); // Agregar mensaje de error
                        }
                        else
                        {
                            dato.User = UsuarioDuplicado;
                        }

                        // Al final, puedes usar la lista 'errores' para mostrar todos los mensajes
                        if (errores.Count > 0)
                        {
                            string todosErrores = string.Join(Environment.NewLine, errores);
                            // Puedes mostrar 'todosErrores' en un MessageBox o registrarlo
                            // MessageBox.Show(todosErrores);
                        }


                        Cuentas CuentaDuplicada = new XPCollection<Cuentas>(session).Where(x => x.Cuenta.ToUpper() == CuentaRegistro.ToUpper()).FirstOrDefault();
                        if (CuentaDuplicada == null && !string.IsNullOrWhiteSpace(CuentaRegistro))
                        {
                            Cuentas Cuen = new Cuentas(session);
                            Cuen.Cuenta = CuentaRegistro;
                            Cuen.Save();
                            Cuen.Session.CommitTransaction();
                            objectSpace.CommitChanges();
                            dato.Cuenta = Cuen;
                            Advertencia = true;

                        }
                        else
                        {
                            dato.Cuenta = CuentaDuplicada;
                            dato.Clabes = CuentaDuplicada.Clabes;
                            dato.Comites = CuentaDuplicada.Comites;
                            dato.Banco = CuentaDuplicada.Bancos.Nombre;
                            dato.NombreCtaRegistro = CuentaDuplicada.Nombre;
                        }

                        if (IngresoEgreso == "INGRESO" || IngresoEgreso == "EGRESO")
                        {
                            TipoOperacion tipo = new TipoOperacion(session);
                            tipo.Nombre = IngresoEgreso;
                            tipo.Save();
                            tipo.Session.CommitTransaction();
                            objectSpace.CommitChanges();
                            dato.TipoOperacion = tipo;
                        }
                        else
                        {
                            EncontroError = true;
                        }
                        
                        TipoMovimiento TipoMDuplicado = new XPCollection<TipoMovimiento>(session).Where(x => x.Nombre.ToUpper() == TipoMovimiento.ToUpper()).FirstOrDefault();
                        if (TipoMDuplicado == null && TipoMovimiento != "")
                        {
                            TipoMovimiento tipo = new TipoMovimiento(session);
                            tipo.Nombre = TipoMovimiento;
                            tipo.Save();
                            tipo.Session.CommitTransaction();
                            objectSpace.CommitChanges();
                            dato.TipoMovimientosE = tipo;

                        }
                        else
                        {
                            dato.TipoMovimientosE = TipoMDuplicado;
                        }

                        EmisorProveedorBeneficiario EmisorDuplicado = new XPCollection<EmisorProveedorBeneficiario>(session).Where(x => x.Nombre.ToUpper() == EmisorProveedorBeneficiario.ToUpper()).FirstOrDefault();if (EmisorDuplicado == null && EmisorProveedorBeneficiario != "")
                        {
                            EmisorProveedorBeneficiario emisor = new EmisorProveedorBeneficiario(session);
                            emisor.Nombre = EmisorProveedorBeneficiario;
                            emisor.Save();
                            emisor.Session.CommitTransaction();
                            objectSpace.CommitChanges();
                            dato.EmisorProveedorBeneficiarios = emisor;
                            Advertencia = true;

                        }
                        else
                        {
                            dato.EmisorProveedorBeneficiarios = EmisorDuplicado;
                            dato.CuentaExterna = EmisorDuplicado.CuentaExterna;
                            dato.BancoReceptorS = EmisorDuplicado.BancoReceptor;
                            dato.ClabeProveedor = EmisorDuplicado.ClabeProveedor;
                        }

                        try
                        {
                            if (FechaOficio != null && FechaOficio != "")
                            {
                                DateTime dateTime = DateTime.Parse(FechaOficio);
                                dato.FechaOficio = dateTime;
                            }
                        }
                        catch (Exception ex)
                        {
                            FechaOficio = null;
                        }

                        dato.FolioInterno = FolioInterno;

                        var existe = new XPQuery<Datos>(session).Any(d => d.FolioIdentificadorOficio == FolioIdentificadorOficio);

                        if (!existe)
                        {
                            dato.FolioIdentificadorOficio = FolioIdentificadorOficio;
                        }
                        else
                        {
                            EncontroError = true;
                        }

                        dato.FolioFiscal = FolioFiscal;
                        dato.NoFacturaDocumentoComprobatorio = NoFactura;

                        Concepto ConceptoDuplicado = new XPCollection<Concepto>(session).Where(x => x.ConceptoPagoGasto.ToUpper() == Concepto.ToUpper()).FirstOrDefault();
                        if (ConceptoDuplicado == null && Concepto != "")
                        {
                            Concepto Con = new Concepto(session);
                            Con.ConceptoPagoGasto = Concepto;
                            Con.Save();
                            Con.Session.CommitTransaction();
                            objectSpace.CommitChanges();
                            dato.Concepto = Con;

                        }
                        else
                        {
                            dato.Concepto = ConceptoDuplicado;
                        }

                        dato.ConceptoPortal = ConceptoPortal;
                        dato.ClaveRastreoReferencia = ClaveRastreoReferencia;

                        if ((ImporteAbono != null || ImporteAbono != string.Empty) && (ImporteCargo != null || ImporteCargo != string.Empty))
                        {
                            EncontroError = true;
                        }
                        else
                        {
                            decimal valorDecimal;
                            if (decimal.TryParse(ImporteCargo, out valorDecimal))
                            {
                                dato.ImporteCargo = valorDecimal;
                            }
                            else
                            {
                                dato.ImporteCargo = 0;
                            }

                            if (decimal.TryParse(ImporteAbono, out valorDecimal))
                            {
                                dato.ImporteAbono = valorDecimal;
                            }
                            else
                            {
                                dato.ImporteAbono = 0;
                            }
                        }

                        Areas AreaDuplicada = new XPCollection<Areas>(session).Where(x => x.Nombre.ToUpper() == Area.ToUpper()).FirstOrDefault();
                        if (AreaDuplicada == null && Area != "")
                        {
                            Areas ar = new Areas(session);
                            ar.Nombre = Area;
                            ar.Save();
                            ar.Session.CommitTransaction();
                            objectSpace.CommitChanges();
                            dato.Area = ar;
                        }
                        else
                        {
                            dato.Area = AreaDuplicada;
                        }

                        Status StatusDuplicado = new XPCollection<Status>(session).Where(x => x.Nombre.ToUpper() == Estatus.ToUpper()).FirstOrDefault();
                        if (StatusDuplicado == null && Estatus != "")
                        {
                            Status sta = new Status(session);
                            sta.Nombre = Estatus;
                            sta.Save();
                            sta.Session.CommitTransaction();
                            objectSpace.CommitChanges();
                            dato.Status = sta;

                        }
                        else
                        {
                            dato.Status = StatusDuplicado;
                        }

                        dato.Observaciones = Observaciones;

                        dato.Banco = CuentaDuplicada.Estados.Estado != null ? CuentaDuplicada.Estados.Estado : Banco;

                        if (TipoMDuplicado != null && TipoMDuplicado.Nombre == "Pago Proveedor ")
                        {
                            if (CuentaRegistro != null)
                            {
                                dato.TipoGasto = CuentaDuplicada.TipoCuenta == Cuentas.TipoCuentaEnum.GastoProgramado
                                    ? "Gasto Programado"
                                    : "Ordinario";
                            }
                            else
                            {
                                dato.TipoGasto = "Ordinario";
                            }
                        }
                        else
                        {
                            dato.TipoGasto = "Ordinario";
                        }

                        if (CuentaDuplicada != null)
                        {
                            dato.Nomenclatura = CuentaDuplicada.TipoCuenta == Cuentas.TipoCuentaEnum.GastoProgramado
                                ? "Gasto Programado"
                                : "Ordinario";
                        }
                        else
                        {
                            dato.Nomenclatura = null;
                        }

                        if (CuentaDuplicada != null)
                        {
                            dato.NombreAlias = CuentaDuplicada.Nombre;
                        }
                        else
                        {
                            dato.NombreAlias = null;
                        }

                        if (TipoMovimiento == null)
                        {
                            if (Mes == null)
                            {

                            }
                            else
                            {
                                dato.Concatenar = MesNumerico;
                            }
                        }
                        else
                        {
                            dato.Concatenar = Comites + " " + TipoMovimiento + " " + MesNumerico;
                        }

                        if (EncontroError = true)
                        {
                            dato = null;
                            Console.WriteLine(dato);
                        }
                        else
                        {
                            dato.Save();
                            dato.Session.CommitTransaction();
                            objectSpace.CommitChanges();
                            registrosCargados++;
                        }
                    }
                    recorrido++;
                }
                this.View.Refresh();
                this.View.RefreshDataSource();

                // Mostrar contador
                Application.ShowViewStrategy.ShowMessage(
                    $"Se cargaron {registrosCargados}",
                    InformationType.Success,
                    4500
                );

            }

        }

        private void popupCargarHojaDatos_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            NonPersistentObjectSpace objectSpace =
     (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(NotMapped.DC_SubirArchivo));
            NotMapped.DC_SubirArchivo parameters = objectSpace.CreateObject<NotMapped.DC_SubirArchivo>();

            DetailView confirmationListView = Application.CreateDetailView(objectSpace, parameters);
            confirmationListView.Caption = "Elija un archivo Excel";
            confirmationListView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.View = confirmationListView;
        }
    }
}
//throw new UserFriendlyException($"El usuario '{Usuarios}' no existe. No se creara el registro.");
//throw new InvalidOperationException($"Solo se acepta 'INGRESO' o 'EGRESO' para el campo 'Ingreso / Egreso .");
