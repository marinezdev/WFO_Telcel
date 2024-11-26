<%@ Page Title="" Language="C#" MasterPageFile="~/Utilerias/Site.Master" AutoEventWireup="true" CodeBehind="TramiteNuevo1.aspx.cs" Inherits="WFO.Procesos.Promotoria.TramiteNuevo1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DevExpress.XtraCharts.v17.2.Web, Version=17.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v17.2, Version=17.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <asp:UpdatePanel ID="upPnlCaptura" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- MODAL DE INFORMACIÓN DE AYUDA -->
            <div class="modal fade bs-example-modal-sm" id="ayudaArchivos" tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalTitleAyudaArchivos" style="color:#26B99A; ">Documentos Requeridos</h4>
                        </div>
                        <div class="modal-body">
                            <p>
                                El archivo requerido es la plantilla de horas trabajas por el personal proveniente del Proveedor en el proyecto
                            </p>

                            <br />
                            <small>
                                <strong>NOTAS INFORMATIVAS</strong><br />
                                <ul>
                                    <li><asp:Label ID="lblDocumentosTipos" runat="server" /><br /></li>
                                    <li><asp:Label ID="lblDocumentosTamaño" runat="server" /><br /></li>
                                    <li><asp:Label ID="ExpedienteTamaño" runat="server" /><br /></li>
                                </ul>
                            </small>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-warning" data-dismiss="modal">Cerrar Ventana Informativa</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- MODAL DE FOLIO -->
            <div class="modal fade bs-example-modal-sm" id="myModal" tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalTitleTramiteCreado">Registro trámite </h4>
                        </div>
                        <div class="modal-body">
                            <h4>Trámite</h4>
                            <p>Trámite registrado exitosamente con el folio:</p>
                            <h4><asp:Label ID="LabelFolio" runat="server" ></asp:Label></h4>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button2" runat="server" Text="Aceptar" class="btn btn-primary" CausesValidation="False" OnClick="TramiteTerminado"  />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12 text-left">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 style="color:#26B99A;">Gestión de Proveedores. Nueva Solicitud</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li>
                                    <a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                      	        </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content text-left">
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <asp:Panel ID="producto1" runat="server" Visible="true">
                                        <asp:Label ID="lblProducto" runat="server" Text="*Producto" Font-Bold="true" class="control-label col-md-1 col-sm-1 col-xs-6 "></asp:Label>
                                        <asp:DropDownList ID="cboProyectos" runat="server" AutoPostBack="false" class="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="cboProyectos" ErrorMessage="*" InitialValue="-1" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </asp:Panel>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <asp:UpdatePanel ID="PnlArchivosAnexos" runat="server">
                                        <ContentTemplate>
                                            <fieldset>
                                                <div class="row">
                                                    <asp:Label ID="lblDocumentosRequeridosTitulo" runat="server" Text="* Documentos Requeridos" Font-Bold="true" class="control-label"></asp:Label>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-2 col-sm-2 col-xs-12">
                                                        <a onclick="$('#ayudaArchivos').modal({backdrop: 'static', keyboard: false});">
                                                            <img id="myImg" src="/imagenes/information.png" alt="Información Archivos Requeridos" title="Información sobre los documentos requeridos para la solicitud." style="width:20px;height:20px;">
                                                        </a>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-12">
                                                        <asp:FileUpload ID="fileUpDocumento" runat="server"></asp:FileUpload>
                                                    </div>
                                                    <div class="col-md-2 col-sm-2 col-xs-12">
                                                        <asp:Button ID="btnSubirDocumento" runat="server" Text="Subir" class="btn btn-primary" CausesValidation="False" OnClick="btnSubirDocumento_Click"/><br />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <code><asp:Label ID="LabRespuestaArchivosCarga" runat="server" Text =""></asp:Label></code>
                                                    </div>
                                                </div>
                                                <div class="row" style="display:none;">
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <asp:Label ID="lblDocumentosRequeridos" runat="server" Text="Archivos (*.XLS)"></asp:Label>
                                                        <br />
                                                        <span style="font-size: 9px">Tamaño máximo de archivo: <%= ArchivoMaximo1 %>&nbsp;MB</span><br />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <asp:ListBox ID="lstDocumentos" runat="server" Height="100px" Width="100%" SelectionMode="Single" class="select2_multiple form-control"></asp:ListBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <asp:Button ID="btnEliminaDocumento" runat="server" Text="Eliminar" class="btn btn-danger" CausesValidation="False"  OnClick="btnEliminaDocumento_Click"/>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnSubirDocumento" />
                                        </Triggers>
                                    </asp:UpdatePanel>   
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-1 col-sm-1 col-xs-12 text-center">
                                    <asp:Button ID="BtnContinuar" runat="server"  AutoPostBack="True" Text="Crear nueva solicitud" Class="btn btn-success" OnClick="BtnContinuar_Click"/>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12 text-center">
                                    <code><asp:Label ID="Respuesta" runat="server" ></asp:Label><asp:Label ID="Mensajes" runat="server"></asp:Label></code>
                                </div>
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
