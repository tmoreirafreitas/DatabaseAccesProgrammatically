using AdoNetDemo;
using SVD.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;

namespace SV_View
{
    public partial class WFCategoria : System.Web.UI.Page
    {
        private CategoriaRepositorio categoriaRepositorio { get { return new CategoriaRepositorio(); } }
        private static List<SVD.Model.Categoria> categorias;
        private static bool ascendente;
        private static string filter = string.Empty;
        private static string columnOrdernate = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ascendente = false;
                BindData();
            }
        }

        protected void btnSalvar_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeCategoria.Text.Trim()) && !string.IsNullOrEmpty(txtValor.Text.Trim()))
            {
                SVD.Model.Categoria categoria = new SVD.Model.Categoria 
                { 
                    Descricao = txtNomeCategoria.Text, 
                    ValorLocacao = decimal.Parse(txtValor.Text.Replace(',', '.'), CultureInfo.InvariantCulture) 
                };
                categoriaRepositorio.Insert(categoria);
                txtNomeCategoria.Text = string.Empty;           

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("alert('Record Added Successfully');");
                sb.Append("$('#myModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);

                Response.Redirect(Request.RawUrl);
            }
            else
                upModal.Update();
        }

        protected void dgvCategoria_PreRender(object sender, EventArgs e)
        {
            try
            {
                dgvCategoria.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void dgvCategoria_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvCategoria.PageIndex = e.NewPageIndex;
            BindData();
        }

        private void BindData()
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
                categorias = categoriaRepositorio.GetAll();
            else
                categorias = categoriaRepositorio.GetAll(txtFilter.Text);
            columnOrdernate = "Id";
            categorias.Sort(new Ordenator<SVD.Model.Categoria>(columnOrdernate, ascendente));
            dgvCategoria.DataSource = categorias;
            dgvCategoria.DataBind();
        }

        protected void dgvCategoria_Sorting(object sender, GridViewSortEventArgs e)
        {
            columnOrdernate = e.SortExpression;

            if (ascendente)
                ascendente = false;
            else
                ascendente = true;

            categorias.Sort(new Ordenator<SVD.Model.Categoria>(columnOrdernate, ascendente));
            dgvCategoria.DataSource = categorias;
            dgvCategoria.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
                categorias = categoriaRepositorio.GetAll();

            else
                categorias = categoriaRepositorio.GetAll(txtFilter.Text);

            dgvCategoria.DataSource = categorias;
            dgvCategoria.DataBind();
        }
    }
}