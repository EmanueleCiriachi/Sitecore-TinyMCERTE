using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.WebControls.Ribbons;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using Sitecore;

namespace TinyMCERTE {
    public class TinyMCERTEType : CustomField {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Sitecore.Data.Fields.TinyMCERTEType" /> class.
    /// </summary>
    /// <param name="innerField">The inner field.</param>
    /// <contract>
    ///   <requires name="innerField" condition="none" />
    /// </contract>
        public TinyMCERTEType(Field innerField)
      : base(innerField)
    {
        Assert.ArgumentNotNull((object)innerField, ExtensionMethods.nameof(() => innerField));
    }

    /// <summary>
    /// Converts a <see cref="T:Sitecore.Data.Fields.Field" /> to a <see cref="T:Sitecore.Data.Fields.TinyMCERTEType" />.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <returns>The implicit operator.</returns>
    public static implicit operator TinyMCERTEType(Field field)
    {
      if (field != null)
        return new TinyMCERTEType(field);
      return (TinyMCERTEType) null;
    }

    /// <summary>Gets the plain text.</summary>
    /// <returns></returns>
    public virtual string GetPlainText()
    {
      string input = this.InnerField.GetValue(true);
      if (input == null)
        return (string) null;
      return HttpUtility.HtmlDecode(Regex.Replace(input, "<[^>]*>", string.Empty));
    }

    /// <summary>Gets the web edit buttons.</summary>
    /// <returns>The web edit buttons.</returns>
    /// <contract>
    ///   <ensures condition="nullable" />
    /// </contract>
    public override List<WebEditButton> GetWebEditButtons()
    {
      List<WebEditButton> webEditButtonList = new List<WebEditButton>();
      Item obj = Sitecore.Client.GetDatabaseNotNull("core").GetItem(StringUtil.GetString(new string[2]
      {
        this.InnerField.Source,
        Sitecore.Configuration.Settings.GetSetting("TinyEditor.DefaultProfile")
      }));

      if (obj == null)
        return webEditButtonList;
      Item child1 = obj.Children["WebEdit Buttons"];
      if (child1 == null)
        return webEditButtonList;
      foreach (Item child2 in child1.Children)
      {
        WebEditButton webEditButton = new WebEditButton();
        if (child2.TemplateID == Ribbon.Separator)
        {
          webEditButton.IsDivider = true;
        }
        else
        {
          webEditButton.Header = child2["Header"];
          webEditButton.Icon = child2["Icon"];
          webEditButton.Click = child2["Click"];
          webEditButton.Tooltip = child2["Tooltip"];
        }
        if (UIUtil.SupportsInlineEditing() || webEditButton.Click.Contains("edithtml"))
          webEditButtonList.Add(webEditButton);
      }
      return webEditButtonList;
    }
  }
}
