using OrgChartDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Helpers
{
    public class DemoTableHelper
    {
        public static Microsoft.AspNetCore.Html.HtmlString DemoTable(Dictionary<string, int[]> demoInfo)
        {

            return new Microsoft.AspNetCore.Html.HtmlString( "<strong>Unit Demographics:</strong><table>" +
                "<tr>" +
                "<th>Race:</th>" +
                "<th> M </th>" +
                "<th> F </th>" +
                "</tr>" +
                "<td>Black: </td>" +
                "<td>" + demoInfo["B"][0] + "</td>" +
                "<td>" + demoInfo["B"][0] + "</td>" +
                "</tr>" +
                "<tr>" +
                "<td>White: </td>" +
                "<td> " + demoInfo["W"][0] + " </td>" +
                "<td> " + demoInfo["W"][1] + " </td>" +
                "</tr>" +
                "<tr>" +
                "<td>Asian: </td>" +
                "<td> " + demoInfo["A"][0] + " </td>" +
                "<td> " + demoInfo["A"][1] + " </td>" +
                "</tr>" +
                "<tr>" +
                "<td>American Indian: </td>" +
                "<td> " + demoInfo["I"][0] + " </td>" +
                "<td> " + demoInfo["I"][1] + " </td>" +
                "</tr>" +
                "<tr>" +
                "<td>Hispanic: </td>" +
                "<td> " + demoInfo["H"][0] + " </td>" +
                "<td> " + demoInfo["H"][1] + " </td>" +
                "</tr>" +
                "</table>");
        }
    }
}
