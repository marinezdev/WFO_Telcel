<%@ Page Title="" Language="C#" MasterPageFile="~/Utilerias/Site.Master" AutoEventWireup="true" CodeBehind="ConsultaTramite.aspx.cs" Inherits="WFO.Procesos.Promotoria.ConsultaTramite" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">

    <!-- Campos Ocultos -->
    <div>
        <asp:HiddenField ID="hfTipoTramite" runat="server" Value="0" />
    </div>

    <!-- MODAL DE CARTAS -->
    <div class="modal fade Carta" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title" id="myModalLabel2">PDF <asp:Label ID="LabelTipoCarta" runat="server" ></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="Label4" runat="server" ></asp:Label>
                    <asp:Literal ID="ltMuestraCarta" runat="server"></asp:Literal>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL DE PDF -->
    <div class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title" id="myModalLabel2">PDF Expediente </h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="MensajePDF" runat="server" ></asp:Label>
                    <asp:Literal ID="ltMuestraPdf" runat="server"></asp:Literal>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL DE BITACORA -->
    <div class="modal fade bitacora" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title" id="myModalLabel3">Bitácora trámite </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <asp:Repeater ID="rptBitacora" runat="server" >
                                <HeaderTemplate>
                                    <table id="datatable" class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Numero</th>
                                                <th>Mesa</th>
                                                <th>Fecha inicio</th>
                                                <th>Fecha termino</th>
                                                <th>Usuario</th>
                                                <th>Estatus mesa</th>
                                                <th>Observación</th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Eval("Numero")%></td>
                                        <td><%#Eval("Mesa")%></td>
                                        <td><%#Eval("FechaInicio","{0:dd/MM/yyyy HH:mm:ss}")%></td>
                                        <td><%#Eval("FechaTermino","{0:dd/MM/yyyy HH:mm:ss}")%></td>
                                        <td><%#Eval("Usuario")%></td>
                                        <td><%#Eval("EstatusMesa")%></td>
                                        <td><%#Eval("Observacion")%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL DE FOLIO -->
    <div class="modal fade bs-example-modal-sm" id="myModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel2">Actualización trámite </h4>
                </div>
                <div class="modal-body text-center">
                    <h4>Operación realizada </h4>
                    <br />
                    <h4><asp:Label ID="LabelActualizacionTramite" Text="" runat="server" ></asp:Label></h4>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button2" runat="server" Text="Aceptar" class="btn btn-primary" CausesValidation="False" OnClick="TramiteTerminado"  />
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <!-- INFORMAICON DEL TRAMITE -->
        <div class="col-md-6 col-sm-6 col-xs-12">
            <div class="x_panel">
                <div class="x_title">
                    <h2><small style="color:#66B366;"><strong>Información de trámite</strong></small></h2>
                    <div class="clearfix"></div>
                </div>
                <div class="x_content">
                    <asp:Label ID="Label2" runat="server" Text="Folio" Font-Bold="true" class="control-label col-md-3 col-sm-3 col-xs-6 "></asp:Label>
                    <div class="col-md-9 col-sm-9 col-xs-6 form-group has-feedback">
                        <asp:Label ID="LabelFolio" runat="server" Text="-" Font-Bold="true" class="text-muted font-13 m-b-30"></asp:Label>
                    </div>
                    <asp:Label ID="Label5" runat="server" Text="Fecha Registro" Font-Bold="true" class="control-label col-md-3 col-sm-3 col-xs-6 "></asp:Label>
                    <div class="col-md-9 col-sm-9 col-xs-6 form-group has-feedback">
                        <asp:Label ID="LabelFechaRegistro" runat="server" Text="-" Font-Bold="true" class="text-muted font-13 m-b-30"></asp:Label>
                    </div>
                    <button type="button" class="btn btn-default" data-toggle="modal" data-target=".bd-example-modal-lg">Muestra PDF</button>
                </div>
            </div>
        </div>


        <!-- BITACORA -->
        <div class="col-md-6 col-sm-6 col-xs-12">
            <div class="x_panel">
                <div class="x_title">
                    <h2><small style="color:#66B366;"><strong>Estatus del trámite</strong></small></h2>
                    <div class="clearfix"></div>
                </div>
                <div class="x_content text-center">
                    <h2><small><asp:Label ID="LabelEstatusTramite" runat="server" Text="" Font-Bold="true"></asp:Label></small></h2>
                    <br />
                    <button type="button" class="btn btn-default" data-toggle="modal" data-target=".bitacora">Bitácora</button>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <asp:UpdatePanel ID="ObservacionesCartaSuspendido" runat="server" UpdateMode="Conditional" Visible="False">
            <ContentTemplate>
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2><small>Observaciones</small></h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="col-md-9 col-sm-9 col-xs-6 form-group has-feedback">
                                <p class="text-muted well well-sm no-shadow" style="margin-top: 10px;">
                                    <asp:Label ID="LabelObservacionesSuspendido" Text="" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </p>
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-6 form-group has-feedback">
                                <br />
                                <button style="display:none;" type="button" class="btn btn-default" data-toggle="modal" data-target=".Carta">Mostrar carta</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="AnexoArchivos" runat="server" UpdateMode="Conditional" Visible="False">
            <ContentTemplate>
                <div class="col-md-12 col-sm-12 col-xs-12 text-left">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2><small style="color:#66B366;"><strong>Reingresar Tráites</strong></small></h2>
                            <div class="clearfix"></div>
                            <div class="x_content text-left">
                                <br />
                                <div class="row">
                                    <div class=" profile_details">
                                        <div class="col-md-6 col-sm-6 col-xs-12 well profile_view">
                                            <div class="col-xs-12 bottom text-center">
                                                <h4 class="brief">Anexar Documento</h4><br />
                                            </div>
                                            <div class="right col-xs-12 text-left">
                                                <asp:UpdatePanel ID="PnlArchivosAnexos" runat="server">
                                                    <ContentTemplate>
                                                        <fieldset>
                                                        <asp:Label ID="lblDocumentosRequeridos" runat="server" Text="Archivos (*.PDF, *.JPG, *.PNG)"></asp:Label>
                                                        <asp:FileUpload ID="fileUpDocumento" runat="server"></asp:FileUpload>
                                                        <code><asp:Label ID="LabRespuestaArchivosCarga" runat="server" Text =""></asp:Label></code>
                                                        <br />
                                                        <asp:Button ID="btnSubirDocumento" runat="server" Text="Subir" class="btn btn-primary" CausesValidation="False" OnClick="btnSubirDocumento_Click"/><br />
                                                        <asp:ListBox ID="lstDocumentos" runat="server" Height="100px" Width="100%" SelectionMode="Single" class="select2_multiple form-control">
                                                        </asp:ListBox>
                                                        <br />
                                                        <asp:Button ID="btnEliminaDocumento" runat="server" Text="Eliminar" class="btn btn-danger" CausesValidation="False" OnClick="btnEliminaDocumento_Click" />
                                                    </fieldset>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnSubirDocumento" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        
                                    </div>

                                    <div class="profile_details">
                                        <div class="col-md-6 col-sm-6 col-xs-12 well profile_view">
                                            <div class="col-xs-12 bottom text-center">
                                                <h4 class="brief">Observaciones</h4><br />
                                            </div>
                                            <div class="right col-xs-12 text-left">
                                                <asp:UpdatePanel ID="RegistrarSuspendido" runat="server" UpdateMode="Conditional" Visible="False">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 col-sm-12 col-xs-12 ">
                                                            <div class="">
                                                                <div class="x_content text-left">
                                                                    <div class="row">
                                                                        <div class="col-md-12 col-sm-12 col-xs-12 form-group has-feedback">
                                                                            <asp:TextBox ID="txObervacionesSuspendido" runat="server" Font-Size="14px" TextMode="MultiLine" Height="175px" Width="100%" class="form-control" onKeyUp="document.getElementById(this.id).value=document.getElementById(this.id).value.toUpperCase()"></asp:TextBox>
                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" TargetControlID="txObervacionesSuspendido" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyzáéíóúÁÉÍÓÚ = $%*_0123456789-,.:+*/?¿+¡\/][{};" />
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txObervacionesSuspendido" ErrorMessage="*" InitialValue="" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-1 col-sm-1 col-xs-12 text-center">
                                                                            <asp:Button ID="BtnContinuarSuspendido" runat="server"  AutoPostBack="True" Text="Enviar" Class="btn btn-success" OnClick="BtnContinuarSuspendido_Click"/>
                                                                        </div>
                                                                        <div class="col-md-4 col-sm-4 col-xs-12 text-center">
                                                                            <code><asp:Label ID="MensajeSuspendido" runat="server"></asp:Label></code>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        
            
        

        <asp:UpdatePanel ID="ObservacionesCartaEjecucion" runat="server" UpdateMode="Conditional" Visible="False">
            <ContentTemplate>
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2><small>Carta ejecución</small></h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="col-xs-6">
                                <button type="button" class="btn btn-default" data-toggle="modal" data-target=".Carta">Mostrar carta</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="ObservacionesCartaRechazo" runat="server" UpdateMode="Conditional" Visible="False">
            <ContentTemplate>
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2><small>Observaciones</small></h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="col-md-3 col-sm-3 col-xs-6 form-group has-feedback">
                                <br />
                                <button type="button" class="btn btn-default" data-toggle="modal" data-target=".Carta">Mostrar carta</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="ObservacionesCartaCancelado" runat="server" UpdateMode="Conditional" Visible="False">
            <ContentTemplate>
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2><small>Observaciones</small></h2>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="col-md-3 col-sm-3 col-xs-6 form-group has-feedback">
                                <br />
                                <button type="button" class="btn btn-default" data-toggle="modal" data-target=".Carta">Mostrar carta</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

    <script>
        $(document).ready(function () {
            $('#pdfviewer').on('contextmenu', function (e) {
                e.preventDefault();
            });
        });
    </script>
</asp:Content>
