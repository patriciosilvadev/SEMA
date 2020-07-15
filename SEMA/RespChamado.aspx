﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RespChamado.aspx.cs" Inherits="SEMA.RespChamado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>  
    <style>
        .nav-tabs>li.active>a {
            background-color: #478978 !important;
            color: #ffffff !important;
        }
    </style>
    <div class="wrapper">
  <div class="content-wrapper">
    <section class="content-header">
      <h1>Responder Chamado</h1>
        <br />
      <ol class="breadcrumb">
        <li><a href="home.aspx"><i class="fas fa-home"></i> Home</a></li>
        <li class="active">Chamados</li>
      </ol>
    </section>
   <div class="nav-tabs-custom">
            <ul class="nav nav-tabs">
              <li class="active"><a href="#tab_1" data-toggle="tab">Resposta ao Chamado</a></li>
              <li><a href="#tab_2" data-toggle="tab">Informações do Chamado</a></li>
                <li><a href="#tab_3" data-toggle="tab">Histórico de Mensagens</a></li>
              <li class="pull-right"><a href="#" class="text-muted"><i class="far fa-bullhorn"></i></a></li>
            </ul>
            <div class="tab-content">
      <div class="tab-pane active" id="tab_1">          
      <section class="content">
      <div class="container-fluid">
        <div class="box box-default">
          <div class="box-header with-border" >
            <h3 class="box-title"><i class="far fa-bullhorn"></i></h3>
            <div class="box-tools">
              <asp:Button Text="Salvar" CssClass="btn btn-sm btn-primary" runat="server" ID="btnSalvar" OnClick="btnSalvar_Click" />
              <asp:Button Text="Voltar" CssClass="btn btn-sm btn-secondary" runat="server" ID="btnVoltar" OnClick="btnVoltar_Click" />
              
            </div>
          </div>
          <!-- /.box-header -->
          <div class="box-body">
            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                      <label>Responsável Pelo Chamado</label>
                        <asp:DropDownList runat="server" ID="cboxUsuario" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                  <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="cboxStatus" EventName="SelectedIndexChanged" />
                      </Triggers>
                      <ContentTemplate>
                  <div class="col-md-4">
                    <div class="form-group">
                      <label>Status</label>
                        <asp:DropDownList runat="server" CssClass="form-control" ID="cboxStatus" OnSelectedIndexChanged="cboxStatus_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Aberto" Value="Aberto"  />
                            <asp:ListItem Text="Em Atendimento" Value="Em Atendimento" />
                            <asp:ListItem Text="Finalizado" Value="Finalizado" />
                            <asp:ListItem Text="Pendente" Value="Pendente" />
                        </asp:DropDownList>
                    </div>
                </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
              <div class="row">
                  <div class="col-md-8">
                    <div class="form-group">
                        <CKEditor:CKEditorControl ID="descricao" runat="server"></CKEditor:CKEditorControl>
                    </div>
                </div>
                      <div class="col-md-4" style="height:306px;">
                    <div class="form-group">
                        <asp:Label Text="" runat="server" ID="lblCaminhoImg" />
                        <asp:ImageButton runat="server" ID="Image1" Width="294.33px" Height="280px" data-toggle="modal" data-target="#modal-default"  />
                        <asp:FileUpload runat="server" ID="img" ToolTip="Selecione uma Imagem" CssClass="btn btn-secondary" ClientIDMode="Static" onchange="this.form.submit()"    />
                        <asp:Label runat="server" id="StatusLabel" text="" ForeColor="Red" />
                    </div>
                </div>
            </div>
        </div>
      </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Image1" />
    </Triggers>
    <ContentTemplate>
        <div id="modal-default" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:Image runat="server" ID="imgSel" CssClass="img-responsive"/>
                </div>
            </div>
          </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
     </section>
       </div>
              <!-- /.tab-pane -->
    <div class="tab-pane" id="tab_2">
                
    <section class="content">
      <div class="container-fluid">
        <div class="box box-default">
          <div class="box-header with-border" >
            <h3 class="box-title"><i class="far fa-bullhorn"></i></h3>
            <div class="box-tools">
              <asp:Button Text="Voltar" CssClass="btn btn-sm btn-secondary" runat="server" ID="btnVoltar_resp" OnClick="btnVoltar_resp_Click" />
            </div>
          </div>
          <!-- /.box-header -->
          <div class="box-body">
            <div class="row">
                <div class="col-md-2">
                    <div class="form-group">
                      <label>Nº Protocolo</label>
                        <asp:TextBox runat="server" ID="resp_txtProtocolo" CssClass="form-control"  ReadOnly="true"/>
                    </div>
                </div>
                  <div class="col-md-4">
                    <div class="form-group">
                      <label>Nome</label>
                        <asp:TextBox runat="server" ID="resp_nome" CssClass="form-control" ReadOnly="true"  />
                    </div>
                  </div>
                <!-- /.form-group -->
                <div class="col-md-2">
                    <div class="form-group">
                      <label>E-Mail</label>
                        <asp:TextBox runat="server" ID="resp_email" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>
               <div class="col-md-2">
                    <div class="form-group">
                      <label>Telefone</label>
                        <asp:TextBox runat="server" ID="resp_telefone" CssClass="form-control" ReadOnly="true" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                      <label>CPF</label>
                        <asp:TextBox runat="server" ID="resp_cpf" CssClass="form-control" data-inputmask='"mask": "999.999.999-99"' data-mask  ReadOnly="true"/>
                    </div>
                </div>
                <!-- /.form-group -->
            </div>
              <div class="row">
                <div class="col-md-4">
                <div class="form-group">
                  <label>Assunto</label>
                    <asp:DropDownList runat="server" ID="resp_cboxAssunto" CssClass="form-control" Enabled="false" >
                    </asp:DropDownList>
                </div>
                </div>
                  <div class="col-md-6">
                     <div class="form-group">
                       <label>Tópico</label>
                          <asp:DropDownList runat="server" ID="resp_cboxTopico" CssClass="form-control" Enabled="false">
                          </asp:DropDownList>
                     </div>
                  </div>
                  <div class="col-md-2">
                    <div class="form-group">
                      <label>Status</label>
                        <asp:DropDownList runat="server" CssClass="form-control" ID="resp_cboxStatus" Enabled="false">
                            <asp:ListItem Text="Aberto" Value="Aberto"  />
                            <asp:ListItem Text="Em Atendimento" Value="Em Atendimento" />
                            <asp:ListItem Text="Finalizado" Value="Finalizado" />
                            <asp:ListItem Text="Pendente" Value="Pendente" />
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
      </div>        
    </div>
   </section>                 
 </div>
     <!-- /.tab-pane -->
                <style>          
                    /* Chat containers */
                    .container1 {
                      border: 2px solid #dedede;
                      background-color: #f1f1f1;
                      border-radius: 5px;
                      padding: 5px;
                      margin: 10px auto!important;
                      width:80%!important;
 
                    }

                    /* Darker chat container */
                    .darker {
                      border-color: #ccc;
                      background-color: #ddd;
                    }

                    /* Clear floats */
                    .container1::after {
                      content: "";
                      clear: both;
                      display: table;
                    }

                    /* Style images */
                    .container1 img {
                      float: left;
                      max-width: 60px;
                      width: 100%;
                      margin-right: 20px;
                      border-radius: 50%;
                    }

                    /* Style the right image */
                    .container1 img.right {
                      float: right;
                      margin-left: 20px;
                      margin-right:0;
                    }

                    /* Style time text */
                    .time-right {
                      float: right;
                      color: #aaa;
                    }

                    /* Style time text */
                    .time-left {
                      float: left;
                      color: #999;
                    }
                </style>                 
                <div class="tab-pane" id="tab_3">
<div class="container1">
  <img src="/dist/img/cidadao.jpg" alt="Avatar">
  <p>Hello. How are you today?</p>
  <span class="time-right">11:00</span>
</div>

<div class="container1 darker">
  <img src="/dist/img/sema.jpg" alt="Avatar" class="right">
  <p>Hey! I'm fine. Thanks for asking!</p>
  <span class="time-left">11:01</span>
</div>

<div class="container1">
  <img src="/dist/img/cidadao.jpg" alt="Avatar">
  <p>Sweet! So, what do you wanna do today?</p>
  <span class="time-right">11:02</span>
</div>

<div class="container1 darker">
  <img src="/dist/img/sema.jpg" alt="Avatar" class="right">
  <p>Nah, I dunno. Play soccer.. or learn more coding perhaps?</p>
  <span class="time-left">11:05</span>
</div>


                </div>
    </div>
    </div>
      </div>
    </div>

    <script type="text/javascript">
        function sucesso() {
            toastr.options = {
              "closeButton": false,
              "debug": false,
              "newestOnTop": false,
              "progressBar": true,
              "positionClass": "toast-top-full-width",
              "preventDuplicates": false,
              "onclick": null,
              "showDuration": "300",
              "hideDuration": "1000",
              "timeOut": "5000",
              "extendedTimeOut": "1000",
              "showEasing": "swing",
              "hideEasing": "linear",
              "showMethod": "slideDown",
              "hideMethod": "slideUp"
            }
            toastr["success"]("<%= mensagem %>")
        };
        
    </script>
    <script type="text/javascript">
        function erro() {
            toastr.options = {
              "closeButton": false,
              "debug": false,
              "newestOnTop": false,
              "progressBar": true,
              "positionClass": "toast-top-full-width",
              "preventDuplicates": false,
              "onclick": null,
              "showDuration": "300",
              "hideDuration": "1000",
              "timeOut": "5000",
              "extendedTimeOut": "1000",
              "showEasing": "swing",
              "hideEasing": "linear",
              "showMethod": "slideDown",
              "hideMethod": "slideUp"
            }
            toastr["error"]("<%= mensagem %>")
        };
    </script>
    <script type="text/javascript">
        function acessoNegado() {
            toastr.options = {
              "closeButton": false,
              "debug": false,
              "newestOnTop": false,
              "progressBar": true,
              "positionClass": "toast-top-full-width",
              "preventDuplicates": false,
              "onclick": null,
              "showDuration": "300",
              "hideDuration": "1000",
              "timeOut": "5000",
              "extendedTimeOut": "1000",
              "showEasing": "swing",
              "hideEasing": "linear",
              "showMethod": "slideDown",
              "hideMethod": "slideUp"
            }
            toastr["info"]("Acesso restrito a usuarios Administradores. ", "Erro")
        };
    </script>
    
    <script>
    $('[data-mask]').inputmask()
    </script>

</asp:Content>