using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SV_View
{
    public partial class TemplatePrincipal : System.Web.UI.MasterPage
    {
        private List<HtmlGenericControl> ctrls = new List<HtmlGenericControl>();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            ctrls = new List<HtmlGenericControl>{
                acao,
                animacao,
                aventura,
                biografia,
                classicos,
                comedia,
                comediaRomantica,
                crime,
                documentario,
                drama,
                guerra,
                fantasia,
                faroeste,
                ficcaoCientifica,
                musical,
                suspense,
                terror,
                wfCadastro,
                wfCategoria,
                wfFilme,
                wfGenero,
                wfHome,
                wfSocio
            };

            base.OnInit(e);
        }

        protected void HtmlAnchor_Click(Object sender, EventArgs e)
        {
            foreach (var ctrl in ctrls)
                ctrl.Attributes.Remove("class");

            HtmlGenericControl selectedMenuItem = (HtmlGenericControl)((HtmlAnchor)sender).Parent;
            if (ctrls.Contains(selectedMenuItem))
                selectedMenuItem.Attributes.Add("class", "active");
        }
    }
}