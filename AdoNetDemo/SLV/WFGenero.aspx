<%@ Page Language="C#" MasterPageFile="~/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="WFGenero.aspx.cs" Inherits="SV_View.WfGenero" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <link href="Content/ViewTable.css" rel="stylesheet" />
    <script src="Scripts/ViewTableFilter.js"></script>
    <style>
        .center-table {
            margin: 0 auto !important;
            float: none !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form class="form-horizontal" id="myForm" runat="server" role="form">
        <div class="container">
            <h2>Gênero</h2>
            <br />
            <hr />
            <!-- Trigger the modal with a button -->
            <button type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#myModal" data-whatever="@mdo">Adicionar Gênero</button>

            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div class="row">
                <asp:UpdatePanel ID="upModal" runat="server">
                    <ContentTemplate>
                        <!-- Modal -->
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label><br />
                        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                            aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title" id="myModalLabel">Cadastro de Gênero</h4>
                                    </div>
                                    <div class="modal-body">
                                        <asp:TextBox ID="txtNomeGenero" runat="server" placeholder="Nome do gênero" class="form-control"></asp:TextBox><br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txtNomeGenero"
                                            ErrorMessage="Digite o gênero."
                                            ForeColor="Red">
                                        </asp:RequiredFieldValidator><br />
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">
                                            Fechar</button>
                                        <%--<button type="button"  class="btn btn-primary">
                                        Save changes</button>--%>
                                        <asp:Button ID="btnSalvar" Text="Salvar" class="btn btn-default" OnClick="btnSalvar_ServerClick" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Gêneros</h3>
                            <div class="pull-right">
                                <span class="clickable filter" data-toggle="tooltip" title="Toggle table filter" data-container="body">
                                    <i class="glyphicon glyphicon-filter"></i>
                                </span>
                            </div>
                        </div>
                        <div id="Div1" class="panel-body" runat="server">
                            <div id="Div2" class="input-group" runat="server">
                                <asp:TextBox ID="txtFilter" runat="server" placeholder="Filtrar Gênero" class="form-control" data-action="filter"
                                    data-filters="#dev-table" />
                                <span id="Span1" class="input-group-btn" runat="server">
                                    <asp:LinkButton ID="btnFiltrar" CssClass="btn btn-default" OnClick="btnFiltrar_Click" runat="server" CausesValidation="false">
                                        <span class="glyphicon glyphicon-search"></span>
                                    </asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <asp:GridView ID="dgvGenero" runat="server"
                            AllowSorting="true" AllowPaging="true"
                            PageSize="10" CssClass="table table-hover table-striped"
                            GridLines="None" AutoGenerateColumns="False"
                            UseAccessibleHeader="true" OnPreRender="dgvGenero_PreRender"
                            OnPageIndexChanging="dgvGenero_PageIndexChanging" OnSorting="dgvGenero_Sorting">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Código" SortExpression="ID" />
                                <asp:BoundField DataField="Descricao" HeaderText="Gênero" SortExpression="Descricao" />
                            </Columns>
                            <PagerSettings Mode="Numeric" />
                            <RowStyle CssClass="cursor-pointer" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
