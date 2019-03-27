using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using Dgm = DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Xml;
using System.Xml.Linq;

namespace OrgChartDemo.Models.DocGenerators
{
    public class OrgChartGenerator
    {
        public IEnumerable<ChartableComponentWithMember> ChartableComponents { get; set;}

        public OrgChartGenerator()
        {
        }

        public OrgChartGenerator(IEnumerable<ChartableComponentWithMember> _components)
        {
            ChartableComponents = _components;
        }

        public MemoryStream Generate()
        {

            string hierarchicalStructure = ParseComponentToNestedXMLList(ChartableComponents.First()).ToString();
            var mem = new MemoryStream();
            byte[] byteArray = File.ReadAllBytes("Templates/Organization_Chart_Template.docx");
            mem.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(mem, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                HeaderPart headerPart1 = mainPart.AddNewPart<HeaderPart>();
                //FooterPart footerPart1 = mainPart.AddNewPart<FooterPart>();
                string headerPartId = mainPart.GetIdOfPart(headerPart1);
                //string footerPartId = mainPart.GetIdOfPart(footerPart1);
                GenerateHeaderPart1Content(headerPart1);
                //GenerateFooterPartContent(footerPart1);
                // Get SectionProperties and Replace HeaderReference and FooterRefernce with new Id
                IEnumerable<SectionProperties> sections = mainPart.Document.Body.Elements<SectionProperties>();

                foreach (var section in sections)
                {
                    // Delete existing references to headers and footers
                    section.RemoveAllChildren<HeaderReference>();
                    section.RemoveAllChildren<FooterReference>();

                    // Create the new header and footer reference node
                    section.PrependChild(new HeaderReference() { Id = headerPartId });
                    //section.PrependChild(new FooterReference() { Id = footerPartId });
                }
                DiagramDataPart diagram = wordDoc.MainDocumentPart.DiagramDataParts.First();
                XmlDocument xmlDoc = new XmlDocument();//load xml

                xmlDoc.LoadXml(hierarchicalStructure);
                AddNodesToDiagram("", xmlDoc.ChildNodes[0], diagram.DataModelRoot.PointList, diagram.DataModelRoot.ConnectionList, 0U, true);
                diagram.DataModelRoot.Save();
                wordDoc.MainDocumentPart.Document.Save();
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }

        public XElement ParseComponentToNestedXMLList(ChartableComponentWithMember _component, XElement _element = null)
        {
            XElement x = new XElement(
                "node",
                new XAttribute("componentName", _component.ComponentName),
                new XAttribute("positionName", _component.PositionName),
                new XAttribute("memberName", _component.MemberName)
                );
            if(_element != null)
            {
                _element.Add(x);
            }
            List<ChartableComponentWithMember> children = ChartableComponents.Where(c => c.Parentid == _component.Id).ToList();
            foreach (ChartableComponentWithMember child in children)
            {
                ParseComponentToNestedXMLList(child, x);
            }
            return x;
        }



        private static void AddNodesToDiagram(string parentNodeGuid, XmlNode currentNode, Dgm.PointList pointList, Dgm.ConnectionList connectionList, UInt32 connectionSourcePosition, bool clearExisting)
        {
            //recursive function to add nodes to an existing diagram
            if (clearExisting)
            {
                //remove all connections
                connectionList.RemoveAllChildren();
                //remove all nodes except where type = 'doc' ... NOTE: Didn't test what happens if you also remove the doc type node, but it seemed important.
                List<Dgm.Point> pts = pointList.OfType<Dgm.Point>().Where(x => x.Type != "doc").ToList();
                for (int i = pts.Count - 1; i >= 0; i--) { pts[i].Remove(); }//remove in reverse order.
            }

            string currentNodeGuid = "{" + Guid.NewGuid().ToString() + "}";//generate new guid, not sure if curly brackets are required (probably not).

            //create the new point
            Dgm.Point newPoint = new Dgm.Point(new Dgm.PropertySet()) { ModelId = currentNodeGuid };
            Dgm.ShapeProperties newPointShapeProperties = new Dgm.ShapeProperties();

            if (currentNode.Attributes["highlight"] != null && currentNode.Attributes["highlight"].Value == "true")
            {
                //if we need to highlight this particular point, then add a solidfill to it
                newPointShapeProperties.Append(new A.SolidFill(new A.SchemeColor() { Val = A.SchemeColorValues.Accent5 }));
            }

            Dgm.TextBody newPointTextBody = new Dgm.TextBody(new A.BodyProperties(), new A.ListStyle());

            A.Paragraph paragraph1 = new A.Paragraph();
            A.Run ComponentNameRun = new A.Run();
            A.RunProperties ComponentNameRunProperties = new A.RunProperties() { Language = "en-AU" };
            A.Text ComponentNameText = new A.Text(currentNode.Attributes["componentName"].Value);
            ComponentNameRun.Append(ComponentNameRunProperties);
            ComponentNameRun.Append(ComponentNameText);
            paragraph1.Append(ComponentNameRun);

            A.Paragraph paragraph2 = new A.Paragraph();
            A.Run PositionNameRun = new A.Run();
            A.RunProperties PositionNameRunProperties = new A.RunProperties() { Language = "en-AU" };
            A.Text PositionNameText = new A.Text(currentNode.Attributes["memberName"].Value);
            PositionNameRun.Append(PositionNameRunProperties);
            PositionNameRun.Append(PositionNameText);
            paragraph2.Append(PositionNameRun);

            newPointTextBody.Append(paragraph1);
            newPointTextBody.Append(paragraph2);

            newPoint.Append(newPointShapeProperties, newPointTextBody);//append to point

            //append the point to the point list

            pointList.Append(newPoint);

            if (!string.IsNullOrEmpty(parentNodeGuid))
            {
                //if parent specified, then create the connection where the parent is the source and the current node is the destination
                connectionList.Append(new Dgm.Connection() { ModelId = "{" + Guid.NewGuid().ToString() + "}", SourceId = parentNodeGuid, DestinationId = currentNodeGuid, SourcePosition = (UInt32Value)connectionSourcePosition, DestinationPosition = (UInt32Value)0U });
            }

            foreach (XmlNode childNode in currentNode.ChildNodes)
            {
                //call this method for every child
                AddNodesToDiagram(currentNodeGuid, childNode, pointList, connectionList, connectionSourcePosition++, false);
            }

        }

        // Generates content of headerPart1.
        private void GenerateHeaderPart1Content(HeaderPart headerPart1)
        {
            Header header1 = new Header();

            Paragraph paragraph1 = new Paragraph();

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "Header" };
            Justification justification1 = new Justification() { Val = JustificationValues.Left };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            FontSize fontSize1 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "28" };

            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(paragraphStyleId1);
            paragraphProperties1.Append(justification1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            FontSize fontSize2 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "28" };

            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = $"{ChartableComponents.First().ComponentName} Organization Chart";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            header1.Append(paragraph1);
            

            headerPart1.Header = header1;
        }
    }
}
