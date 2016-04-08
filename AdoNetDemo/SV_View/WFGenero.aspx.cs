using AdoNetDemo;
using SVD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SV_View
{
    public partial class WfGenero : Page
    {
        private GeneroRepositorio generoRepositorio { get { return new GeneroRepositorio(); } }
        private static List<Genero> _generos;
        private static bool _ascendente;
        private static string _filter = string.Empty;
        private static string _columnOrdernate = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            _ascendente = false;
            BindData();
        }

        protected void btnSalvar_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeGenero.Text.Trim()))
            {
                var genero = new Genero { Descricao = txtNomeGenero.Text };
                generoRepositorio.Insert(genero);
                txtNomeGenero.Text = string.Empty;

                var sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("alert('Record Added Successfully');");
                sb.Append("$('#myModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "AddHideModalScript", sb.ToString(), false);

                Response.Redirect(Request.RawUrl);
            }
            else
                upModal.Update();
        }

        protected void dgvGenero_PreRender(object sender, EventArgs e)
        {
            try
            {
                dgvGenero.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void dgvGenero_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvGenero.PageIndex = e.NewPageIndex;
            BindData();
        }

        private void BindData()
        {
            _generos = string.IsNullOrEmpty(txtFilter.Text) ? generoRepositorio.GetAll() : generoRepositorio.GetAll(txtFilter.Text);
            _columnOrdernate = "ID";
            _generos.Sort(new Ordenator<Genero>(_columnOrdernate, _ascendente));
            dgvGenero.DataSource = _generos;
            dgvGenero.DataBind();
        }

        protected void dgvGenero_Sorting(object sender, GridViewSortEventArgs e)
        {
            _columnOrdernate = e.SortExpression;

            _ascendente = !_ascendente;

            _generos.Sort(new Ordenator<Genero>(_columnOrdernate, _ascendente));
            dgvGenero.DataSource = _generos;
            dgvGenero.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            _generos = string.IsNullOrEmpty(txtFilter.Text) ? generoRepositorio.GetAll() : generoRepositorio.GetAll(txtFilter.Text);

            dgvGenero.DataSource = _generos;
            dgvGenero.DataBind();
        }
    }
}