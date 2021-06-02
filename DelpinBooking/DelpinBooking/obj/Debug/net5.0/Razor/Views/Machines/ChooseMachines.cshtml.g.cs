#pragma checksum "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b99fd414e1cfd55d68aa57b6c6dc06e8a823ead3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Machines_ChooseMachines), @"mvc.1.0.view", @"/Views/Machines/ChooseMachines.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\_ViewImports.cshtml"
using DelpinBooking;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\_ViewImports.cshtml"
using DelpinBooking.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
using DelpinBooking.Controllers;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b99fd414e1cfd55d68aa57b6c6dc06e8a823ead3", @"/Views/Machines/ChooseMachines.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"34db9a369f3039fa783a641cfebc635549f85736", @"/Views/_ViewImports.cshtml")]
    public class Views_Machines_ChooseMachines : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Machine>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "AddToCart", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
  
    ViewData["Title"] = "Vælg maskiner";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Maskiner</h1>\r\n\r\n");
#nullable restore
#line 10 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
  
    int counter = 0;
    int pageValue = ViewBag.QueryParameters.Page;
    int sizeValue = ViewBag.QueryParameters.Size;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 16 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
  
    List<SelectListItem> cities = new List<SelectListItem>();
    foreach (string s in ViewBag.WarehouseCities)
    {
        cities.Add(new SelectListItem() { Text = s, Value = s });
    }


#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 25 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
 using (Html.BeginForm(FormMethod.Get))
{
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
Write(Html.DropDownList("warehousecity", cities, "-- Vælg et varehus --"));

#line default
#line hidden
#nullable disable
            WriteLiteral("    <input type=\"hidden\" name=\"page\" value=\"1\" />\r\n    <input type=\"hidden\" name=\"size\"");
            BeginWriteAttribute("value", " value=\"", 681, "\"", 699, 1);
#nullable restore
#line 29 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
WriteAttributeValue("", 689, sizeValue, 689, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n    <input type=\"submit\" value=\"Søg\" />\r\n");
#nullable restore
#line 31 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
#nullable restore
#line 37 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
           Write(Html.DisplayNameFor(model => model.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 40 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
           Write(Html.DisplayNameFor(model => model.Type));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 43 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
           Write(Html.DisplayNameFor(model => model.Warehouse.City));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 49 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
#nullable restore
#line 53 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
               Write(Html.DisplayFor(modelItem => item.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 56 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
               Write(Html.DisplayFor(modelItem => item.Type));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 59 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
               Write(Html.DisplayFor(modelItem => item.Warehouse.City));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b99fd414e1cfd55d68aa57b6c6dc06e8a823ead38836", async() => {
                WriteLiteral("Add to cart");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 62 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
                                                WriteLiteral(item.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </td>\r\n            </tr>\r\n");
#nullable restore
#line 65 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n\r\n");
#nullable restore
#line 69 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
 if (pageValue > 1)
{
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 71 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
     using (Html.BeginForm(FormMethod.Get))
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <input type=\"hidden\" name=\"page\"");
            BeginWriteAttribute("value", " value=\"", 1845, "\"", 1867, 1);
#nullable restore
#line 73 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
WriteAttributeValue("", 1853, pageValue-1, 1853, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        <input type=\"hidden\" name=\"size\"");
            BeginWriteAttribute("value", " value=\"", 1913, "\"", 1931, 1);
#nullable restore
#line 74 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
WriteAttributeValue("", 1921, sizeValue, 1921, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        <input type=\"submit\" class=\"floated\" value=\"Forige side\" />\r\n");
#nullable restore
#line 76 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 76 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
     
}

#line default
#line hidden
#nullable disable
#nullable restore
#line 78 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
 if (counter == sizeValue)
{
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 80 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
     using (Html.BeginForm(FormMethod.Get))
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <input type=\"hidden\" name=\"page\"");
            BeginWriteAttribute("value", " value=\"", 2139, "\"", 2161, 1);
#nullable restore
#line 82 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
WriteAttributeValue("", 2147, pageValue+1, 2147, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        <input type=\"hidden\" name=\"size\"");
            BeginWriteAttribute("value", " value=\"", 2207, "\"", 2225, 1);
#nullable restore
#line 83 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
WriteAttributeValue("", 2215, sizeValue, 2215, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        <input type=\"submit\" class=\"floated\" value=\"Næste side\" />\r\n");
#nullable restore
#line 85 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 85 "C:\Users\Bess\Documents\delpin\DelpinSemesterProjekt\DelpinBooking\DelpinBooking\Views\Machines\ChooseMachines.cshtml"
     
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Machine>> Html { get; private set; }
    }
}
#pragma warning restore 1591
