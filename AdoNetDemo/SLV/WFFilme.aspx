<%@ Page Title="" Language="C#" MasterPageFile="~/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="WFFilme.aspx.cs" Inherits="SV_View.WfFilme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form class="form-horizontal" id="myForm" runat="server" role="form" enableviewstate="true">
        <div class="container">
            <div class="row clearfix">
                <div class="col-md-9 column">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="upTable" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Table ID="tab_logic" runat="server" CssClass="table table-bordered table-hover">
                                <asp:TableRow>
                                    <asp:TableCell ID="NumeroDaLinha">#</asp:TableCell>
                                    <asp:TableCell ID="Titulo">Título</asp:TableCell>
                                    <asp:TableCell ID="Duracao">Duração</asp:TableCell>
                                    <asp:TableCell ID="Categoria">Categoria</asp:TableCell>
                                    <asp:TableCell ID="Genero">Gênero</asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell ID="NumeroDaLinha0">1</asp:TableCell>
                                    <asp:TableCell ID="Titulo0">
                                        <asp:TextBox ID="txtTitulo0" runat="server" CssClass="form-control" placeholder="Título"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell ID="Duracao0">
                                        <asp:TextBox ID="txtDuracao0" runat="server" CssClass="form-control" placeholder="Duração"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell ID="Categoria0">
                                        <asp:DropDownList ID="ddlCategoria0" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell ID="Genero0">
                                        <asp:DropDownList ID="ddlGenero0" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="Div2" class="input-group col-md-9 column">
                <asp:LinkButton runat="server" ID="AdicionarLinha" CssClass="btn btn-default pull-left" Text="Adicionar" OnClick="AdicionarLinha_Click" CausesValidation="false" />
                <asp:LinkButton runat="server" ID="DeletarLinha" CssClass="btn btn-default pull-right" Text="Deletar" OnClick="DeletarLinha_Click" CausesValidation="false" />
            </div>
            <hr />
            <asp:LinkButton runat="server" ID="Salvar" CssClass="btn btn-default pull-left" Text="Salvar" OnClick="Salvar_Click" CausesValidation="false" />
        </div>
    </form>
</asp:Content>
