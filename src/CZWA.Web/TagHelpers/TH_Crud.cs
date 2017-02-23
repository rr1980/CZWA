using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CZWA.Web.TagHelpers
{
    [HtmlTargetElement("th_crud")]
    public class TH_Crud : TagHelper
    {
        [HtmlAttributeName("th-save")]
        public string Save { get; set; } = "onClickSave";

        [HtmlAttributeName("th-insert")]
        public string Insert { get; set; } = "onClickInsert";

        [HtmlAttributeName("th-edit")]
        public string Edit { get; set; } = "onClickEdit";

        [HtmlAttributeName("th-del")]
        public string Del { get; set; } = "onClickDelete";

        [HtmlAttributeName("th-show-save")]
        public bool IsSave { get; set; } = true;

        [HtmlAttributeName("th-show-insert")]
        public bool IsInsert { get; set; } = true;

        [HtmlAttributeName("th-show-edit")]
        public bool IsEdit { get; set; } = true;

        [HtmlAttributeName("th-show-del")]
        public bool IsDel { get; set; } = true;

        [HtmlAttributeName("th-col")]
        public int Col { get; set; } = 12;

        [HtmlAttributeName("th-isFC")]
        public bool IsFormControl { get; set; } = true;

        [HtmlAttributeName("th-id")]
        public string Id { get; set; }

        private string template = "";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Id = Id == null ? TagHelperTools.GetID() : Id;

            if (IsFormControl)
            {
                output.TagName = "div";
                output.Attributes.Add("class", $"form-group-sm col-md-{Col}");
                output.TagMode = TagMode.StartTagAndEndTag;
            }
            template += "<div class='form-group-sm col-md-12'>";
            template += $"<label class='control-label' for='{Id}'>&nbsp;</label>";
            template += "</div>";
            template += "<div class='form-group-sm pull-right'>";
            template += IsSave ? $"<button style='margin-left:10px;' class='btn btn-warning btn-sm' data-bind='click: {Save}'>Speichern</button>" : "";
            template += IsInsert ? $"<button style='margin-left:10px;' class='btn btn-info btn-sm' data-bind='click: {Insert}'>Einfügen</button>" : "";
            template += IsEdit ? $"<button style='margin-left:10px;' class='btn btn-warning btn-sm' data-bind='click: {Edit}'>Bearbeiten</button>" : "";
            template += IsDel ? $"<button style='margin-left:10px;' class='btn btn-danger btn-sm' data-bind='click: {Del}'>Löschen</button>" : "";
            template += "</div>";

            output.Content.SetHtmlContent(template);

        }
    }
}
