using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SVD.Model;
using AdoNetDemo;
using AjaxControlToolkit.Bundling;
using AjaxControlToolkit.Design;
using AjaxControlToolkit.HtmlEditor;
using AjaxControlToolkit.MaskedEditValidatorCompatibility;
using AjaxControlToolkit.ToolboxIcons;

namespace SV_View
{
    public partial class WfFilme : Page
    {
        private static int _i = 1;
        private static readonly List<TableRow> Linhas = new List<TableRow>();
        private static FilmeRepositorio FilmeRepositorio { get { return new FilmeRepositorio(); } }
        private static CategoriaRepositorio CategoriaRepositorio { get { return new CategoriaRepositorio(); } }
        private static GeneroRepositorio GeneroRepositorio { get { return new GeneroRepositorio(); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (ViewState["categorias"] == null)
                ViewState["categorias"] = CategoriaRepositorio.GetAll();

            if (ViewState["generos"] == null)
                ViewState["generos"] = GeneroRepositorio.GetAll();

            FillDropDownList();
            upTable.Update();
        }

        private void FillDropDownList()
        {
            foreach (var ddl in tab_logic.Rows.Cast<TableRow>()
                .SelectMany(row => row.Cells.Cast<TableCell>()
                    .SelectMany(cell => cell.Controls.OfType<DropDownList>()
                        .Select(control => control))))
            {
                Session[ddl.ID] = ddl.SelectedValue;

                if (ddl.ID.Contains("Genero"))
                {
                    if (string.IsNullOrEmpty(ddl.Text))
                    {
                        if (ViewState["generos"] != null)
                            ddl.DataSource = (List<Genero>)ViewState["generos"];
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(ddl.Text))
                    {
                        if (ViewState["categorias"] != null)
                            ddl.DataSource = (List<Categoria>)ViewState["categorias"];
                    }
                }

                ddl.DataValueField = "ID";
                ddl.DataTextField = "Descricao";
                ddl.DataBind();
                ddl.EnableViewState = true;
            }
        }

        protected void DeletarLinha_Click(object sender, EventArgs e)
        {
            if (Linhas.Count <= 0) return;
            Linhas.Reverse();
            Linhas.RemoveAt(0);
            Linhas.Reverse();
            _i--;

            //Adiciona todas as Linhas na tabela.
            foreach (var linha in Linhas)
                tab_logic.Rows.Add(linha);
        }

        protected void AdicionarLinha_Click(object sender, EventArgs e)
        {
            _i++;

            //Cria a linha
            var row = new TableRow();

            //Cria as células
            var cellLinha = new TableCell();
            var cellTitulo = new TableCell();
            var cellDuracao = new TableCell();
            var cellCategoria = new TableCell();
            var cellGenero = new TableCell();

            //Adiciona as células na linha
            row.Cells.AddAt(0, cellLinha);
            row.Cells.AddAt(1, cellTitulo);
            row.Cells.AddAt(2, cellDuracao);
            row.Cells.AddAt(3, cellCategoria);
            row.Cells.AddAt(4, cellGenero);

            //Seta os valores ou os controles para as células 
            cellLinha.Text = _i.ToString();

            var tbTitulo = new TextBox
            {
                CssClass = "form-control",
                ID = "txtTitulo" + _i
            };
            tbTitulo.Attributes.Add("placeholder", "Título");
            cellTitulo.Controls.Add(tbTitulo);

            var tbDuracao = new TextBox
            {
                CssClass = "form-control",
                ID = "txtDuracao" + _i
            };
            tbDuracao.Attributes.Add("placeholder", "Duração");
            cellDuracao.Controls.Add(tbDuracao);

            var ddlCategoria = new DropDownList
            {
                AutoPostBack = false,
                ID = "ddlCategoria" + _i,
                CssClass = "form-control"
            };
            cellCategoria.Controls.Add(ddlCategoria);
            if (ViewState["categorias"] != null)
            {
                ddlCategoria.DataSource = (List<Categoria>)ViewState["categorias"];
                ddlCategoria.DataValueField = "Id";
                ddlCategoria.DataTextField = "Descricao";
                ddlCategoria.DataBind();
            }

            var ddlGenero = new DropDownList
            {
                AutoPostBack = false,
                ID = "ddlGenero" + _i,
                CssClass = "form-control"
            };
            cellGenero.Controls.Add(ddlGenero);
            if (ViewState["generos"] != null)
            {
                ddlGenero.DataSource = (List<Genero>)ViewState["generos"];
                ddlGenero.DataValueField = "Id";
                ddlGenero.DataTextField = "Descricao";
                ddlGenero.DataBind();
            }

            //Adicionas alinha na lista
            Linhas.Add(row);

            //Adiciona todas as Linhas na tabela.
            foreach (var linha in Linhas)
                tab_logic.Rows.Add(linha);
        }
        
        protected void Salvar_Click(object sender, EventArgs e)
        {

        }
    }
}