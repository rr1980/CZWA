using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZWA.Web.TagHelpers
{
    [HtmlTargetElement("th_select", Attributes = "th-label, th-value, th-options, th-ovalue, th-otext")]
    public class TH_Select : TagHelper
    {
        [HtmlAttributeName("th-label")]
        public string Label { get; set; }

        [HtmlAttributeName("th-value")]
        public string Value { get; set; }

        [HtmlAttributeName("th-options")]
        public string Options { get; set; }

        [HtmlAttributeName("th-id")]
        public string Id { get; set; }

        [HtmlAttributeName("th-col")]
        public int Col { get; set; } = 12;

        [HtmlAttributeName("th-ovalue")]
        public string OptionsValue { get; set; }

        [HtmlAttributeName("th-otext")]
        public string OptionsText { get; set; }

        [HtmlAttributeName("th-isFC")]
        public bool IsFormControl { get; set; } = true;

        [HtmlAttributeName("th-withLabel")]
        public bool WithLabel { get; set; } = true;

        private string template = "";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Value = TagHelperTools.FirstLetterToLower(Value);
            OptionsValue = TagHelperTools.FirstLetterToLower(OptionsValue);
            Options = TagHelperTools.FirstLetterToLower(Options);
            OptionsText = TagHelperTools.FirstLetterToLower(OptionsText);

            Id = Id == null ? TagHelperTools.GetID() : Id;

            if (IsFormControl)
            {
                output.TagName = "div";
                output.Attributes.Add("class", $"form-group-sm col-md-{Col}");
                output.TagMode = TagMode.StartTagAndEndTag;
            }

            if (WithLabel)
            {
                template += $"<label class='control-label' for='{Id}'>{Label}</label>";
            }

            template += $"<select class='form-control show-tick input-sm' data-style='btn btn-info btn-sm' data-bind='selectPicker: {Value}, optionsText: \"{OptionsText}\", optionsValue : \"{OptionsValue}\",  selectPickerOptions: {{ optionsArray: {Options} }}'></select>";

            output.Content.SetHtmlContent(template);

        }
    }
}
