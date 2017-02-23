using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZWA.Web.TagHelpers
{
    [HtmlTargetElement("th_enum", Attributes = "th-label, th-value, th-type")]
    public class TH_Enum : TagHelper
    {
        [HtmlAttributeName("th-label")]
        public string Label { get; set; }

        [HtmlAttributeName("th-value")]
        public string Value { get; set; }

        [HtmlAttributeName("th-type")]
        public Type EnumType { get; set; }

        [HtmlAttributeName("th-id")]
        public string Id { get; set; }

        [HtmlAttributeName("th-col")]
        public int Col { get; set; } = 12;

        [HtmlAttributeName("th-isFC")]
        public bool IsFormControl { get; set; } = true;

        [HtmlAttributeName("th-withLabel")]
        public bool WithLabel { get; set; } = true;

        private string template = "";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Value = TagHelperTools.FirstLetterToLower(Value);
            Id = Id == null ? TagHelperTools.GetID() : Id;

            Array OptionsArray = System.Enum.GetValues(EnumType);


            var enumVals = new List<object>();

            foreach (var item in Enum.GetValues(EnumType))
            {

                enumVals.Add(new
                {
                    id = (int)item,
                    name = item.ToString()
                });
            }

            string Options = JsonConvert.SerializeObject(enumVals);

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

            template += $"<select class='selectpicker form-control show-tick input-sm' data-style='btn btn-info btn-sm' multiple='true' data-bind='selectedOptions: {Value}, optionsText: \"name\", optionsValue : \"id\", options: {Options}'></select>";
            //template += $"<select class='form-control show-tick input-sm' data-style='btn btn-info btn-sm' multiple='true' data-bind='selectPicker: {Value}, optionsText: \"name\", optionsValue : \"id\",selectPickerOptions: {{ optionsArray: {Options}}}'></select>";

            output.Content.SetHtmlContent(template);

        }
    }
}
