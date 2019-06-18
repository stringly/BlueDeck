using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using BlueDeck.Models.Types;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models.DocGenerators
{
    public class ComponentRosterGenerator
    {
        public List<Component> Components  {get;set;}
    
        public ComponentRosterGenerator()
        {
        }
        public ComponentRosterGenerator(List<Component> _components)
        {
            Components = _components;
        }
        
        public MemoryStream Generate()
        {
            var mem = new MemoryStream();
            byte[] byteArray = File.ReadAllBytes("Templates/Component_Roster_Template.docx");
            mem.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(mem, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                Table headerTable = mainPart.HeaderParts.ElementAt(2).RootElement.Elements<Table>().ElementAt(0);
                RunProperties runProperties1 = new RunProperties();
                Bold bold1 = new Bold();
                runProperties1.Append(bold1);
                FontSize fontSize1 = new FontSize(){ Val = "36" };
                runProperties1.Append(fontSize1);
                Run run1 = new Run();
                run1.Append(runProperties1);
                Text text1 = new Text("Personnel Roster");
                run1.Append(text1);
                
                headerTable.Elements<TableRow>().ElementAt(0)
                    .Elements<TableCell>().ElementAt(1)
                        .Elements<Paragraph>().ElementAt(0)
                            .Append(run1);
                headerTable.Elements<TableRow>().ElementAt(1)
                    .Elements<TableCell>().ElementAt(1)
                        .Elements<Paragraph>().ElementAt(0)
                            .Append(new Run(new Text($"{Components.First().Name}")));
                headerTable.Elements<TableRow>().ElementAt(1)
                    .Elements<TableCell>().ElementAt(2)
                        .Elements<Paragraph>().ElementAt(0)
                            .Append(new Run(new Text(DateTime.Now.ToString("MMMM dd, yyyy"))));
                mainPart.Document.Body.Elements<Paragraph>().ElementAt(0).Remove();
                foreach (Component c in Components)
                {
                    mainPart.Document.Body.Append(GenerateComponentTable(c));
                    mainPart.Document.Body.Append(new Paragraph());
                }
                mainPart.Document.Save();
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;

        }

        private Table GenerateComponentTable(Component c)
        {
            Table ComponentTable = new Table();
            ComponentTable.Append(GenerateComponentTableProperties());
            ComponentTable.Append(GenerateComponentTableGrid());
            ComponentTable.Append(GenerateComponentTableHeaderRow(c.Name, c.ComponentId));
            ComponentTable.Append(GenerateComponentTablePositionRow(c.Name, c.Positions?.ToList() ?? new List<Position>()));
            
            List<Component> childComponents = Components.Where(x => x.ParentComponent.ComponentId == c.ComponentId).OrderBy(x => x.LineupPosition).ToList();
            if (childComponents.Count > 0)
            {
                ComponentTable.Append(GenerateComponentTableChildComponentRow(c.Name, childComponents));
            }
            ComponentTable.Append(GenerateComponentTableDemographicRow(c));
            return ComponentTable;
        }

        // Methods that Generate the parts of the Component Table


        private TableProperties GenerateComponentTableProperties()
        {
            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "TableGrid" };
            TableWidth tableWidth1 = new TableWidth() { Width = "14400", Type = TableWidthUnitValues.Dxa };

            TableBorders tableBorders1 = new TableBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder1 = new InsideHorizontalBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder1 = new InsideVerticalBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };

            tableBorders1.Append(topBorder1);
            tableBorders1.Append(leftBorder1);
            tableBorders1.Append(bottomBorder1);
            tableBorders1.Append(rightBorder1);
            tableBorders1.Append(insideHorizontalBorder1);
            tableBorders1.Append(insideVerticalBorder1);
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableBorders1);
            tableProperties1.Append(tableLook1);
            return tableProperties1;
        }
        private TableGrid GenerateComponentTableGrid()
        {
            TableGrid tableGrid1 = new TableGrid();
            //GridColumn gridColumn1 = new GridColumn() { Width = "450" };
            GridColumn gridColumn2 = new GridColumn() { Width = "14400" };

            //tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            return tableGrid1;
        }
        /// <summary>
        /// Generates the Header Row for a Component Table.
        /// </summary>
        /// <param name="_componentName">Name of the component. This is required to populate the Table title and the link Bookmark</param>
        /// <param name="_componentId">The component id. This is required for the Table's BookMarkStart/End.</param>
        /// <returns></returns>
        private TableRow GenerateComponentTableHeaderRow(string _componentName, int _componentId)
        {
            TableRow tableRow1 = new TableRow();

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)378U };

            tableRowProperties1.Append(tableRowHeight1);

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "14400", Type = TableWidthUnitValues.Dxa };
            //GridSpan gridSpan1 = new GridSpan() { Val = 2 };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Single, Color = "767171", ThemeColor = ThemeColorValues.Background2, ThemeShade = "80", Size = (UInt32Value)18U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(topBorder1);
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            //tableCellProperties1.Append(gridSpan1);
            tableCellProperties1.Append(tableCellBorders1);
            tableCellProperties1.Append(shading1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph();
            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            KeepNext keepNext1 = new KeepNext();
            paragraphProperties1.Append(keepNext1);

            BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = $"{_componentName}", Id = $"{_componentId}" };
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = $"{_componentId}" };

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            Color color1 = new Color() { Val = "000000" };
            FontSize fontSize1 = new FontSize() { Val = "28" };

            runProperties1.Append(color1);
            runProperties1.Append(fontSize1);
            Text text1 = new Text();
            text1.Text = $"{_componentName}";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(bookmarkStart1);
            paragraph1.Append(bookmarkEnd1);
            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell1);
            return tableRow1;
        }
        private TableRow GenerateComponentTablePositionRow(string _componentName, List<Position> _positions)
        {
            TableRow tableRow1 = new TableRow();

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)1080U };

            tableRowProperties1.Append(tableRowHeight1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "13950", Type = TableWidthUnitValues.Dxa };
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(shading1);

            Paragraph paragraph2 = new Paragraph();

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            KeepNext keepNext2 = new KeepNext();
            paragraphProperties2.Append(keepNext2);
            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            Bold bold1 = new Bold();
            Color color1 = new Color() { Val = "009999" };

            paragraphMarkRunProperties1.Append(bold1);
            paragraphMarkRunProperties1.Append(color1);

            paragraphProperties2.Append(paragraphMarkRunProperties1);

            paragraph2.Append(paragraphProperties2);
            // Begin Generating the Positions Sub Table

            // Start by Generating Sub Table with the header row
            Table table1 = GeneratePositionSubTableWithHeaderRow(_componentName);
            // Then add a row for each Position in the _positions Parameter
            foreach(Position p in _positions)
            {
                foreach(Member m in p.Members)
                {
                    table1.Append(GeneratePositionSubTablePositionRow(m));
                }
                
            }

            Paragraph paragraph3 = new Paragraph();
            ParagraphProperties paragraphProperties3 = new ParagraphProperties();            
            KeepNext keepNext3 = new KeepNext();
            paragraphProperties3.Append(keepNext3);
            paragraph3.Append(paragraphProperties3);
            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);
            // Position sub table is appended here
            tableCell2.Append(table1);
            tableCell2.Append(paragraph3);

            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell2);
            return tableRow1;
        }
        private TableRow GenerateComponentTableChildComponentRow(string _componentName, List<Component> _components)
        {
            // Create new Table Row
            TableRow tableRow1 = new TableRow();

            // Create Row Properties
            TableRowProperties tableRowProperties1 = new TableRowProperties();
            CantSplit cantSplit1 = new CantSplit();
            tableRowProperties1.Append(cantSplit1);
            TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)1233U };

            tableRowProperties1.Append(tableRowHeight1);
            // Append properties to table row
            tableRow1.Append(tableRowProperties1);

            TableCell tableCell2 = new TableCell();
            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "13950", Type = TableWidthUnitValues.Dxa };

            tableCellProperties2.Append(tableCellWidth2);
            tableCell2.Append(tableCellProperties2);

            // spacer paragraph
            Paragraph paragraph2 = new Paragraph();
            tableCell2.Append(paragraph2);

            //// generate Sub Table here
            Table table1 = GenerateChildComponentSubTableWithHeaderRow(_componentName);
            foreach (Component c in _components)
            {
                table1.Append(GenerateChildComponentSubTableComponentRow(c));
            }
            tableCell2.Append(table1);

            // final spacer paragraph

            Paragraph paragraph3 = new Paragraph();
            tableCell2.Append(paragraph3);

            // append second column
            tableRow1.Append(tableCell2);
            return tableRow1;

        }
        private TableRow GenerateComponentTableDemographicRow(Component _component)
        {
            // Create new Table Row
            TableRow tableRow1 = new TableRow();

            // Create Row Properties
            TableRowProperties tableRowProperties1 = new TableRowProperties();
            TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)1233U };
            CantSplit cantSplit1 = new CantSplit();
            tableRowProperties1.Append(cantSplit1);
            tableRowProperties1.Append(tableRowHeight1);
            // Append properties to table row
            tableRow1.Append(tableRowProperties1);   
                        
            TableCell tableCell2 = new TableCell();
            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "13950", Type = TableWidthUnitValues.Dxa };

            tableCellProperties2.Append(tableCellWidth2);
            tableCell2.Append(tableCellProperties2);

            // spacer paragraph
            Paragraph paragraph2 = new Paragraph();
            tableCell2.Append(paragraph2);

            // generate Sub Table here
            Table table1 = GenerateComponentDemoTable(_component.Name);

            // APPEND DEMO ROWS HERE
            List<Member> allMembers = RecurseForMembers(_component, new List<Member>());
            List<Rank> distinctRankList = allMembers.Select(x => x.Rank).OrderByDescending(x => x.RankId).Distinct().ToList();

            foreach (Rank r in distinctRankList)
            {
                List<Member> RankMembers = allMembers.Where(x => x.Rank == r).ToList();
                table1.Append(GenerateDemoTableRow(r.RankFullName, RankMembers));
            }
            table1.Append(GenerateDemoTableRow("Total", allMembers));
            tableCell2.Append(table1);

            // final spacer paragraph

            Paragraph paragraph3 = new Paragraph();
            tableCell2.Append(paragraph3);
            Paragraph paragraph4 = new Paragraph();
            tableCell2.Append(paragraph4);

            // append second column
            tableRow1.Append(tableCell2);
            return tableRow1;
        }

        // Component Table Sub Table Methods

        // Position Sub-Table
        private Table GeneratePositionSubTableWithHeaderRow(string _componentName)
        {
            // Make new table
            Table PositionSubTable = new Table();
            // Make new table Properties
            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "TableGrid" };
            TableWidth tableWidth1 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLook1);
            // Append Table Properties to Table
            PositionSubTable.Append(tableProperties1);

            // Make new TableGrid
            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "4401" };
            GridColumn gridColumn2 = new GridColumn() { Width = "4401" };
            GridColumn gridColumn3 = new GridColumn() { Width = "1233" };
            GridColumn gridColumn4 = new GridColumn() { Width = "1233" };
            GridColumn gridColumn5 = new GridColumn() { Width = "1233" };
            GridColumn gridColumn6 = new GridColumn() { Width = "1233" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);
            tableGrid1.Append(gridColumn4);
            tableGrid1.Append(gridColumn5);
            tableGrid1.Append(gridColumn6);
            // Append TableGrid to Table
            PositionSubTable.Append(tableGrid1);

            // Create Header Row
            TableRow tableRow1 = new TableRow() { RsidTableRowAddition = "00971797", RsidTableRowProperties = "005D464F" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "4401", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders1.Append(topBorder1);
            tableCellBorders1.Append(leftBorder1);
            tableCellBorders1.Append(bottomBorder1);
            tableCellBorders1.Append(rightBorder1);
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(tableCellBorders1);
            tableCellProperties1.Append(shading1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph();

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            Color color1 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties1.Append(color1);

            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "005D464F" };

            RunProperties runProperties1 = new RunProperties();
            Color color2 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };
            Bold bold1 = new Bold();
            runProperties1.Append(bold1);
            runProperties1.Append(color2);
            Text text1 = new Text();
            text1.Text = $"{_componentName} Positions";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "4401", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            TopBorder topBorder2 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder2 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders2.Append(topBorder2);
            tableCellBorders2.Append(leftBorder2);
            tableCellBorders2.Append(bottomBorder2);
            tableCellBorders2.Append(rightBorder2);
            Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellBorders2);
            tableCellProperties2.Append(shading2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph2 = new Paragraph();

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            Color color3 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties2.Append(color3);

            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run() { RsidRunProperties = "005D464F" };

            RunProperties runProperties2 = new RunProperties();
            Color color4 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };
            Bold bold2 = new Bold();
            runProperties2.Append(bold2);
            runProperties2.Append(color4);
            Text text2 = new Text();
            text2.Text = "Name";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            TopBorder topBorder3 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder3 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder3 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder3 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders3.Append(topBorder3);
            tableCellBorders3.Append(leftBorder3);
            tableCellBorders3.Append(bottomBorder3);
            tableCellBorders3.Append(rightBorder3);
            Shading shading3 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellBorders3);
            tableCellProperties3.Append(shading3);
            tableCellProperties3.Append(tableCellVerticalAlignment3);

            Paragraph paragraph3 = new Paragraph();

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            Color color5 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties3.Append(color5);

            paragraphProperties3.Append(justification1);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run() { RsidRunProperties = "005D464F" };

            RunProperties runProperties3 = new RunProperties();
            Color color6 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };
            Bold bold3 = new Bold();
            runProperties3.Append(bold3);
            runProperties3.Append(color6);
            Text text3 = new Text();
            text3.Text = "ID";

            run3.Append(runProperties3);
            run3.Append(text3);

            Run run4 = new Run() { RsidRunProperties = "005D464F" };

            RunProperties runProperties4 = new RunProperties();
            Color color7 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            runProperties4.Append(color7);
            Text text4 = new Text();
            text4.Text = "#";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);
            paragraph3.Append(run4);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders4 = new TableCellBorders();
            TopBorder topBorder4 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder4 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder4 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder4 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders4.Append(topBorder4);
            tableCellBorders4.Append(leftBorder4);
            tableCellBorders4.Append(bottomBorder4);
            tableCellBorders4.Append(rightBorder4);
            Shading shading4 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellBorders4);
            tableCellProperties4.Append(shading4);
            tableCellProperties4.Append(tableCellVerticalAlignment4);

            Paragraph paragraph4 = new Paragraph();

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            Color color8 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties4.Append(color8);

            paragraphProperties4.Append(justification2);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run5 = new Run() { RsidRunProperties = "005D464F" };

            RunProperties runProperties5 = new RunProperties();
            Color color9 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };
            Bold bold5 = new Bold();
            runProperties5.Append(bold5);
            runProperties5.Append(color9);
            Text text5 = new Text();
            text5.Text = "Call Sign";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run5);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders5 = new TableCellBorders();
            TopBorder topBorder5 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder5 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder5 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder5 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders5.Append(topBorder5);
            tableCellBorders5.Append(leftBorder5);
            tableCellBorders5.Append(bottomBorder5);
            tableCellBorders5.Append(rightBorder5);
            Shading shading5 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders5);
            tableCellProperties5.Append(shading5);
            tableCellProperties5.Append(tableCellVerticalAlignment5);

            Paragraph paragraph5 = new Paragraph();

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            Color color10 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties5.Append(color10);

            paragraphProperties5.Append(justification3);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run6 = new Run() { RsidRunProperties = "005D464F" };

            RunProperties runProperties6 = new RunProperties();
            Color color11 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };
            Bold bold6 = new Bold();
            runProperties6.Append(bold6);
            runProperties6.Append(color11);
            Text text6 = new Text();
            text6.Text = "Race";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run6);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders6 = new TableCellBorders();
            TopBorder topBorder6 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder6 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder6 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder6 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders6.Append(topBorder6);
            tableCellBorders6.Append(leftBorder6);
            tableCellBorders6.Append(bottomBorder6);
            tableCellBorders6.Append(rightBorder6);
            Shading shading6 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment6 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellBorders6);
            tableCellProperties6.Append(shading6);
            tableCellProperties6.Append(tableCellVerticalAlignment6);

            Paragraph paragraph6 = new Paragraph();

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            Color color12 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties6.Append(color12);

            paragraphProperties6.Append(justification4);
            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run7 = new Run() { RsidRunProperties = "005D464F" };

            RunProperties runProperties7 = new RunProperties();
            Color color13 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            runProperties7.Append(color13);
            Bold bold7 = new Bold();
            runProperties7.Append(bold7);
            Text text7 = new Text();
            text7.Text = "Gender";

            run7.Append(runProperties7);
            run7.Append(text7);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run7);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            tableRow1.Append(tableCell6);

            // Append Header Row
            PositionSubTable.Append(tableRow1);
            return PositionSubTable;

        }
        private TableRow GeneratePositionSubTablePositionRow(Member _member)
        {
            TableRow tableRow1 = new TableRow() { RsidTableRowAddition = "00971797", RsidTableRowProperties = "005D464F" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "4401", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Nil };
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders1.Append(topBorder1);
            tableCellBorders1.Append(leftBorder1);
            tableCellBorders1.Append(bottomBorder1);
            tableCellBorders1.Append(rightBorder1);
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(tableCellBorders1);
            tableCellProperties1.Append(shading1);

            Paragraph paragraph1 = new Paragraph();

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();

            Text text1 = new Text();
            text1.Text = $"{_member.Position.Name}";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "4401", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            TopBorder topBorder2 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder2 = new BottomBorder() { Val = BorderValues.Nil };
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders2.Append(topBorder2);
            tableCellBorders2.Append(leftBorder2);
            tableCellBorders2.Append(bottomBorder2);
            tableCellBorders2.Append(rightBorder2);
            Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellBorders2);
            tableCellProperties2.Append(shading2);
            Paragraph paragraph2 = new Paragraph();
            Run run2 = new Run();

            RunProperties runProperties2 = new RunProperties();

            Text text2 = new Text();
            text2.Text = $"{_member.GetTitleName()}";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            TopBorder topBorder3 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder3 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder3 = new BottomBorder() { Val = BorderValues.Nil };
            RightBorder rightBorder3 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders3.Append(topBorder3);
            tableCellBorders3.Append(leftBorder3);
            tableCellBorders3.Append(bottomBorder3);
            tableCellBorders3.Append(rightBorder3);
            Shading shading3 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellBorders3);
            tableCellProperties3.Append(shading3);
            tableCellProperties3.Append(tableCellVerticalAlignment1);

            Paragraph paragraph3 = new Paragraph();
            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();

            Text text3 = new Text();
            text3.Text = $"#{_member.IdNumber}";

            run3.Append(runProperties3);
            run3.Append(text3);

            
            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties1.Append(justification1);

            paragraph3.Append(paragraphProperties1);
            paragraph3.Append(run3);
            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders4 = new TableCellBorders();
            TopBorder topBorder4 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder4 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder4 = new BottomBorder() { Val = BorderValues.Nil };
            RightBorder rightBorder4 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders4.Append(topBorder4);
            tableCellBorders4.Append(leftBorder4);
            tableCellBorders4.Append(bottomBorder4);
            tableCellBorders4.Append(rightBorder4);
            Shading shading4 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellBorders4);
            tableCellProperties4.Append(shading4);
            tableCellProperties4.Append(tableCellVerticalAlignment2);

            Paragraph paragraph4 = new Paragraph();
            Run run4 = new Run();

            RunProperties runProperties4 = new RunProperties();

            Text text4 = new Text();
            string callSign = _member.Position.Callsign != null ? _member.Position.Callsign : "-";
            text4.Text = $"{callSign}";

            run4.Append(runProperties4);
            run4.Append(text4);

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties2.Append(justification2);

            paragraph4.Append(paragraphProperties2);
            paragraph4.Append(run4);
            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders5 = new TableCellBorders();
            TopBorder topBorder5 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder5 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder5 = new BottomBorder() { Val = BorderValues.Nil };
            RightBorder rightBorder5 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders5.Append(topBorder5);
            tableCellBorders5.Append(leftBorder5);
            tableCellBorders5.Append(bottomBorder5);
            tableCellBorders5.Append(rightBorder5);
            Shading shading5 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders5);
            tableCellProperties5.Append(shading5);
            tableCellProperties5.Append(tableCellVerticalAlignment3);

            Paragraph paragraph5 = new Paragraph();
            Run run5 = new Run();

            RunProperties runProperties5 = new RunProperties();

            Text text5 = new Text();            
            text5.Text = $"{_member.Race.Abbreviation}";

            run5.Append(runProperties5);
            run5.Append(text5);
            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties3.Append(justification3);

            paragraph5.Append(paragraphProperties3);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders6 = new TableCellBorders();
            TopBorder topBorder6 = new TopBorder() { Val = BorderValues.Nil };
            LeftBorder leftBorder6 = new LeftBorder() { Val = BorderValues.Nil };
            BottomBorder bottomBorder6 = new BottomBorder() { Val = BorderValues.Nil };
            RightBorder rightBorder6 = new RightBorder() { Val = BorderValues.Nil };

            tableCellBorders6.Append(topBorder6);
            tableCellBorders6.Append(leftBorder6);
            tableCellBorders6.Append(bottomBorder6);
            tableCellBorders6.Append(rightBorder6);
            Shading shading6 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellBorders6);
            tableCellProperties6.Append(shading6);
            tableCellProperties6.Append(tableCellVerticalAlignment4);

            Paragraph paragraph6 = new Paragraph();
            Run run6 = new Run();

            RunProperties runProperties6 = new RunProperties();

            Text text6 = new Text();
            text6.Text = $"{_member.Gender.Abbreviation}";

            run6.Append(runProperties6);
            run6.Append(text6);
            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties4.Append(justification4);

            paragraph6.Append(paragraphProperties4);
            paragraph6.Append(run6);
            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            tableRow1.Append(tableCell6);
            return tableRow1;
        }

        // Child Component Sub-Table
        private Table GenerateChildComponentSubTableWithHeaderRow(string _componentName)
        {
            // Create the New Table
            Table ChildComponentSubTable = new Table();
            // Create table properties
            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "TableGrid" };
            TableWidth tableWidth1 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };

            TableBorders tableBorders1 = new TableBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder1 = new InsideHorizontalBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder1 = new InsideVerticalBorder() { Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };

            tableBorders1.Append(topBorder1);
            tableBorders1.Append(leftBorder1);
            tableBorders1.Append(bottomBorder1);
            tableBorders1.Append(rightBorder1);
            tableBorders1.Append(insideHorizontalBorder1);
            tableBorders1.Append(insideVerticalBorder1);
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableBorders1);
            tableProperties1.Append(tableLook1);
            // Append Table Properties
            ChildComponentSubTable.Append(tableProperties1);
            // Create Table Grid
            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "4392" };
            GridColumn gridColumn2 = new GridColumn() { Width = "4410" };
            GridColumn gridColumn3 = new GridColumn() { Width = "1233" };
            GridColumn gridColumn4 = new GridColumn() { Width = "1233" };
            GridColumn gridColumn5 = new GridColumn() { Width = "1233" };
            GridColumn gridColumn6 = new GridColumn() { Width = "1233" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);
            tableGrid1.Append(gridColumn4);
            tableGrid1.Append(gridColumn5);
            tableGrid1.Append(gridColumn6);
            // Append Table Grid
            ChildComponentSubTable.Append(tableGrid1);

            // Create Header Row
            TableRow tableRow1 = new TableRow();

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "4392", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            BottomBorder bottomBorder2 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(bottomBorder2);
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(tableCellBorders1);
            tableCellProperties1.Append(shading1);

            Paragraph paragraph1 = new Paragraph();

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            Color color1 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties1.Append(color1);

            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "005D464F" };

            RunProperties runProperties1 = new RunProperties();
            Color color2 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            runProperties1.Append(color2);
            Bold bold1 = new Bold();
            runProperties1.Append(bold1);
            Text text1 = new Text();
            text1.Text = $"{_componentName} Subordinate Units";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "4410", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            BottomBorder bottomBorder3 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders2.Append(bottomBorder3);
            Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellBorders2);
            tableCellProperties2.Append(shading2);

            Paragraph paragraph2 = new Paragraph();

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            Color color3 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties2.Append(color3);

            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run();

            RunProperties runProperties2 = new RunProperties();
            Color color4 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };
            Bold bold2 = new Bold();
            runProperties2.Append(bold2);
            runProperties2.Append(color4);
            Text text2 = new Text();
            text2.Text = "Supervisor";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            BottomBorder bottomBorder4 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders3.Append(bottomBorder4);
            Shading shading3 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellBorders3);
            tableCellProperties3.Append(shading3);
            tableCellProperties3.Append(tableCellVerticalAlignment1);

            Paragraph paragraph3 = new Paragraph();

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            Color color5 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties3.Append(color5);

            paragraphProperties3.Append(justification1);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run();

            RunProperties runProperties3 = new RunProperties();
            Color color6 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };
            Bold bold3 = new Bold();
            runProperties3.Append(bold3);
            runProperties3.Append(color6);
            Text text3 = new Text();
            text3.Text = "ID#";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders4 = new TableCellBorders();
            BottomBorder bottomBorder5 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders4.Append(bottomBorder5);
            Shading shading4 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellBorders4);
            tableCellProperties4.Append(shading4);
            tableCellProperties4.Append(tableCellVerticalAlignment2);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "005D464F", RsidParagraphAddition = "00971797", RsidParagraphProperties = "00971797", RsidRunAdditionDefault = "00971797" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            Color color7 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties4.Append(color7);

            paragraphProperties4.Append(justification2);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run4 = new Run();

            RunProperties runProperties4 = new RunProperties();
            Color color8 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };
            Bold bold4 = new Bold();
            runProperties4.Append(bold4);
            runProperties4.Append(color8);
            Text text4 = new Text();
            text4.Text = "Call Sign";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders5 = new TableCellBorders();
            BottomBorder bottomBorder6 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders5.Append(bottomBorder6);
            Shading shading5 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellBorders5);
            tableCellProperties5.Append(shading5);
            tableCellProperties5.Append(tableCellVerticalAlignment3);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "005D464F", RsidParagraphAddition = "00971797", RsidParagraphProperties = "00971797", RsidRunAdditionDefault = "00971797" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            Color color9 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties5.Append(color9);

            paragraphProperties5.Append(justification3);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run5 = new Run() { RsidRunProperties = "005D464F" };
            
            
            RunProperties runProperties5 = new RunProperties();
            Color color10 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };
            Bold bold5 = new Bold();
            runProperties5.Append(bold5);
            runProperties5.Append(color10);
            Text text5 = new Text();
            text5.Text = "Race";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders6 = new TableCellBorders();
            BottomBorder bottomBorder7 = new BottomBorder() { Val = BorderValues.Single, Color = "AEAAAA", ThemeColor = ThemeColorValues.Background2, ThemeShade = "BF", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders6.Append(bottomBorder7);
            Shading shading6 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF", ThemeFill = ThemeColorValues.Background1 };
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellBorders6);
            tableCellProperties6.Append(shading6);
            tableCellProperties6.Append(tableCellVerticalAlignment4);

            Paragraph paragraph6 = new Paragraph();

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties6 = new ParagraphMarkRunProperties();
            Color color11 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };

            paragraphMarkRunProperties6.Append(color11);

            paragraphProperties6.Append(justification4);
            paragraphProperties6.Append(paragraphMarkRunProperties6);

            Run run6 = new Run() { RsidRunProperties = "005D464F" };

            RunProperties runProperties6 = new RunProperties();
            Color color12 = new Color() { Val = "000000", ThemeColor = ThemeColorValues.Text1 };
            Bold bold6 = new Bold();
            runProperties6.Append(bold6);
            runProperties6.Append(color12);
            Text text6 = new Text();
            text6.Text = "Gender";

            run6.Append(runProperties6);
            run6.Append(text6);

            paragraph6.Append(paragraphProperties6);
            paragraph6.Append(run6);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            tableRow1.Append(tableCell6);
            ChildComponentSubTable.Append(tableRow1);
            return ChildComponentSubTable;
        }
        private TableRow GenerateChildComponentSubTableComponentRow(Component _component)
        {
            Member Manager = _component.GetManager();
            TableRow tableRow1 = new TableRow();

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "4392", Type = TableWidthUnitValues.Dxa };
             

            tableCellProperties1.Append(tableCellWidth1);

            Paragraph paragraph1 = new Paragraph();

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();

            paragraphProperties1.Append(paragraphMarkRunProperties1);
            string bookMarkAnchor = _component.Name.Replace(" ", "_");
            Hyperlink hyperlink1 = new Hyperlink() { Tooltip = $"Go To {_component.Name}", History = true, Anchor = bookMarkAnchor };

            Run run1 = new Run() { RsidRunProperties = "005D464F" };

            RunProperties runProperties1 = new RunProperties();
            RunStyle runStyle1 = new RunStyle() { Val = "Hyperlink" };


            runProperties1.Append(runStyle1);

            Text text1 = new Text();
            text1.Text = $"{_component.Name}";

            run1.Append(runProperties1);
            run1.Append(text1);

            hyperlink1.Append(run1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(hyperlink1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "4410", Type = TableWidthUnitValues.Dxa };

            tableCellProperties2.Append(tableCellWidth2);
            Paragraph paragraph2 = new Paragraph();

            Run run2 = new Run();
            Text text2 = new Text();
            string managerDisplayName = _component?.GetManager()?.GetTitleName() ?? "VACANT";
            text2.Text = $"{managerDisplayName}";
            run2.Append(text2);
            paragraph2.Append(run2);
            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };


            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(tableCellVerticalAlignment1);

            Paragraph paragraph3 = new Paragraph();

            Run run3 = new Run();
            Text text3 = new Text();
            string managerID = Manager?.IdNumber != null ? $"#{Manager.IdNumber}" : "-"; 
            text3.Text = $"{managerID}";
            run3.Append(text3);
            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties2.Append(justification1);

            paragraph3.Append(paragraphProperties2);
            paragraph3.Append(run3);
            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };


            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(tableCellVerticalAlignment2);

            Paragraph paragraph4 = new Paragraph();
            Run run4 = new Run();
            Text text4 = new Text();
            string managerCallsign = _component.Positions?
                .Where(x => x.IsManager == true)
                .FirstOrDefault().Callsign != null ? $"{_component.Positions?.Where(x => x.IsManager == true).FirstOrDefault().Callsign}" : "-";            
            text4.Text = $"{managerCallsign}";
            run4.Append(text4);
            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties3.Append(justification2);

            paragraph4.Append(paragraphProperties3);
            paragraph4.Append(run4);
            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };


            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(tableCellVerticalAlignment3);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphAddition = "00971797", RsidParagraphProperties = "00971797", RsidRunAdditionDefault = "00971797" };
            Run run5 = new Run();
            Text text5 = new Text();            
            string managerRace = Manager?.Race.Abbreviation.ToString() ?? "-";
            text5.Text = managerRace;
            run5.Append(text5);
            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties4.Append(justification3);

            paragraph5.Append(paragraphProperties4);
            paragraph5.Append(run5);
            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "1233", Type = TableWidthUnitValues.Dxa };


            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(tableCellVerticalAlignment4);

            Paragraph paragraph6 = new Paragraph();
            Run run6 = new Run();
            Text text6 = new Text();
            string managerGender = "-";
            if (Manager != null){
                managerGender = Manager.Gender.Abbreviation.ToString();
            }
            text6.Text = managerGender;
            run6.Append(text6);
            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties5.Append(justification4);

            paragraph6.Append(paragraphProperties5);
            paragraph6.Append(run6);
            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            tableRow1.Append(tableCell6);
            return tableRow1;

        }
        // Demographic Sub-Table
        private Table GenerateComponentDemoTable(string _componentName)
        {
            // Create new Table
            Table DemoTable = new Table();
            // Create Table Properties
            TableProperties tableProperties1 = new TableProperties();
            TableWidth tableWidth1 = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

            TableBorders tableBorders1 = new TableBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder1 = new InsideHorizontalBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder1 = new InsideVerticalBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)4U, Space = (UInt32Value)0U };

            tableBorders1.Append(topBorder1);
            tableBorders1.Append(leftBorder1);
            tableBorders1.Append(bottomBorder1);
            tableBorders1.Append(rightBorder1);
            tableBorders1.Append(insideHorizontalBorder1);
            tableBorders1.Append(insideVerticalBorder1);
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableBorders1);
            tableProperties1.Append(tableLook1);
            // Append Table properties
            DemoTable.Append(tableProperties1);

            // Create Table Grid
            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "2101" };
            GridColumn gridColumn2 = new GridColumn() { Width = "396" };
            GridColumn gridColumn3 = new GridColumn() { Width = "396" };
            GridColumn gridColumn4 = new GridColumn() { Width = "344" };
            GridColumn gridColumn5 = new GridColumn() { Width = "396" };
            GridColumn gridColumn6 = new GridColumn() { Width = "336" };
            GridColumn gridColumn7 = new GridColumn() { Width = "344" };
            GridColumn gridColumn8 = new GridColumn() { Width = "344" };
            GridColumn gridColumn9 = new GridColumn() { Width = "403" };
            GridColumn gridColumn10 = new GridColumn() { Width = "396" };
            GridColumn gridColumn11 = new GridColumn() { Width = "396" };
            GridColumn gridColumn12 = new GridColumn() { Width = "344" };
            GridColumn gridColumn13 = new GridColumn() { Width = "396" };
            GridColumn gridColumn14 = new GridColumn() { Width = "500" };
            //GridColumn gridColumn15 = new GridColumn() { Width = "2809" };
            //GridColumn gridColumn16 = new GridColumn() { Width = "889" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);
            tableGrid1.Append(gridColumn4);
            tableGrid1.Append(gridColumn5);
            tableGrid1.Append(gridColumn6);
            tableGrid1.Append(gridColumn7);
            tableGrid1.Append(gridColumn8);
            tableGrid1.Append(gridColumn9);
            tableGrid1.Append(gridColumn10);
            tableGrid1.Append(gridColumn11);
            tableGrid1.Append(gridColumn12);
            tableGrid1.Append(gridColumn13);
            tableGrid1.Append(gridColumn14);
            //tableGrid1.Append(gridColumn15);
            //tableGrid1.Append(gridColumn16);
            DemoTable.Append(tableGrid1);

            // Create Header Row
            TableRow tableRow1 = new TableRow() { RsidTableRowMarkRevision = "00B716D7", RsidTableRowAddition = "00B716D7", RsidTableRowProperties = "00B716D7" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "974", Type = TableWidthUnitValues.Pct };
            VerticalMerge verticalMerge1 = new VerticalMerge() { Val = MergedCellValues.Restart };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(rightBorder2);
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(verticalMerge1);
            tableCellProperties1.Append(tableCellBorders1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00B716D7", RsidParagraphAddition = "00B716D7", RsidParagraphProperties = "00B716D7", RsidRunAdditionDefault = "00B716D7" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts1 = new RunFonts() { EastAsia = "Times New Roman", ComplexScriptTheme = ThemeFontValues.MinorHighAnsi };
            FontSize fontSize1 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties1.Append(runFonts1);
            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(spacingBetweenLines1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run() { RsidRunProperties = "00B716D7" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts2 = new RunFonts() { EastAsia = "Times New Roman", ComplexScriptTheme = ThemeFontValues.MinorHighAnsi };
            FontSize fontSize2 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "20" };

            runProperties1.Append(runFonts2);
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = "RANK";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "711", Type = TableWidthUnitValues.Pct };
            GridSpan gridSpan1 = new GridSpan() { Val = 4 };

            TableCellBorders tableCellBorders2 = new TableCellBorders();
            LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder3 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders2.Append(leftBorder2);
            tableCellBorders2.Append(rightBorder3);
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(gridSpan1);
            tableCellProperties2.Append(tableCellBorders2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00B716D7", RsidParagraphAddition = "00B716D7", RsidParagraphProperties = "00B716D7", RsidRunAdditionDefault = "00B716D7" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines2 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            RunFonts runFonts3 = new RunFonts() { EastAsia = "Times New Roman", ComplexScriptTheme = ThemeFontValues.MinorHighAnsi };
            FontSize fontSize3 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties2.Append(runFonts3);
            paragraphMarkRunProperties2.Append(fontSize3);
            paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

            paragraphProperties2.Append(spacingBetweenLines2);
            paragraphProperties2.Append(justification1);
            paragraphProperties2.Append(paragraphMarkRunProperties2);

            Run run2 = new Run() { RsidRunProperties = "00B716D7" };

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts4 = new RunFonts() { EastAsia = "Times New Roman", ComplexScriptTheme = ThemeFontValues.MinorHighAnsi };
            FontSize fontSize4 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "20" };

            runProperties2.Append(runFonts4);
            runProperties2.Append(fontSize4);
            runProperties2.Append(fontSizeComplexScript4);
            Text text2 = new Text();
            text2.Text = "MALE";

            run2.Append(runProperties2);
            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "663", Type = TableWidthUnitValues.Pct };
            GridSpan gridSpan2 = new GridSpan() { Val = 4 };

            TableCellBorders tableCellBorders3 = new TableCellBorders();
            LeftBorder leftBorder3 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder4 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders3.Append(leftBorder3);
            tableCellBorders3.Append(rightBorder4);
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(gridSpan2);
            tableCellProperties3.Append(tableCellBorders3);
            tableCellProperties3.Append(tableCellVerticalAlignment3);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphMarkRevision = "00B716D7", RsidParagraphAddition = "00B716D7", RsidParagraphProperties = "00B716D7", RsidRunAdditionDefault = "00B716D7" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification2 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
            RunFonts runFonts5 = new RunFonts() { EastAsia = "Times New Roman", ComplexScriptTheme = ThemeFontValues.MinorHighAnsi };
            FontSize fontSize5 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript5 = new FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties3.Append(runFonts5);
            paragraphMarkRunProperties3.Append(fontSize5);
            paragraphMarkRunProperties3.Append(fontSizeComplexScript5);

            paragraphProperties3.Append(spacingBetweenLines3);
            paragraphProperties3.Append(justification2);
            paragraphProperties3.Append(paragraphMarkRunProperties3);

            Run run3 = new Run() { RsidRunProperties = "00B716D7" };

            RunProperties runProperties3 = new RunProperties();
            RunFonts runFonts6 = new RunFonts() { EastAsia = "Times New Roman", ComplexScriptTheme = ThemeFontValues.MinorHighAnsi };
            FontSize fontSize6 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "20" };

            runProperties3.Append(runFonts6);
            runProperties3.Append(fontSize6);
            runProperties3.Append(fontSizeComplexScript6);
            Text text3 = new Text();
            text3.Text = "FEMALE";

            run3.Append(runProperties3);
            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "712", Type = TableWidthUnitValues.Pct };
            GridSpan gridSpan3 = new GridSpan() { Val = 4 };

            TableCellBorders tableCellBorders4 = new TableCellBorders();
            LeftBorder leftBorder4 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder5 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders4.Append(leftBorder4);
            tableCellBorders4.Append(rightBorder5);
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(gridSpan3);
            tableCellProperties4.Append(tableCellBorders4);
            tableCellProperties4.Append(tableCellVerticalAlignment4);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphMarkRevision = "00B716D7", RsidParagraphAddition = "00B716D7", RsidParagraphProperties = "00B716D7", RsidRunAdditionDefault = "00B716D7" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification3 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties4 = new ParagraphMarkRunProperties();
            RunFonts runFonts7 = new RunFonts() { EastAsia = "Times New Roman", ComplexScriptTheme = ThemeFontValues.MinorHighAnsi };
            FontSize fontSize7 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties4.Append(runFonts7);
            paragraphMarkRunProperties4.Append(fontSize7);
            paragraphMarkRunProperties4.Append(fontSizeComplexScript7);

            paragraphProperties4.Append(spacingBetweenLines4);
            paragraphProperties4.Append(justification3);
            paragraphProperties4.Append(paragraphMarkRunProperties4);

            Run run4 = new Run() { RsidRunProperties = "00B716D7" };

            RunProperties runProperties4 = new RunProperties();
            RunFonts runFonts8 = new RunFonts() { EastAsia = "Times New Roman", ComplexScriptTheme = ThemeFontValues.MinorHighAnsi };
            FontSize fontSize8 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript8 = new FontSizeComplexScript() { Val = "20" };

            runProperties4.Append(runFonts8);
            runProperties4.Append(fontSize8);
            runProperties4.Append(fontSizeComplexScript8);
            Text text4 = new Text();
            text4.Text = "TOTALS";

            run4.Append(runProperties4);
            run4.Append(text4);

            paragraph4.Append(paragraphProperties4);
            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "225", Type = TableWidthUnitValues.Pct };
            VerticalMerge verticalMerge2 = new VerticalMerge() { Val = MergedCellValues.Restart };

            TableCellBorders tableCellBorders5 = new TableCellBorders();
            LeftBorder leftBorder5 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder6 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders5.Append(leftBorder5);
            tableCellBorders5.Append(rightBorder6);
            TableCellVerticalAlignment tableCellVerticalAlignment5 = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(verticalMerge2);
            tableCellProperties5.Append(tableCellBorders5);
            tableCellProperties5.Append(tableCellVerticalAlignment5);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00B716D7", RsidParagraphAddition = "00B716D7", RsidParagraphProperties = "00B716D7", RsidRunAdditionDefault = "00B716D7" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines5 = new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            Justification justification4 = new Justification() { Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
            RunFonts runFonts9 = new RunFonts() { EastAsia = "Times New Roman", ComplexScriptTheme = ThemeFontValues.MinorHighAnsi };
            FontSize fontSize9 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript9 = new FontSizeComplexScript() { Val = "20" };

            paragraphMarkRunProperties5.Append(runFonts9);
            paragraphMarkRunProperties5.Append(fontSize9);
            paragraphMarkRunProperties5.Append(fontSizeComplexScript9);

            paragraphProperties5.Append(spacingBetweenLines5);
            paragraphProperties5.Append(justification4);
            paragraphProperties5.Append(paragraphMarkRunProperties5);

            Run run5 = new Run() { RsidRunProperties = "00B716D7" };

            RunProperties runProperties5 = new RunProperties();
            RunFonts runFonts10 = new RunFonts() { EastAsia = "Times New Roman", ComplexScriptTheme = ThemeFontValues.MinorHighAnsi };
            FontSize fontSize10 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript10 = new FontSizeComplexScript() { Val = "20" };

            runProperties5.Append(runFonts10);
            runProperties5.Append(fontSize10);
            runProperties5.Append(fontSizeComplexScript10);
            Text text5 = new Text();
            text5.Text = "ALL";

            run5.Append(runProperties5);
            run5.Append(text5);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);
            tableRow1.Append(tableCell4);
            tableRow1.Append(tableCell5);
            DemoTable.Append(tableRow1);
            return DemoTable;

        }
        private TableRow GenerateDemoTableRow(string ColumnName, List<Member> MemberList)
        {
             TableRow tr = new TableRow();
            // append the "Rank Name" (First Column) cell
            Paragraph p0 = new Paragraph();
            ParagraphProperties pp0 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines0 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            pp0.Append(spacingBetweenLines0);
            
            p0.Append(pp0);
            Run tr0 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp0 = new RunProperties(new Bold());
                tr0.Append(rp0);
            }            
            tr0.Append(new Text(ColumnName));
            p0.Append(tr0);
            TableCell tc0 = new TableCell();
            TableCellProperties tcp0 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs0 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp0.Append(tcs0);
            }
            tc0.Append(tcp0);
            tc0.Append(p0);
            tr.Append(tc0);

            // B/M column (this needs a 1.5pt left border)
            // set properties to apply to all text centering
            int BMCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Black" && x.Gender.GenderFullName == "Male");
            Run tr1 = new Run();                    
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp1 = new RunProperties(new Bold());
                tr1.Append(rp1);
            }
            tr1.Append(new Text(BMCount != 0 ? $"{BMCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp1 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp1.Append(spacingBetweenLines1);
            Paragraph p1 = new Paragraph(pp1);
            p1.Append(tr1);

            TableCellProperties tcp1 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs1 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp1.Append(tcs1);
            }
            TableCellBorders tcb1 = new TableCellBorders(new LeftBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U });
            tcp1.Append(tcb1);
            TableCell tc1 = new TableCell(tcp1);
                    
            tc1.Append(p1);
            tr.Append(tc1);

            // H/M column
            int HMCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Hispanic" && x.Gender.GenderFullName == "Male");
            Run tr2 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp2 = new RunProperties(new Bold());
                tr2.Append(rp2);
            }
            tr2.Append(new Text(HMCount != 0 ? $"{HMCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines2 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp2 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp2.Append(spacingBetweenLines2);

            Paragraph p2 = new Paragraph(pp2);
            p2.Append(tr2);
                    
            TableCellProperties tcp2 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs2 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp2.Append(tcs2);
            }
            TableCell tc2 = new TableCell(tcp2);                                        
                    
            tc2.Append(p2);                    
            tr.Append(tc2);

            // A/M column
            int AMCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Asian" && x.Gender.GenderFullName == "Male");
            Run tr3 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp3 = new RunProperties(new Bold());
                tr3.Append(rp3);
            }
            tr3.Append(new Text(AMCount != 0 ? $"{AMCount}" : "-"));
            
            SpacingBetweenLines spacingBetweenLines3 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp3 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp3.Append(spacingBetweenLines3);

            Paragraph p3 = new Paragraph(pp3);                                        
            p3.Append(tr3);

                    
            TableCellProperties tcp3 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });                                       
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs3 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp3.Append(tcs3);
            }
            
            TableCell tc3 = new TableCell(tcp3);
                    
            tc3.Append(p3);                    
            tr.Append(tc3);

            // W/M Column
            int WMCount = MemberList.Count(x => x.Race.MemberRaceFullName == "White" && x.Gender.GenderFullName == "Male");
            Run tr4 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp4 = new RunProperties(new Bold());
                tr4.Append(rp4);
            }        
            tr4.Append(new Text(WMCount != 0 ? $"{WMCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines4 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp4 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp4.Append(spacingBetweenLines4);

            Paragraph p4 = new Paragraph(pp4);
            p4.Append(tr4);
                                        
            TableCellProperties tcp4 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs4 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp4.Append(tcs4);
            }
            TableCellBorders tcb4 = new TableCellBorders(new RightBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U });
            tcp4.Append(tcb4);

            TableCell tc4 = new TableCell(tcp4);                    
            tc4.Append(p4);
            tr.Append(tc4);

            // B/F Column
            int BFCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Black" && x.Gender.GenderFullName == "Female");
            Run tr5 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp5 = new RunProperties(new Bold());
                tr5.Append(rp5);
            }
            tr5.Append(new Text(BFCount != 0 ? $"{BFCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines5 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp5 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp5.Append(spacingBetweenLines5);

            Paragraph p5 = new Paragraph(pp5);
            p5.Append(tr5);
                    
            TableCellProperties tcp5 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs5 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp5.Append(tcs5);
            }
            TableCell tc5 = new TableCell(tcp5);                                        
                    
            tc5.Append(p5);                    
            tr.Append(tc5);
                    
            // H/F column
            int HFCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Hispanic" && x.Gender.GenderFullName == "Female");
            Run tr6 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp6 = new RunProperties(new Bold());
                tr6.Append(rp6);
            }
            tr6.Append(new Text(HFCount != 0 ? $"{HFCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines6 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp6 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp6.Append(spacingBetweenLines6);

            Paragraph p6 = new Paragraph(pp6);
            p6.Append(tr6);
                    
            TableCellProperties tcp6 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs6 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp6.Append(tcs6);
            }
            TableCell tc6 = new TableCell(tcp6);                                        
                    
            tc6.Append(p6);                    
            tr.Append(tc6);
                    
            // A/F column
            int AFCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Asian" && x.Gender.GenderFullName == "Female");
            Run tr7 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp7 = new RunProperties(new Bold());
                tr7.Append(rp7);
            }
            tr7.Append(new Text(AFCount != 0 ? $"{AFCount}" : "-"));

            SpacingBetweenLines spacingBetweenLines7 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp7 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp7.Append(spacingBetweenLines7);

            Paragraph p7 = new Paragraph(pp7);
            p7.Append(tr7);
                    
            TableCellProperties tcp7 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs7 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp7.Append(tcs7);
            }
            TableCell tc7 = new TableCell(tcp7);                                        
                    
            tc7.Append(p7);                    
            tr.Append(tc7);

            // W/F Column
            int WFCount =  MemberList.Count(x => x.Race.MemberRaceFullName == "White" && x.Gender.GenderFullName == "Female");
            Run tr8 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp8 = new RunProperties(new Bold());
                tr8.Append(rp8);
            } 
            tr8.Append(new Text(WFCount != 0 ? $"{WFCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines8 = new SpacingBetweenLines(){ After = "0", Line = "280", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp8 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp8.Append(spacingBetweenLines8);

            Paragraph p8 = new Paragraph(pp8);
            p8.Append(tr8);
                                        
            TableCellProperties tcp8 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs8 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp8.Append(tcs8);
            }
            TableCellBorders tcb8 = new TableCellBorders(new RightBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U });
            tcp8.Append(tcb8);

            TableCell tc8 = new TableCell(tcp8);                    
            tc8.Append(p8);
            tr.Append(tc8);

            // B/T Column
            int BTCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Black");
            Run tr9 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp9 = new RunProperties(new Bold());
                tr9.Append(rp9);
            } 
            tr9.Append(new Text(BTCount != 0 ? $"{BTCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines9 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp9 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp9.Append(spacingBetweenLines9);

            Paragraph p9 = new Paragraph(pp9);
            p9.Append(tr9);
                    
            TableCellProperties tcp9 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs9 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp9.Append(tcs9);
            }
            TableCell tc9 = new TableCell(tcp9);                                        
                    
            tc9.Append(p9);                    
            tr.Append(tc9);

            // H/T Column
            int HTCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Hispanic");
            Run tr10 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp10 = new RunProperties(new Bold());
                tr10.Append(rp10);
            } 
            tr10.Append(new Text(HTCount != 0 ? $"{HTCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines10 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp10 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp10.Append(spacingBetweenLines10);

            Paragraph p10 = new Paragraph(pp10);
            p10.Append(tr10);
                    
            TableCellProperties tcp10 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs10 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp10.Append(tcs10);
            }
            TableCell tc10 = new TableCell(tcp10);                                        
                    
            tc10.Append(p10);                    
            tr.Append(tc10);

            // A/T Column
            int ATCount = MemberList.Count(x => x.Race.MemberRaceFullName == "Asian");
            Run tr11 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp11 = new RunProperties(new Bold());
                tr11.Append(rp11);
            } 
            tr11.Append(new Text(ATCount != 0 ? $"{ATCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines11 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp11 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp11.Append(spacingBetweenLines11);

            Paragraph p11 = new Paragraph(pp11);
            p11.Append(tr11);
                    
            TableCellProperties tcp11 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs11 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp11.Append(tcs11);
            }
            TableCell tc11 = new TableCell(tcp11);                                        
                    
            tc11.Append(p11);                    
            tr.Append(tc11);

            // W/T Column
            int WTCount = MemberList.Count(x => x.Race.MemberRaceFullName == "White");
            Run tr12 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp12 = new RunProperties(new Bold());
                tr12.Append(rp12);
            }         
            tr12.Append(new Text(WTCount != 0 ? $"{WTCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines12 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp12 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp12.Append(spacingBetweenLines12);

            Paragraph p12 = new Paragraph(pp12);
            p12.Append(tr12);
                                        
            TableCellProperties tcp12 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs12 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp12.Append(tcs12);
            }
            TableCellBorders tcb12 = new TableCellBorders(new RightBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U });
            tcp12.Append(tcb12);

            TableCell tc12 = new TableCell(tcp12);                    
            tc12.Append(p12);
            tr.Append(tc12);

            // TotalForRank column
            int RankTotalCount = MemberList.Count;
            Run tr13 = new Run();
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                RunProperties rp13 = new RunProperties(new Bold());
                tr13.Append(rp13);
            }  
            tr13.Append(new Text(RankTotalCount != 0 ? $"{RankTotalCount}" : "-"));
            SpacingBetweenLines spacingBetweenLines13 = new SpacingBetweenLines(){ After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto };
            ParagraphProperties pp13 = new ParagraphProperties(new Justification(){ Val = JustificationValues.Center });
            pp13.Append(spacingBetweenLines13);

            Paragraph p13 = new Paragraph(pp13);
            p13.Append(tr13);
                                        
            TableCellProperties tcp13 = new TableCellProperties(new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center });
            if (ColumnName == "Sworn" || ColumnName == "Total")
            {
                Shading tcs13 = new Shading(){ Val = ShadingPatternValues.Clear, Color = "auto", Fill = "D9D9D9", ThemeFill = ThemeColorValues.Background1, ThemeFillShade = "D9" };
                tcp13.Append(tcs13);
            }
            TableCellBorders tcb13 = new TableCellBorders(new RightBorder(){ Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)13U, Space = (UInt32Value)0U });
            tcp13.Append(tcb13);

            TableCell tc13 = new TableCell(tcp13);                    
            tc13.Append(p13);
            tr.Append(tc13);
            return tr;
        }

        private List<Member> RecurseForMembers(Component _component, List<Member> _memberList)
        {
            
            if (Components.Any(x => x.ParentComponent.ComponentId == _component.ComponentId))
            {
                List<Component> children = Components.Where(x => x.ParentComponent.ComponentId == _component.ComponentId).ToList();
                foreach (Component c in children)
                {
                    RecurseForMembers(c, _memberList);
                }
            }
            if (_component.Positions != null)
            {
                foreach (Position p in _component.Positions)
                {
                    foreach (Member m in p.Members)
                    {
                        _memberList.Add(m);
                    }
                }
            }
            
            return _memberList;
        }
    }

    
}
