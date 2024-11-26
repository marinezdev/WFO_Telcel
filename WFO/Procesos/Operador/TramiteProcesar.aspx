<%@ Page Title="" Language="C#" MasterPageFile="~/Utilerias/Site.Master" AutoEventWireup="true" CodeBehind="TramiteProcesar.aspx.cs" Inherits="WFO.Procesos.Operador.TramiteProcesar" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DevExpress.XtraCharts.v17.2.Web, Version=17.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v17.2, Version=17.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v17.2" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v17.2, Version=17.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxwtl" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v17.2" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <script>
        function Alerta(titulo, texto) {
            new PNotify({
                title: titulo,
                text: texto,
                type: 'error',
                styling: 'bootstrap3'
            });
        }

        function ShowSuspender() {
            var observacionesPrivadas = document.getElementById('<%=txtObservacionesPrivadas.ClientID%>').value;
            if (observacionesPrivadas.length > 0) {
                pnlPopMotivosSuspender.Show();
                pnlCallbackMotSuspender.PerformCallback();
            }
            else {
                Alerta('Observaciones Privadas', 'Por favor agregue observaciones privadas');
            }
        }

        function treeList_CustomDataCallbackSuspender(s, e) {
            document.getElementById('treeListCountCell').innerHTML = e.result;
        }
        function treeList_SelectionChangedSuspender(s, e) {
            window.setTimeout(function () { s.PerformCustomDataCallback(''); }, 0)
        }

        function ShowRechazar() {
            var observacionesPrivadas = document.getElementById('<%=txtObservacionesPrivadas.ClientID%>').value;
            if (observacionesPrivadas.length > 0) {
                pnlPopMotivosRechazar.Show();
                pnlCallbackMotRechazar.PerformCallback();
            }
            else {
                Alerta('Observaciones Privadas', 'Por favor agregue observaciones privadas');
            }
        }

        function treeList_CustomDataCallbackRechazar(s, e) {
            document.getElementById('treeListCountCell').innerHTML = e.result;
        }
        function treeList_SelectionChangedRechazar(s, e) {
            window.setTimeout(function () { s.PerformCustomDataCallback(''); }, 0)
        }



    </script>

    <!-- Campos Ocultos -->
    <div>
        <asp:HiddenField ID="hfIdArchivo"   runat="server" Value="0" />
        <asp:HiddenField ID="hfIdTramite"   runat="server" Value="0" />
        <asp:HiddenField ID="hfIdMesa"      runat="server" Value="0" />
        <asp:HiddenField ID="hfNombreMesa"  runat="server" Value="0" />
        <asp:HiddenField ID="hfTipoTramite" runat="server" Value="0" />
        <asp:HiddenField ID="hfAutomatico"  runat="server" Value="1" />
        <asp:HiddenField ID="hfExpediente"  runat="server" Value="0" />
    </div>

  
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>

    <!-- MODAL DE CONFIRMACION ACEPTAR -->
    <div class="modal fade confirmacion" id="ContinuarTramite" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <h4 class="modal-title" id="myModalLabel2">
                        <asp:label ID="Label4" runat="server" Text="Confirmación de movimiento">
                        </asp:label>
                    </h4>
                </div>
                <div class="modal-body text-center">
                    <h5 class="modal-title" id="myModalLabel2">
                        <asp:Label runat="server" ID="Label5" Text="¿ Deseas aceptar el tramite ?"></asp:Label>
                    </h5>
                </div>
                <div class="modal-footer text-center">
                    <div class="row text-center">
                        <button type="button" class="btn btn-default col-md-6 col-sm-6 col-xs-12" data-dismiss="modal">Cancelar</button>
                        <asp:Button ID="Button1" runat="server" Text="Aceptar" class="btn btn-primary col-md-5 col-sm-5 col-xs-12" CausesValidation="False" OnClick="btnAceptar_Click"  />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!-- MODAL DE  OPERACIONES -->
    <div class="modal fade bs-example-modal-sm" id="myModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalTitleOperacion">
                        <asp:label ID="TituloModal" runat="server" Text="Procesamiento">
                        </asp:label>
                    </h4>
                </div>
                <div class="modal-body">
                    <p><asp:Label runat="server" ID="MensajeModal" Text="El trámite se proceso correctamente"></asp:Label></p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button2" runat="server" Text="Aceptar" class="btn btn-primary" CausesValidation="False" OnClick="TramiteActualizado_Click"  />
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL DE BITACORA PRIVADA  -->
    <div class="modal fade BitacoraPrivada" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title" id="myModalLabel3">Bitácora Privada </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <asp:Repeater ID="rptBitacoraPrivada" runat="server" >
                                <HeaderTemplate>
                                    <table id="datatableBitacora" class="table table-striped table-bordered">
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
                                        <td><%#Eval("ObservacionPrivada")%></td>
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
    
    <!-- MODAL DE BITACORA PÚBLICA -->
    <div class="modal fade BitacoraPublica" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title" id="myModalLabel3">Bitácora Pública </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <asp:Repeater ID="rptBitacoraPublica" runat="server" >
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

    <!-- DATOS MOSTRADOS EN TODAS LAS MESAS -->  
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 text-left">
            <div class="x_panel">
                <div class="x_title">
                    <h2 style="color:#26B99A">Solictud. Gestión de Proveedores</h2>
                    
                    <ul class="nav navbar-right panel_toolbox">
                        <li>
                            <a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                      	</li>
                    </ul>
                    <div class="clearfix"></div>

                    <br />
                    <ul class="nav navbar-right panel_toolbox">
                        <li>
                            <asp:Label runat="server" ID="LabelFlujo" Text="" Font-Bold="True" ></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="LabelNombreMesa" Text="" Font-Bold="True" ></asp:Label>
                      	</li>
                    </ul>
                    <div class="clearfix"></div>
                </div>
                <div class="x_content text-left" >
                    <div class="row">
                        <!-- IMFORMAICON DEL TRAMITE -->
                        <div class="col-md-8 col-sm-8 col-xs-12">
                            <asp:UpdatePanel id="DatosTramiteInformacion" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <table class="table table-hover table-bordered">
                                        <thead>
                                            <tr>
                                                <th style="text-align:center; border-bottom: 1px solid #ddd; background-color:#5BC0DE; color:#2A3F54;">
                                                    Fecha Registro:
                                                </th>
                                                <th style="text-align:center; border-bottom: 1px solid #ddd; background-color:#5BC0DE; color:white">
                                                    <asp:Label ID="InfoFechaRegistro" runat="server" class="control-label" Visible="True" ></asp:Label>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                            </tr>
                                         </tbody>
                                    </table>
                                    <br />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        
                        <div class="col-md-4 col-sm-4 col-xs-12">
                            <table class="table table-hover table-bordered">
                                <thead>
                                    <tr>
                                        <th colspan="2" style="text-align:center; border-bottom: 1px solid #ddd; background-color:#5BC0DE; color:#2A3F54;">
                                            Folio: 
                                        </th>
                                        <th style="text-align:center; border-bottom: 1px solid #ddd; background-color:#5BC0DE; color:white">
                                            <asp:Label runat="server" ID="LabelFolio" Text="Folio" Font-Bold="true" class="control-label" ForeColor="white"></asp:Label>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                    </tr>
                                 </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- ACCIONES -->
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 text-left">
            <div class="x_panel">
                <div class="x_title">
                    <h2><small>Acciones </small> </h2>
                    <ul class="nav navbar-right panel_toolbox">
                        <li>
                            <a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                      	</li>
                    </ul>
                    <div class="clearfix"></div>
                </div>
                <div class="x_content text-left" >
                    <div class="row">
                        <div class="control-label col-md-5 col-sm-5 col-xs-12">
                            <strong>OBSERVACIONES PRIVADAS</strong>
                            <asp:TextBox ID="txtObservacionesPrivadas" runat="server" class="form-control" TextMode="MultiLine" Rows="5" AutoPostBack="false" onKeyUp="document.getElementById(this.id).value=document.getElementById(this.id).value.toUpperCase()"></asp:TextBox>
                            <br />
                        </div>
                        <div class="control-label col-md-7 col-sm-12 col-xs-12">
                            <div class="row">
                                <code><asp:Label runat="server" ID="Mensajes" Text=""></asp:Label></code>
                            </div>
                            <div class="row">
                                <asp:Button ID="btnAceptar" Visible="false" ValidationGroup="Observaciones" runat="server" Text="Aceptar" class="btn btn-success col-md-3 col-sm-3 col-xs-12" title="Aceptar" OnClick="btnAceptarValida_Click"/>
                                <asp:Button ID="btnSuspender" Visible="false"  runat="server" Text="Suspender" class="btn btn-warning col-md-3 col-sm-3 col-xs-12"/>
                                <asp:Button ID="btnRechazar" Visible="false" runat="server" Text="Rechazar" class="btn btn-danger col-md-3 col-sm-3 col-xs-12"/>
                            </div>
                            <div class="row">
                                <asp:Button ID="btnPausa" Visible="false"  runat="server" Text="Pausa de Trámite" class="btn btn-danger col-md-5 col-sm-5 col-xs-12"/>
                                <asp:Button ID="btnDetener" Visible="false"  runat="server" Text="Pausa de Usuario" class="btn btn-warning col-md-4 col-sm-4 col-xs-12"/>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="PanelObservacionesPubicas" runat="server" Visible="false">
                        <div class="row">
                            <div class="control-label col-md-5 col-sm-5 col-xs-12">
                                <strong>OBSERVACIONES PUBLICAS</strong>
                                <asp:TextBox ID="txtObservacionesPublicas" runat="server" ValidationGroup="Observaciones" class="form-control" TextMode="MultiLine" Rows="5" AutoPostBack="false" onkeypress="return soloLetras(event)" onKeyUp="document.getElementById(this.id).value=document.getElementById(this.id).value.toUpperCase()"></asp:TextBox>
                                <br />
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="control-label col-md-5 col-sm-5 col-xs-12">
                            <button type="button" class="btn btn-default col-md-2 col-sm-2 col-xs-12" data-toggle="modal" title="Bitácora pública" data-target=".BitacoraPublica"><i class="fa fa-unlock-alt"></i></button>
                            <button type="button" class="btn btn-default col-md-2 col-sm-2 col-xs-12" data-toggle="modal" title="Bitácora privada" data-target=".BitacoraPrivada"><i class="fa fa-lock"></i> </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    <!-- CARGA DE ARCHIVOS A EXPEDIENTE HE INSUMOS -->
    <asp:UpdatePanel ID="AnexoArchivos" runat="server" UpdateMode="Conditional" Visible="False">
        <ContentTemplate>
            <div class="col-md-12 col-sm-12 col-xs-12 text-left">
                <div class="x_panel">
                    <div class="x_title">
                        <h2><small>Archivos Anexos </small></h2>
                        <div class="clearfix"></div>
                        <div class="x_content text-left">
                            <br />
                            <div class="row">
                                <div class=" profile_details">
                                    <div class="col-md-6 col-sm-6 col-xs-12 well profile_view">
                                        <div class="col-xs-12 bottom text-center">
                                            <h4 class="brief">ARCHIVOS CON DOCUMENTOS REQUERIDOS</h4><br />
                                        </div>
                                        <div class="right col-xs-12 text-left">
                                            <asp:UpdatePanel ID="PnlArchivosAnexos" runat="server">
                                                <ContentTemplate>
                                                    <fieldset>
                                                    <asp:Label ID="lblDocumentosRequeridos" runat="server" Text="Archivos (*.PDF, *.JPG, *.PNG)"></asp:Label>
                                                    <asp:FileUpload ID="fileUpDocumento" runat="server"></asp:FileUpload>
                                                    <code><asp:Label ID="LabRespuestaArchivosCarga" runat="server" Text =""></asp:Label></code>
                                                    <br />
                                                    <asp:Button ID="btnSubirDocumento" runat="server" Text="Subir" class="btn btn-primary" CausesValidation="False"/><br />
                                                    <asp:ListBox ID="lstDocumentos" runat="server" Height="100px" Width="100%" SelectionMode="Single" class="select2_multiple form-control">
                                                    </asp:ListBox>
                                                    <br />
                                                    <asp:Button ID="btnEliminaDocumento" runat="server" Text="Eliminar" class="btn btn-danger" CausesValidation="False" />
                                                </fieldset>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSubirDocumento" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12 well profile_view">
                                        <div class="col-xs-12 bottom text-center">
                                            <h4 class="brief">ARCHIVOS ADICIONALES</h4><br />
                                        </div> 
                                        <div class="right col-xs-12 text-left">
                                            <asp:CheckBox ID="CheckBoxInsumos"  runat="server" AutoPostBack="True" Text="¿Desea agregar archivos adicionales?" />
                                            <asp:UpdatePanel ID="PanelInsumos" runat="server" Visible="False">
                                                <ContentTemplate>
                                                    <fieldset>
                                                        <asp:FileUpload ID="fileUpInsumo" runat="server"></asp:FileUpload>
                                                        <code><asp:Label ID="MensajeInsumos" runat="server" Text =""></asp:Label></code>
                                                        <br />
                                                        <asp:Button ID="btnSubirInsumo" runat="server" Text="Subir" class="btn btn-primary" CausesValidation="False"/><br />
                                                        <asp:ListBox ID="lstInsumos" runat="server" Height="100px" Width="100%" class="select2_multiple form-control" SelectionMode="Single">
                                                        </asp:ListBox>
                                                        <br />
                                                        <asp:Button ID="btnEliminaInsumo" runat="server" Text="Eliminar" class="btn btn-danger" CausesValidation="False"/>
                                                    </fieldset>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSubirInsumo" />
                                                </Triggers>
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

    <!-- EXPEDIENTE PDF -->
    <asp:Panel ID="Expediente" runat="server" Visible="true">
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 text-left">
            <div class="x_panel">
                <div class="x_title">
                    <h2><small>Expediente</small> </h2>
                    <ul class="nav navbar-right panel_toolbox">
                        <li>
                            <a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                      	</li>
                    </ul>
                    <div class="clearfix"></div>
                </div>
                <div class="x_content text-left" style="height: 550px;">
                    <asp:Literal ID="ltMuestraPdf" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
    </asp:Panel>
    
    
    <!-- Botón de Pausa -->
    <dx:ASPxPopupControl ID="pnlPopPausaTramite" 
	        runat="server" 
	        CloseAction="CloseButton" 
	        HeaderText="Pausar Trámite" 
	        ShowFooter="True" 
	        Theme="iOS" 
	        Width="350px" 
	        ClientInstanceName="pnlPopPausaTramite" 
	        Modal="True" 
	        PopupHorizontalAlign="WindowCenter" 
	        PopupVerticalAlign="WindowCenter" 
	        FooterText="">
	    <ContentStyle>
		    <Paddings Padding="5px" />
	    </ContentStyle>
	    <ContentCollection>
		    <dx:PopupControlContentControl runat="server">
                <p>
                    <strong>OBSERVACIONES PÚBLICAS</strong>
                </p>
                <asp:TextBox ID="txtObservacionesPublicasPausar" runat="server" TextMode="MultiLine" Width="95%" Height="50px" onkeypress="return soloLetras(event)" onKeyUp="document.getElementById(this.id).value=document.getElementById(this.id).value.toUpperCase()"> </asp:TextBox>
		    </dx:PopupControlContentControl>
	    </ContentCollection>
	    <FooterTemplate>
		    <div style="text-align:right;">
                <br />
                <asp:Button ID="btnPausaTramite" runat="server" Text="Pausar" CausesValidation="false" class="btn btn-warning" OnClientClick="pnlPopPausaTramite.Hide();" />
			    <br />&nbsp;
		    </div>
	    </FooterTemplate>
    </dx:ASPxPopupControl>


    <!-- Botones Emergentes SUSPENDER -->
    <dx:ASPxPopupControl ID="pnlPopMotivosSuspender" 
					    runat="server" 
					    CloseAction="CloseButton" 
					    HeaderText="Motivos SUSPENDER" 
					    ShowFooter="True" 
					    Theme="iOS" 
					    Width="350px" 
					    ClientInstanceName="pnlPopMotivosSuspender" 
					    Modal="True" 
					    PopupHorizontalAlign="WindowCenter" 
					    PopupVerticalAlign="WindowCenter" 
					    FooterText="">
	    <ContentStyle>
		    <Paddings Padding="5px" />
	    </ContentStyle>
	    <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxCallbackPanel ID="pnlCallbackMotSuspender" 
								    runat="server" 
					                ClientInstanceName="pnlCallbackMotSuspender" 
				                    Width="100%" 
								    OnCallback="pnlCallbackMotSuspender_Callback">
				    <PanelCollection>
	                    <dx:PanelContent runat="server">
						    <dx:ASPxTreeList ID="treeListSuspender" runat="server" AutoGenerateColumns="False" KeyFieldName="Id" OnCustomDataCallback="treeList_CustomDataCallbackSuspender" OnDataBound="treeList_DataBoundSuspender" ParentFieldName="idParent" Width="100%">
                                <Columns>
                                    <dx:TreeListDataColumn AutoFilterCondition="Default" Caption="Motivos de Suspensión" FieldName="motivoRechazo" ShowInCustomizationForm="True" ShowInFilterControl="Default" VisibleIndex="0"></dx:TreeListDataColumn>
                                </Columns>
                                <settingsbehavior allowautofilter="True" expandcollapseaction="NodeDblClick"></settingsbehavior>
                                <settingscustomizationwindow caption="" popuphorizontalalign="RightSides" popupverticalalign="BottomSides"></settingscustomizationwindow>
                                <settingsselection enabled="True"></settingsselection>
                                <settingspopupeditform verticaloffset="-1"></settingspopupeditform>
                                <settingspopup>
                                    <editform verticaloffset="-1"></editform>
                                </settingspopup>
                                <clientsideevents customdatacallback="treeList_CustomDataCallbackSuspender" selectionchanged="treeList_SelectionChangedSuspender"></clientsideevents>
                            </dx:ASPxTreeList>
                            <br />
                            <asp:Label ID="lblObservacionesPublicasSuspender" runat="server" Text="OBSERVACIONES PÚBLICAS" Font-Bold="True"></asp:Label>
                            <asp:TextBox ID="txtObservacionesPublicasSuspender" runat="server" TextMode="MultiLine" Width="98%" Height="50px" onkeypress="return soloLetras(event)" onKeyUp="document.getElementById(this.id).value=document.getElementById(this.id).value.toUpperCase()"></asp:TextBox>
					    </dx:PanelContent>
				    </PanelCollection>
	            </dx:ASPxCallbackPanel>
		    </dx:PopupControlContentControl>
        </ContentCollection>
	    <FooterTemplate>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                    <div style="text-align:right;">
                        <br />&nbsp;
                        <asp:Button ID="btnAplicarSuspender" runat="server" CausesValidation="false" Text="Aplicar Suspensión" class="btn btn-warning"  OnClick="btnAplicarSuspender_Click"/>
                        <br />&nbsp;
		            </div>
                </ContentTemplate>
            </asp:UpdatePanel>
	    </FooterTemplate>
    </dx:ASPxPopupControl>
    
    <!-- Botones Emergentes RECHAZAR -->
    <dx:ASPxPopupControl ID="pnlPopMotivosRechazar" 
					    runat="server" 
					    CloseAction="CloseButton" 
					    HeaderText="Motivos de Rechazo" 
					    ShowFooter="True" 
					    Theme="iOS" 
					    Width="350px" 
					    ClientInstanceName="pnlPopMotivosRechazar" 
					    Modal="True" 
					    PopupHorizontalAlign="WindowCenter" 
					    PopupVerticalAlign="WindowCenter" 
					    FooterText="">
	    <ContentStyle>
		    <Paddings Padding="5px" />
	    </ContentStyle>
	    <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxCallbackPanel ID="pnlCallbackMotRechazar" 
								    runat="server" 
					                ClientInstanceName="pnlCallbackMotRechazar" 
				                    Width="100%" 
								    OnCallback="pnlCallbackMotRechazar_Callback">
				    <PanelCollection>
	                    <dx:PanelContent runat="server">
                            <dx:ASPxTreeList ID="treeListRechazar" runat="server" AutoGenerateColumns="False" KeyFieldName="Id" OnCustomDataCallback="treeList_CustomDataCallbackRechazo" OnDataBound="treeList_DataBoundRechazo" ParentFieldName="idParent" Width="100%">
                                <Columns>
                                    <dx:TreeListDataColumn AutoFilterCondition="Default" Caption="Motivos de rechazo" FieldName="motivoRechazo" ShowInCustomizationForm="True" ShowInFilterControl="Default" VisibleIndex="0"></dx:TreeListDataColumn>
                                </Columns>
                                <settingsbehavior allowautofilter="True" expandcollapseaction="NodeDblClick"></settingsbehavior>
                                <settingscustomizationwindow caption="" popuphorizontalalign="RightSides" popupverticalalign="BottomSides"></settingscustomizationwindow>
                                <settingsselection enabled="True"></settingsselection>
                                <settingspopupeditform verticaloffset="-1"></settingspopupeditform>
                                <settingspopup>
                                    <editform verticaloffset="-1"></editform>
                                </settingspopup>
                                <clientsideevents customdatacallback="treeList_CustomDataCallbackSuspender" selectionchanged="treeList_SelectionChangedSuspender"></clientsideevents>
                            </dx:ASPxTreeList>
						    <br />
                            <asp:Label ID="lblObservacionesPublicasRechazar" runat="server" Text="OBSERVACIONES PÚBLICAS" Font-Bold="True"></asp:Label>
                            <asp:TextBox ID="txtObservacionesPublicasRechazara" runat="server" TextMode="MultiLine" Width="98%" Height="50px" onkeypress="return soloLetras(event)" onKeyUp="document.getElementById(this.id).value=document.getElementById(this.id).value.toUpperCase()"></asp:TextBox>
					    </dx:PanelContent>
				    </PanelCollection>
	            </dx:ASPxCallbackPanel>
		    </dx:PopupControlContentControl>
        </ContentCollection>
	    <FooterTemplate>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                    <div style="text-align:right;">
                        <br />&nbsp;
                        <asp:Button ID="btnAplicarRechazar" CausesValidation="false" runat="server" Text="Aplicar Recahzo" class="btn btn-warning" OnClick="btnAplicarRechazar_Click"/>
                        <br />&nbsp;
		            </div>
                </ContentTemplate>
            </asp:UpdatePanel>
	    </FooterTemplate>
    </dx:ASPxPopupControl>


</ContentTemplate>
</asp:UpdatePanel>
        
</asp:Content>