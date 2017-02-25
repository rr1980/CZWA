using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZWA.Web.TagHelpers
{
    [HtmlTargetElement("th_li", Attributes = "th-label, th-input")]
    public class TH_Label_Input : TagHelper
    {
        [HtmlAttributeName("th-label")]
        public string Label { get; set; }

        [HtmlAttributeName("th-input")]
        public string Input { get; set; }

        [HtmlAttributeName("th-id")]
        public string Id { get; set; }

        [HtmlAttributeName("th-col")]
        public int Col { get; set; } = 12;

        [HtmlAttributeName("th-isFC")]
        public bool IsFormControl { get; set; } = true;

        private string template = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Input = TagHelperTools.FirstLetterToLower(Input);
            Id = Id == null ? TagHelperTools.GetID() : Id;

            if (IsFormControl)
            {
                output.TagName = "div";
                output.Attributes.Add("class", $"form-group-sm col-md-{Col}");
                output.TagMode = TagMode.StartTagAndEndTag;
            }

            template += $"<label class='control-label'>{Label}</label>";
            template += $"<input class='form-control' data-bind='value: {Input}'></input>";

            output.Content.SetHtmlContent(template);

        }
    }
}
