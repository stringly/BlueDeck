using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace OrgChartDemo.Models.DocGenerators
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
                HeaderPart headerPart1 = mainPart.AddNewPart<HeaderPart>();
                GenerateHeaderPart1Content(headerPart1);
                FooterPart footerPart1 = mainPart.AddNewPart<FooterPart>();
                GenerateFooterPartContent(footerPart1);
                Table initialTable = mainPart.Document.Body.Elements<Table>().ElementAt(0);
                foreach(Component c in Components)
                {
                    mainPart.Document.Body.Append(GenerateComponentTable(c));
                }
                mainPart.Document.Save();
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;

        }
        // Generates content of headerPart1.
        private void GenerateHeaderPart1Content(HeaderPart headerPart1)
        {
            Header header1 = new Header();

            Paragraph paragraph1 = new Paragraph();

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId(){ Val = "Header" };
            Justification justification1 = new Justification(){ Val = JustificationValues.Center };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            FontSize fontSize1 = new FontSize(){ Val = "28" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript(){ Val = "28" };

            paragraphMarkRunProperties1.Append(fontSize1);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

            paragraphProperties1.Append(paragraphStyleId1);
            paragraphProperties1.Append(justification1);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            FontSize fontSize2 = new FontSize(){ Val = "28" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript(){ Val = "28" };

            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);
            Text text1 = new Text();
            text1.Text = $"{Components.First().Name} Personnel Roster";

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            Paragraph paragraph2 = new Paragraph();

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId2 = new ParagraphStyleId(){ Val = "Header" };
            Justification justification2 = new Justification(){ Val = JustificationValues.Center };

            paragraphProperties2.Append(paragraphStyleId2);
            paragraphProperties2.Append(justification2);

            Run run2 = new Run();
            Text text2 = new Text();
            text2.Text = $"{DateTime.Now.ToString("MMMM dd, yyyy")}";

            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            header1.Append(paragraph1);
            header1.Append(paragraph2);

            headerPart1.Header = header1;
        }

        // Generates content of part.
        private void GenerateFooterPartContent(FooterPart part)
        {
            Footer footer1 = new Footer();

            Paragraph paragraph1 = new Paragraph();

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId(){ Val = "Footer" };

            paragraphProperties1.Append(paragraphStyleId1);

            Run run1 = new Run();
            Text text1 = new Text();
            text1.Text = $"{Components.First().Name} Personnel Roster";

            run1.Append(text1);

            Run run2 = new Run();
            Text text2 = new Text(){ Space = SpaceProcessingModeValues.Preserve };
            text2.Text = $" – {DateTime.Now.ToString("MMMM dd, yyyy")}";

            run2.Append(text2);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);
            paragraph1.Append(run2);

            footer1.Append(paragraph1);

            part.Footer = footer1;
        }

        public Table GenerateComponentTable(Component c)
        {
            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle(){ Val = "TableGrid" };
            TableWidth tableWidth1 = new TableWidth(){ Width = "14400", Type = TableWidthUnitValues.Dxa };

            TableBorders tableBorders1 = new TableBorders();
            TopBorder topBorder1 = new TopBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            LeftBorder leftBorder1 = new LeftBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder1 = new BottomBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            RightBorder rightBorder1 = new RightBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder1 = new InsideHorizontalBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder1 = new InsideVerticalBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };

            tableBorders1.Append(topBorder1);
            tableBorders1.Append(leftBorder1);
            tableBorders1.Append(bottomBorder1);
            tableBorders1.Append(rightBorder1);
            tableBorders1.Append(insideHorizontalBorder1);
            tableBorders1.Append(insideVerticalBorder1);
            TableLook tableLook1 = new TableLook(){ Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableBorders1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn(){ Width = "450" };
            GridColumn gridColumn2 = new GridColumn(){ Width = "13950" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);

            TableRow tableRow1 = new TableRow();

            TableRowProperties tableRowProperties1 = new TableRowProperties();
            CantSplit cantSplit1 = new CantSplit();
            TableRowHeight tableRowHeight1 = new TableRowHeight(){ Val = (UInt32Value)603U };

            tableRowProperties1.Append(cantSplit1);
            tableRowProperties1.Append(tableRowHeight1);

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth(){ Width = "14400", Type = TableWidthUnitValues.Dxa };
            GridSpan gridSpan1 = new GridSpan(){ Val = 2 };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            TopBorder topBorder2 = new TopBorder(){ Val = BorderValues.Single, Color = "767171", ThemeColor = ThemeColorValues.Background2, ThemeShade = "80", Size = (UInt32Value)18U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(topBorder2);
            TableCellVerticalAlignment tableCellVerticalAlignment1 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(gridSpan1);
            tableCellProperties1.Append(tableCellBorders1);
            tableCellProperties1.Append(tableCellVerticalAlignment1);

            Paragraph paragraph1 = new Paragraph();
            BookmarkStart bookmarkStart1 = new BookmarkStart(){ Name = $"{c.Name}", Id = $"{c.ComponentId}"};
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd(){Id = $"{c.ComponentId}"};

            Run run1 = new Run();
            RunProperties runProperties1 = new RunProperties();
            FontSize fontSize1 = new FontSize(){ Val = "32" };
            runProperties1.Append(fontSize1);
            Text text1 = new Text();
            text1.Text = $"{c.Name}";
            run1.Append(runProperties1);
            run1.Append(text1);


            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            KeepNext keepNext1 = new KeepNext();
            KeepLines keepLines1 = new KeepLines();

            paragraphProperties1.Append(keepNext1);
            paragraphProperties1.Append(keepLines1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(bookmarkStart1);
            paragraph1.Append(bookmarkEnd1);
            paragraph1.Append(run1);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell1);

            TableRow tableRow2 = new TableRow();

            TableRowProperties tableRowProperties2 = new TableRowProperties();
            CantSplit cantSplit2 = new CantSplit();
            TableRowHeight tableRowHeight2 = new TableRowHeight(){ Val = (UInt32Value)1080U };

            tableRowProperties2.Append(cantSplit2);
            tableRowProperties2.Append(tableRowHeight2);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth(){ Width = "450", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment2 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(tableCellVerticalAlignment2);

            Paragraph paragraph2 = new Paragraph();

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            KeepNext keepNext2 = new KeepNext();
            KeepLines keepLines2 = new KeepLines();

            paragraphProperties2.Append(keepNext2);
            paragraphProperties2.Append(keepLines2);

            paragraph2.Append(paragraphProperties2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth(){ Width = "13950", Type = TableWidthUnitValues.Dxa };

            tableCellProperties3.Append(tableCellWidth3);

            

            Paragraph paragraph8 = new Paragraph();

            ParagraphProperties paragraphProperties8 = new ParagraphProperties();
            KeepLines keepLines8 = new KeepLines();

            paragraphProperties8.Append(keepLines8);

            paragraph8.Append(paragraphProperties8);

            tableCell3.Append(tableCellProperties3);
            // TODO: Append Positions table here
            tableCell3.Append(GeneratePositionsSubTable(c.Positions.ToList()));
            tableCell3.Append(paragraph8);

            tableRow2.Append(tableRowProperties2);
            tableRow2.Append(tableCell2);
            tableRow2.Append(tableCell3);

            TableRow tableRow4 = new TableRow();

            TableRowProperties tableRowProperties3 = new TableRowProperties();
            CantSplit cantSplit3 = new CantSplit();
            TableRowHeight tableRowHeight3 = new TableRowHeight(){ Val = (UInt32Value)1233U };

            tableRowProperties3.Append(cantSplit3);
            tableRowProperties3.Append(tableRowHeight3);

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth(){ Width = "450", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment3 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties9.Append(tableCellWidth9);
            tableCellProperties9.Append(tableCellVerticalAlignment3);

            Paragraph paragraph9 = new Paragraph();

            ParagraphProperties paragraphProperties9 = new ParagraphProperties();
            KeepNext keepNext3 = new KeepNext();
            KeepLines keepLines9 = new KeepLines();

            paragraphProperties9.Append(keepNext3);
            paragraphProperties9.Append(keepLines9);

            paragraph9.Append(paragraphProperties9);

            tableCell9.Append(tableCellProperties9);
            tableCell9.Append(paragraph9);

            TableCell tableCell10 = new TableCell();

            TableCellProperties tableCellProperties10 = new TableCellProperties();
            TableCellWidth tableCellWidth10 = new TableCellWidth(){ Width = "13950", Type = TableWidthUnitValues.Dxa };

            tableCellProperties10.Append(tableCellWidth10);

            Table table3 = new Table();

            TableProperties tableProperties3 = new TableProperties();
            TableStyle tableStyle3 = new TableStyle(){ Val = "TableGrid" };
            TableWidth tableWidth3 = new TableWidth(){ Width = "0", Type = TableWidthUnitValues.Auto };

            TableBorders tableBorders3 = new TableBorders();
            TopBorder topBorder4 = new TopBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            LeftBorder leftBorder3 = new LeftBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder3 = new BottomBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            RightBorder rightBorder3 = new RightBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder3 = new InsideHorizontalBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder3 = new InsideVerticalBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };

            tableBorders3.Append(topBorder4);
            tableBorders3.Append(leftBorder3);
            tableBorders3.Append(bottomBorder3);
            tableBorders3.Append(rightBorder3);
            tableBorders3.Append(insideHorizontalBorder3);
            tableBorders3.Append(insideVerticalBorder3);
            TableLook tableLook3 = new TableLook(){ Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties3.Append(tableStyle3);
            tableProperties3.Append(tableWidth3);
            tableProperties3.Append(tableBorders3);
            tableProperties3.Append(tableLook3);

            TableGrid tableGrid3 = new TableGrid();
            GridColumn gridColumn8 = new GridColumn(){ Width = "2744" };
            GridColumn gridColumn9 = new GridColumn(){ Width = "2745" };
            GridColumn gridColumn10 = new GridColumn(){ Width = "2745" };
            GridColumn gridColumn11 = new GridColumn(){ Width = "2745" };
            GridColumn gridColumn12 = new GridColumn(){ Width = "2745" };

            tableGrid3.Append(gridColumn8);
            tableGrid3.Append(gridColumn9);
            tableGrid3.Append(gridColumn10);
            tableGrid3.Append(gridColumn11);
            tableGrid3.Append(gridColumn12);

            TableRow tableRow5 = new TableRow();

            TableCell tableCell11 = new TableCell();

            TableCellProperties tableCellProperties11 = new TableCellProperties();
            TableCellWidth tableCellWidth11 = new TableCellWidth(){ Width = "2744", Type = TableWidthUnitValues.Dxa };

            tableCellProperties11.Append(tableCellWidth11);

            Paragraph paragraph10 = new Paragraph();

            ParagraphProperties paragraphProperties10 = new ParagraphProperties();
            KeepLines keepLines10 = new KeepLines();

            paragraphProperties10.Append(keepLines10);

            paragraph10.Append(paragraphProperties10);

            tableCell11.Append(tableCellProperties11);
            tableCell11.Append(paragraph10);

            TableCell tableCell12 = new TableCell();

            TableCellProperties tableCellProperties12 = new TableCellProperties();
            TableCellWidth tableCellWidth12 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties12.Append(tableCellWidth12);

            Paragraph paragraph11 = new Paragraph();

            ParagraphProperties paragraphProperties11 = new ParagraphProperties();
            KeepLines keepLines11 = new KeepLines();

            paragraphProperties11.Append(keepLines11);

            paragraph11.Append(paragraphProperties11);

            tableCell12.Append(tableCellProperties12);
            tableCell12.Append(paragraph11);

            TableCell tableCell13 = new TableCell();

            TableCellProperties tableCellProperties13 = new TableCellProperties();
            TableCellWidth tableCellWidth13 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties13.Append(tableCellWidth13);

            Paragraph paragraph12 = new Paragraph();

            ParagraphProperties paragraphProperties12 = new ParagraphProperties();
            KeepLines keepLines12 = new KeepLines();

            paragraphProperties12.Append(keepLines12);

            paragraph12.Append(paragraphProperties12);

            tableCell13.Append(tableCellProperties13);
            tableCell13.Append(paragraph12);

            TableCell tableCell14 = new TableCell();

            TableCellProperties tableCellProperties14 = new TableCellProperties();
            TableCellWidth tableCellWidth14 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties14.Append(tableCellWidth14);

            Paragraph paragraph13 = new Paragraph();

            ParagraphProperties paragraphProperties13 = new ParagraphProperties();
            KeepLines keepLines13 = new KeepLines();

            paragraphProperties13.Append(keepLines13);

            paragraph13.Append(paragraphProperties13);

            tableCell14.Append(tableCellProperties14);
            tableCell14.Append(paragraph13);

            TableCell tableCell15 = new TableCell();

            TableCellProperties tableCellProperties15 = new TableCellProperties();
            TableCellWidth tableCellWidth15 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties15.Append(tableCellWidth15);

            Paragraph paragraph14 = new Paragraph();

            ParagraphProperties paragraphProperties14 = new ParagraphProperties();
            KeepLines keepLines14 = new KeepLines();

            paragraphProperties14.Append(keepLines14);

            paragraph14.Append(paragraphProperties14);

            tableCell15.Append(tableCellProperties15);
            tableCell15.Append(paragraph14);

            tableRow5.Append(tableCell11);
            tableRow5.Append(tableCell12);
            tableRow5.Append(tableCell13);
            tableRow5.Append(tableCell14);
            tableRow5.Append(tableCell15);

            table3.Append(tableProperties3);
            table3.Append(tableGrid3);
            table3.Append(tableRow5);

            Paragraph paragraph15 = new Paragraph();

            ParagraphProperties paragraphProperties15 = new ParagraphProperties();
            KeepLines keepLines15 = new KeepLines();

            paragraphProperties15.Append(keepLines15);

            paragraph15.Append(paragraphProperties15);

            tableCell10.Append(tableCellProperties10);
            tableCell10.Append(table3);
            tableCell10.Append(paragraph15);

            tableRow4.Append(tableRowProperties3);
            tableRow4.Append(tableCell9);
            tableRow4.Append(tableCell10);

            TableRow tableRow6 = new TableRow();

            TableRowProperties tableRowProperties4 = new TableRowProperties();
            CantSplit cantSplit4 = new CantSplit();
            TableRowHeight tableRowHeight4 = new TableRowHeight(){ Val = (UInt32Value)1233U };

            tableRowProperties4.Append(cantSplit4);
            tableRowProperties4.Append(tableRowHeight4);

            TableCell tableCell16 = new TableCell();

            TableCellProperties tableCellProperties16 = new TableCellProperties();
            TableCellWidth tableCellWidth16 = new TableCellWidth(){ Width = "450", Type = TableWidthUnitValues.Dxa };
            TableCellVerticalAlignment tableCellVerticalAlignment4 = new TableCellVerticalAlignment(){ Val = TableVerticalAlignmentValues.Center };

            tableCellProperties16.Append(tableCellWidth16);
            tableCellProperties16.Append(tableCellVerticalAlignment4);

            Paragraph paragraph16 = new Paragraph();

            ParagraphProperties paragraphProperties16 = new ParagraphProperties();
            KeepNext keepNext4 = new KeepNext();
            KeepLines keepLines16 = new KeepLines();

            paragraphProperties16.Append(keepNext4);
            paragraphProperties16.Append(keepLines16);

            paragraph16.Append(paragraphProperties16);

            tableCell16.Append(tableCellProperties16);
            tableCell16.Append(paragraph16);

            TableCell tableCell17 = new TableCell();

            TableCellProperties tableCellProperties17 = new TableCellProperties();
            TableCellWidth tableCellWidth17 = new TableCellWidth(){ Width = "13950", Type = TableWidthUnitValues.Dxa };

            tableCellProperties17.Append(tableCellWidth17);

            Table table4 = new Table();

            TableProperties tableProperties4 = new TableProperties();
            TableStyle tableStyle4 = new TableStyle(){ Val = "TableGrid" };
            TableWidth tableWidth4 = new TableWidth(){ Width = "0", Type = TableWidthUnitValues.Auto };

            TableBorders tableBorders4 = new TableBorders();
            TopBorder topBorder5 = new TopBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            LeftBorder leftBorder4 = new LeftBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder4 = new BottomBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            RightBorder rightBorder4 = new RightBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder4 = new InsideHorizontalBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder4 = new InsideVerticalBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };

            tableBorders4.Append(topBorder5);
            tableBorders4.Append(leftBorder4);
            tableBorders4.Append(bottomBorder4);
            tableBorders4.Append(rightBorder4);
            tableBorders4.Append(insideHorizontalBorder4);
            tableBorders4.Append(insideVerticalBorder4);
            TableLook tableLook4 = new TableLook(){ Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties4.Append(tableStyle4);
            tableProperties4.Append(tableWidth4);
            tableProperties4.Append(tableBorders4);
            tableProperties4.Append(tableLook4);

            TableGrid tableGrid4 = new TableGrid();
            GridColumn gridColumn13 = new GridColumn(){ Width = "2744" };
            GridColumn gridColumn14 = new GridColumn(){ Width = "2745" };
            GridColumn gridColumn15 = new GridColumn(){ Width = "2745" };
            GridColumn gridColumn16 = new GridColumn(){ Width = "2745" };
            GridColumn gridColumn17 = new GridColumn(){ Width = "2745" };

            tableGrid4.Append(gridColumn13);
            tableGrid4.Append(gridColumn14);
            tableGrid4.Append(gridColumn15);
            tableGrid4.Append(gridColumn16);
            tableGrid4.Append(gridColumn17);

            TableRow tableRow7 = new TableRow();

            TableCell tableCell18 = new TableCell();

            TableCellProperties tableCellProperties18 = new TableCellProperties();
            TableCellWidth tableCellWidth18 = new TableCellWidth(){ Width = "2744", Type = TableWidthUnitValues.Dxa };

            tableCellProperties18.Append(tableCellWidth18);

            Paragraph paragraph17 = new Paragraph();

            ParagraphProperties paragraphProperties17 = new ParagraphProperties();
            KeepLines keepLines17 = new KeepLines();

            paragraphProperties17.Append(keepLines17);

            paragraph17.Append(paragraphProperties17);

            tableCell18.Append(tableCellProperties18);
            tableCell18.Append(paragraph17);

            TableCell tableCell19 = new TableCell();

            TableCellProperties tableCellProperties19 = new TableCellProperties();
            TableCellWidth tableCellWidth19 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties19.Append(tableCellWidth19);

            Paragraph paragraph18 = new Paragraph();

            ParagraphProperties paragraphProperties18 = new ParagraphProperties();
            KeepLines keepLines18 = new KeepLines();

            paragraphProperties18.Append(keepLines18);

            paragraph18.Append(paragraphProperties18);

            tableCell19.Append(tableCellProperties19);
            tableCell19.Append(paragraph18);

            TableCell tableCell20 = new TableCell();

            TableCellProperties tableCellProperties20 = new TableCellProperties();
            TableCellWidth tableCellWidth20 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties20.Append(tableCellWidth20);

            Paragraph paragraph19 = new Paragraph();

            ParagraphProperties paragraphProperties19 = new ParagraphProperties();
            KeepLines keepLines19 = new KeepLines();

            paragraphProperties19.Append(keepLines19);

            paragraph19.Append(paragraphProperties19);

            tableCell20.Append(tableCellProperties20);
            tableCell20.Append(paragraph19);

            TableCell tableCell21 = new TableCell();

            TableCellProperties tableCellProperties21 = new TableCellProperties();
            TableCellWidth tableCellWidth21 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties21.Append(tableCellWidth21);

            Paragraph paragraph20 = new Paragraph();

            ParagraphProperties paragraphProperties20 = new ParagraphProperties();
            KeepLines keepLines20 = new KeepLines();

            paragraphProperties20.Append(keepLines20);

            paragraph20.Append(paragraphProperties20);

            tableCell21.Append(tableCellProperties21);
            tableCell21.Append(paragraph20);

            TableCell tableCell22 = new TableCell();

            TableCellProperties tableCellProperties22 = new TableCellProperties();
            TableCellWidth tableCellWidth22 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties22.Append(tableCellWidth22);

            Paragraph paragraph21 = new Paragraph();

            ParagraphProperties paragraphProperties21 = new ParagraphProperties();
            KeepLines keepLines21 = new KeepLines();

            paragraphProperties21.Append(keepLines21);

            paragraph21.Append(paragraphProperties21);

            tableCell22.Append(tableCellProperties22);
            tableCell22.Append(paragraph21);

            tableRow7.Append(tableCell18);
            tableRow7.Append(tableCell19);
            tableRow7.Append(tableCell20);
            tableRow7.Append(tableCell21);
            tableRow7.Append(tableCell22);

            table4.Append(tableProperties4);
            table4.Append(tableGrid4);
            table4.Append(tableRow7);

            Paragraph paragraph22 = new Paragraph();

            ParagraphProperties paragraphProperties22 = new ParagraphProperties();
            KeepLines keepLines22 = new KeepLines();

            paragraphProperties22.Append(keepLines22);

            paragraph22.Append(paragraphProperties22);

            tableCell17.Append(tableCellProperties17);
            tableCell17.Append(table4);
            tableCell17.Append(paragraph22);

            tableRow6.Append(tableRowProperties4);
            tableRow6.Append(tableCell16);
            tableRow6.Append(tableCell17);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);
            table1.Append(tableRow2);
            table1.Append(tableRow4);
            table1.Append(tableRow6);
            return table1;
        }

        private Table GeneratePositionsSubTable(List<Position> positions)
        {
            Table table2 = new Table();

            TableProperties tableProperties2 = new TableProperties();
            TableStyle tableStyle2 = new TableStyle(){ Val = "TableGrid" };
            TableWidth tableWidth2 = new TableWidth(){ Width = "0", Type = TableWidthUnitValues.Auto };

            TableBorders tableBorders2 = new TableBorders();
            TopBorder topBorder3 = new TopBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            LeftBorder leftBorder2 = new LeftBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder2 = new BottomBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            RightBorder rightBorder2 = new RightBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideHorizontalBorder insideHorizontalBorder2 = new InsideHorizontalBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };
            InsideVerticalBorder insideVerticalBorder2 = new InsideVerticalBorder(){ Val = BorderValues.None, Color = "auto", Size = (UInt32Value)0U, Space = (UInt32Value)0U };

            tableBorders2.Append(topBorder3);
            tableBorders2.Append(leftBorder2);
            tableBorders2.Append(bottomBorder2);
            tableBorders2.Append(rightBorder2);
            tableBorders2.Append(insideHorizontalBorder2);
            tableBorders2.Append(insideVerticalBorder2);
            TableLook tableLook2 = new TableLook(){ Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties2.Append(tableStyle2);
            tableProperties2.Append(tableWidth2);
            tableProperties2.Append(tableBorders2);
            tableProperties2.Append(tableLook2);

            TableGrid tableGrid2 = new TableGrid();
            GridColumn gridColumn3 = new GridColumn(){ Width = "2744" };
            GridColumn gridColumn4 = new GridColumn(){ Width = "2745" };
            GridColumn gridColumn5 = new GridColumn(){ Width = "2745" };
            GridColumn gridColumn6 = new GridColumn(){ Width = "2745" };
            GridColumn gridColumn7 = new GridColumn(){ Width = "2745" };

            tableGrid2.Append(gridColumn3);
            tableGrid2.Append(gridColumn4);
            tableGrid2.Append(gridColumn5);
            tableGrid2.Append(gridColumn6);
            tableGrid2.Append(gridColumn7);

            // Header Row
            TableRow tableRow3 = new TableRow();

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth(){ Width = "2744", Type = TableWidthUnitValues.Dxa };

            tableCellProperties4.Append(tableCellWidth4);

            Paragraph paragraph3 = new Paragraph();

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            KeepLines keepLines3 = new KeepLines();

            paragraphProperties3.Append(keepLines3);

            paragraph3.Append(paragraphProperties3);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph3);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties5.Append(tableCellWidth5);

            Paragraph paragraph4 = new Paragraph();

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            KeepLines keepLines4 = new KeepLines();

            paragraphProperties4.Append(keepLines4);

            paragraph4.Append(paragraphProperties4);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph4);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties6.Append(tableCellWidth6);

            Paragraph paragraph5 = new Paragraph();

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            KeepLines keepLines5 = new KeepLines();

            paragraphProperties5.Append(keepLines5);

            paragraph5.Append(paragraphProperties5);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph5);

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            TableCellWidth tableCellWidth7 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties7.Append(tableCellWidth7);

            Paragraph paragraph6 = new Paragraph();

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            KeepLines keepLines6 = new KeepLines();

            paragraphProperties6.Append(keepLines6);

            paragraph6.Append(paragraphProperties6);

            tableCell7.Append(tableCellProperties7);
            tableCell7.Append(paragraph6);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth(){ Width = "2745", Type = TableWidthUnitValues.Dxa };

            tableCellProperties8.Append(tableCellWidth8);

            Paragraph paragraph7 = new Paragraph();

            ParagraphProperties paragraphProperties7 = new ParagraphProperties();
            KeepLines keepLines7 = new KeepLines();

            paragraphProperties7.Append(keepLines7);

            paragraph7.Append(paragraphProperties7);

            tableCell8.Append(tableCellProperties8);
            tableCell8.Append(paragraph7);

            tableRow3.Append(tableCell4);
            tableRow3.Append(tableCell5);
            tableRow3.Append(tableCell6);
            tableRow3.Append(tableCell7);
            tableRow3.Append(tableCell8);
            table2.Append(tableProperties2);
            table2.Append(tableGrid2);
            table2.Append(tableRow3);
            
            foreach(Position p in positions)
            {

            }

            return table2;
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
    }

    
}
