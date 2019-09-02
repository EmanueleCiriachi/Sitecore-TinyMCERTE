using Sitecore.Resources;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace TinyMCERTE.Commands {
    public class InsertImage : WebEditCommand {
        public override void Execute(CommandContext context) {
            Sitecore.Context.ClientPage.Start((object)this, "Run", context.Parameters);
        }

        protected static void Run(ClientPipelineArgs args) {
            if (args.IsPostBack) {
                if (!args.HasResult)
                    return;
                SheerResponse.Eval("window.parent.Sitecore.PageModes.ChromeManager.handleMessage('chrome:field:imageinserted',{{html:{0}}})", (object)args.Result);
            } else {
                UrlString urlString = ResourceUri.Parse("control:TinyMCE.InsertImage").ToUrlString();
                urlString.Add("mo", "webedit");
                urlString.Add("la", args.Parameters["language"]);
                SheerResponse.ShowModalDialog(urlString.ToString(), "1200", "700", string.Empty, true);
                args.WaitForPostBack();
            }
        }
    }
}
